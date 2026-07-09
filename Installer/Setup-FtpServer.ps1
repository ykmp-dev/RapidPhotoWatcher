<#
.SYNOPSIS
    RapidPhotoWatcher 用 PC側FTPサーバー(IIS FTP)自動セットアップスクリプト

.DESCRIPTION
    カメラ(Canon/Nikon/Sony等)からのFTP転送を受信できるように、
    Windows標準のIIS FTPサーバーを自動でセットアップします。

    実行内容:
      1. IIS FTPサーバー機能(Windows機能)の有効化
      2. FTP受信フォルダの作成
      3. カメラ接続用ローカルユーザーの作成(Windowsサインイン画面には非表示)
      4. IIS FTPサイトの作成(基本認証・SSL任意)
      5. Windowsファイアウォールの開放(制御ポート + パッシブポート)
      6. FTPサービス(ftpsvc)の自動起動設定

    ※ 管理者権限で実行してください。

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File .\Setup-FtpServer.ps1
    powershell -ExecutionPolicy Bypass -File .\Setup-FtpServer.ps1 -FtpRoot "D:\Photos\FTP" -UserName camera -Port 21
#>
[CmdletBinding()]
param(
    # FTP受信フォルダ(= RapidPhotoWatcher の監視フォルダに推奨)
    [string]$FtpRoot = (Join-Path ([Environment]::GetFolderPath('MyPictures')) 'RapidPhotoWatcher\FTP'),

    # カメラに設定するFTPユーザー名
    [string]$UserName = 'camera',

    # カメラに設定するFTPパスワード(未指定の場合は対話入力)
    [string]$Password,

    # FTP制御ポート
    [ValidateRange(1, 65535)]
    [int]$Port = 21,

    # IISに作成するFTPサイト名
    [string]$SiteName = 'RapidPhotoWatcher-FTP',

    # パッシブモード用データポート範囲
    [int]$PassivePortStart = 60000,
    [int]$PassivePortEnd = 60100,

    # 完了時にキー入力待ちをしない(サイレント実行用)
    [switch]$NoPause
)

$ErrorActionPreference = 'Stop'

$FirewallRuleControl = 'RapidPhotoWatcher FTP (Control Port)'
$FirewallRulePassive = 'RapidPhotoWatcher FTP (Passive Ports)'

function Write-Step([string]$Message) {
    Write-Host ''
    Write-Host "==> $Message" -ForegroundColor Cyan
}

function Exit-Script([int]$Code) {
    if (-not $NoPause) {
        Write-Host ''
        Read-Host '確認したら Enter キーを押してください'
    }
    exit $Code
}

# ログ(トラブルシュート用)
$logPath = Join-Path $env:TEMP 'RapidPhotoWatcher-FtpSetup.log'
try { Start-Transcript -Path $logPath -Append | Out-Null } catch { }

try {
    Write-Host '=========================================================='
    Write-Host '  RapidPhotoWatcher - FTPサーバー自動セットアップ'
    Write-Host '=========================================================='

    # --- 管理者権限チェック ---
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($identity)
    if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
        Write-Host 'エラー: このスクリプトは管理者権限で実行する必要があります。' -ForegroundColor Red
        Write-Host 'PowerShellを「管理者として実行」で開いてから再実行してください。' -ForegroundColor Red
        Exit-Script 1
    }

    # --- パスワード未指定なら対話入力 ---
    if ([string]::IsNullOrEmpty($Password)) {
        $secureInput = Read-Host "FTPユーザー [$UserName] のパスワードを入力してください" -AsSecureString
        $Password = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
            [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureInput))
        if ([string]::IsNullOrEmpty($Password)) {
            Write-Host 'エラー: パスワードが入力されませんでした。' -ForegroundColor Red
            Exit-Script 1
        }
    }

    # --- 1. IIS FTP 機能の有効化 ---
    Write-Step 'IIS FTPサーバー機能を確認・有効化しています(数分かかる場合があります)...'
    $features = @(
        'IIS-WebServerRole',
        'IIS-WebServerManagementTools',
        'IIS-ManagementConsole',
        'IIS-FTPServer',
        'IIS-FTPSvc'
    )
    foreach ($feature in $features) {
        $state = (Get-WindowsOptionalFeature -Online -FeatureName $feature).State
        if ($state -ne 'Enabled') {
            Write-Host "  機能を有効化: $feature"
            Enable-WindowsOptionalFeature -Online -FeatureName $feature -All -NoRestart | Out-Null
        } else {
            Write-Host "  有効化済み  : $feature"
        }
    }

    # --- 2. FTP受信フォルダの作成 ---
    Write-Step "FTP受信フォルダを作成しています: $FtpRoot"
    if (-not (Test-Path -LiteralPath $FtpRoot)) {
        New-Item -ItemType Directory -Path $FtpRoot -Force | Out-Null
    }

    # --- 3. カメラ接続用ローカルユーザーの作成 ---
    Write-Step "FTPユーザーを作成しています: $UserName"
    $securePassword = ConvertTo-SecureString $Password -AsPlainText -Force
    $existingUser = Get-LocalUser -Name $UserName -ErrorAction SilentlyContinue
    if ($existingUser) {
        Write-Host '  ユーザーは既に存在するため、パスワードを更新します。'
        Set-LocalUser -Name $UserName -Password $securePassword -PasswordNeverExpires $true
    } else {
        New-LocalUser -Name $UserName `
                      -Password $securePassword `
                      -FullName 'RapidPhotoWatcher FTP User' `
                      -Description 'RapidPhotoWatcher: カメラからのFTP転送受信用アカウント' `
                      -PasswordNeverExpires `
                      -AccountNeverExpires `
                      -UserMayNotChangePassword | Out-Null
    }

    # Windowsサインイン画面にこのユーザーを表示しない
    $userListKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList'
    if (-not (Test-Path $userListKey)) {
        New-Item -Path $userListKey -Force | Out-Null
    }
    New-ItemProperty -Path $userListKey -Name $UserName -PropertyType DWord -Value 0 -Force | Out-Null

    # FTP受信フォルダへの書き込み権限を付与
    & icacls "$FtpRoot" /grant "${UserName}:(OI)(CI)M" /T | Out-Null

    # --- 4. IIS FTPサイトの作成 ---
    Write-Step "IIS FTPサイトを作成しています: $SiteName (ポート $Port)"
    Import-Module WebAdministration

    if (Get-Website -Name $SiteName -ErrorAction SilentlyContinue) {
        Write-Host '  既存のFTPサイトを再作成します。'
        Remove-Website -Name $SiteName
    }
    New-WebFtpSite -Name $SiteName -Port $Port -PhysicalPath $FtpRoot -Force | Out-Null

    $sitePath = "IIS:\Sites\$SiteName"
    # 基本認証を有効化・匿名認証を無効化
    Set-ItemProperty $sitePath -Name 'ftpServer.security.authentication.basicAuthentication.enabled' -Value $true
    Set-ItemProperty $sitePath -Name 'ftpServer.security.authentication.anonymousAuthentication.enabled' -Value $false
    # カメラは平文FTPが主流のためSSLは「任意」に設定(SSL対応カメラはFTPSも利用可)
    Set-ItemProperty $sitePath -Name 'ftpServer.security.ssl.controlChannelPolicy' -Value 'SslAllow'
    Set-ItemProperty $sitePath -Name 'ftpServer.security.ssl.dataChannelPolicy' -Value 'SslAllow'

    # FTPユーザーに読み取り/書き込みを許可
    Clear-WebConfiguration -Filter '/system.ftpServer/security/authorization' -PSPath 'IIS:\' -Location $SiteName -ErrorAction SilentlyContinue
    Add-WebConfiguration -Filter '/system.ftpServer/security/authorization' -PSPath 'IIS:\' -Location $SiteName -Value @{
        accessType  = 'Allow'
        users       = $UserName
        permissions = 'Read,Write'
    }

    # パッシブモード用データポート範囲(サーバー全体設定)
    Set-WebConfigurationProperty -PSPath 'IIS:\' -Filter 'system.ftpServer/firewallSupport' -Name 'lowDataChannelPort'  -Value $PassivePortStart
    Set-WebConfigurationProperty -PSPath 'IIS:\' -Filter 'system.ftpServer/firewallSupport' -Name 'highDataChannelPort' -Value $PassivePortEnd

    # --- 5. Windowsファイアウォールの開放 ---
    Write-Step 'Windowsファイアウォールを設定しています...'
    Remove-NetFirewallRule -DisplayName $FirewallRuleControl -ErrorAction SilentlyContinue
    Remove-NetFirewallRule -DisplayName $FirewallRulePassive -ErrorAction SilentlyContinue
    New-NetFirewallRule -DisplayName $FirewallRuleControl -Direction Inbound -Protocol TCP -LocalPort $Port -Action Allow -Profile Any | Out-Null
    New-NetFirewallRule -DisplayName $FirewallRulePassive -Direction Inbound -Protocol TCP -LocalPort "$PassivePortStart-$PassivePortEnd" -Action Allow -Profile Any | Out-Null
    & netsh advfirewall set global StatefulFtp enable | Out-Null

    # --- 6. FTPサービスの自動起動・再起動 ---
    Write-Step 'FTPサービス(ftpsvc)を起動しています...'
    Set-Service -Name 'ftpsvc' -StartupType Automatic
    Restart-Service -Name 'ftpsvc' -Force

    # --- 完了サマリー ---
    $ipAddresses = @(Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
        Where-Object { $_.IPAddress -notlike '127.*' -and $_.IPAddress -notlike '169.254.*' } |
        Select-Object -ExpandProperty IPAddress)

    Write-Host ''
    Write-Host '=========================================================='
    Write-Host '  FTPサーバーのセットアップが完了しました！' -ForegroundColor Green
    Write-Host '=========================================================='
    Write-Host ''
    Write-Host '  カメラ側には以下を設定してください:'
    Write-Host "    サーバー(IPアドレス): $($ipAddresses -join ' / ')"
    Write-Host "    ポート              : $Port"
    Write-Host "    ユーザー名          : $UserName"
    Write-Host '    パスワード          : (設定したパスワード)'
    Write-Host '    転送モード          : パッシブ(PASV)推奨'
    Write-Host ''
    Write-Host '  PC側:'
    Write-Host "    FTP受信フォルダ     : $FtpRoot"
    Write-Host '    ※ RapidPhotoWatcher の監視フォルダに上記フォルダを設定すると、'
    Write-Host '       カメラから転送された写真が自動でリネーム・整理されます。'
    Write-Host "    ログ                : $logPath"
    Write-Host ''

    Exit-Script 0
}
catch {
    Write-Host ''
    Write-Host "エラーが発生しました: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "詳細ログ: $logPath" -ForegroundColor Yellow
    Exit-Script 1
}
finally {
    try { Stop-Transcript | Out-Null } catch { }
}
