<#
.SYNOPSIS
    RapidPhotoWatcher 用 FTPサーバー設定の削除スクリプト

.DESCRIPTION
    Setup-FtpServer.ps1 が構成した以下の設定を削除します。
      1. IIS FTPサイト
      2. Windowsファイアウォール規則
      3. (オプション) カメラ接続用ローカルユーザー

    ※ IISのWindows機能自体、およびFTP受信フォルダ内の写真は削除しません。
    ※ 管理者権限で実行してください。

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File .\Remove-FtpServer.ps1
    powershell -ExecutionPolicy Bypass -File .\Remove-FtpServer.ps1 -RemoveUser -UserName camera
#>
[CmdletBinding()]
param(
    # 削除するIIS FTPサイト名
    [string]$SiteName = 'RapidPhotoWatcher-FTP',

    # -RemoveUser 指定時に削除するFTPユーザー名
    [string]$UserName = 'camera',

    # FTPユーザーも削除する
    [switch]$RemoveUser,

    # 完了時にキー入力待ちをしない(サイレント実行用)
    [switch]$NoPause
)

$ErrorActionPreference = 'Continue'

$FirewallRuleControl = 'RapidPhotoWatcher FTP (Control Port)'
$FirewallRulePassive = 'RapidPhotoWatcher FTP (Passive Ports)'

function Exit-Script([int]$Code) {
    if (-not $NoPause) {
        Write-Host ''
        Read-Host '確認したら Enter キーを押してください'
    }
    exit $Code
}

Write-Host '=========================================================='
Write-Host '  RapidPhotoWatcher - FTPサーバー設定の削除'
Write-Host '=========================================================='

# --- 管理者権限チェック ---
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal($identity)
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host 'エラー: このスクリプトは管理者権限で実行する必要があります。' -ForegroundColor Red
    Exit-Script 1
}

# --- IIS FTPサイトの削除 ---
try {
    Import-Module WebAdministration -ErrorAction Stop
    if (Get-Website -Name $SiteName -ErrorAction SilentlyContinue) {
        Write-Host "FTPサイトを削除しています: $SiteName"
        Remove-Website -Name $SiteName
    } else {
        Write-Host "FTPサイトは見つかりませんでした: $SiteName"
    }
    Clear-WebConfiguration -Filter '/system.ftpServer/security/authorization' -PSPath 'IIS:\' -Location $SiteName -ErrorAction SilentlyContinue
}
catch {
    Write-Host "IISの設定削除をスキップしました: $($_.Exception.Message)" -ForegroundColor Yellow
}

# --- ファイアウォール規則の削除 ---
Write-Host 'ファイアウォール規則を削除しています...'
Remove-NetFirewallRule -DisplayName $FirewallRuleControl -ErrorAction SilentlyContinue
Remove-NetFirewallRule -DisplayName $FirewallRulePassive -ErrorAction SilentlyContinue

# --- FTPユーザーの削除(オプション) ---
if ($RemoveUser) {
    if (Get-LocalUser -Name $UserName -ErrorAction SilentlyContinue) {
        Write-Host "FTPユーザーを削除しています: $UserName"
        Remove-LocalUser -Name $UserName
    }
    $userListKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList'
    if (Test-Path $userListKey) {
        Remove-ItemProperty -Path $userListKey -Name $UserName -ErrorAction SilentlyContinue
    }
}

Write-Host ''
Write-Host 'FTPサーバー設定の削除が完了しました。' -ForegroundColor Green
Write-Host '※ IISのWindows機能とFTP受信フォルダ内の写真はそのまま残っています。'

Exit-Script 0
