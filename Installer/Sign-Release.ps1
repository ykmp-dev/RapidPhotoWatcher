<#
.SYNOPSIS
    RapidPhotoWatcher リリース成果物のコード署名スクリプト (signtool使用)

.DESCRIPTION
    exe・インストーラー・PowerShellスクリプトに Authenticode 署名を付与します。
    署名には以下のいずれかが必要です:
      A) 認証局(CA)発行のコード署名証明書のPFXファイル      → -PfxPath / -PfxPassword
      B) 証明書ストアにインストール済みの証明書              → -Thumbprint
    ※ 自己署名証明書でも動作しますが、SmartScreen警告は消えません。
       正式配布にはCA発行(OV/EV)証明書、または Azure Trusted Signing を推奨します。

    signtool.exe は Windows SDK に含まれます。
    https://learn.microsoft.com/ja-jp/windows/win32/seccrypto/cryptography-tools

.EXAMPLE
    # PFXファイルで署名
    .\Sign-Release.ps1 -PfxPath .\codesign.pfx -PfxPassword (Read-Host -AsSecureString)

    # 証明書ストアの証明書(拇印指定)で署名
    .\Sign-Release.ps1 -Thumbprint 1234ABCD...

    # 署名対象を明示指定
    .\Sign-Release.ps1 -PfxPath .\codesign.pfx -Files @(".\publish\RapidPhotoWatcher.AvaloniaUI.exe")
#>
[CmdletBinding(DefaultParameterSetName = 'Pfx')]
param(
    # コード署名証明書のPFXファイルパス
    [Parameter(ParameterSetName = 'Pfx', Mandatory = $true)]
    [string]$PfxPath,

    # PFXのパスワード
    [Parameter(ParameterSetName = 'Pfx')]
    [SecureString]$PfxPassword,

    # 証明書ストア内の証明書の拇印(サムプリント)
    [Parameter(ParameterSetName = 'Store', Mandatory = $true)]
    [string]$Thumbprint,

    # 署名対象ファイル(省略時はリポジトリ内の既定成果物を自動探索)
    [string[]]$Files,

    # RFC3161 タイムスタンプサーバー
    [string]$TimestampUrl = 'http://timestamp.digicert.com'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent

# --- signtool.exe を探す ---
function Find-SignTool {
    $cmd = Get-Command signtool.exe -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }

    $kitsRoot = 'C:\Program Files (x86)\Windows Kits\10\bin'
    if (Test-Path $kitsRoot) {
        $candidates = Get-ChildItem -Path $kitsRoot -Recurse -Filter signtool.exe -ErrorAction SilentlyContinue |
            Where-Object { $_.FullName -like '*\x64\*' } |
            Sort-Object FullName -Descending
        if ($candidates) { return $candidates[0].FullName }
    }

    throw 'signtool.exe が見つかりません。Windows SDK をインストールしてください。' +
          ' (https://developer.microsoft.com/windows/downloads/windows-sdk/)'
}

$signtool = Find-SignTool
Write-Host "signtool: $signtool"

# --- 署名対象の既定リスト ---
if (-not $Files) {
    $defaults = @(
        (Join-Path $repoRoot 'RapidPhotoWatcher.AvaloniaUI\bin\Release\net8.0\win-x64\publish\RapidPhotoWatcher.AvaloniaUI.exe'),
        (Join-Path $PSScriptRoot 'Setup-FtpServer.ps1'),
        (Join-Path $PSScriptRoot 'Remove-FtpServer.ps1')
    )
    # ビルド済みインストーラー(Output\)も対象に含める
    $defaults += @(Get-ChildItem -Path (Join-Path $repoRoot 'Output\RapidPhotoWatcher_v*_Setup.exe') -ErrorAction SilentlyContinue |
        Select-Object -ExpandProperty FullName)
    $Files = $defaults | Where-Object { $_ -and (Test-Path $_) }
    if (-not $Files) {
        throw '署名対象が見つかりません。先にビルド(dotnet publish / iscc)を実行するか、-Files で指定してください。'
    }
}

# --- 証明書指定の引数を組み立て ---
$certArgs = @()
if ($PSCmdlet.ParameterSetName -eq 'Pfx') {
    if (-not (Test-Path $PfxPath)) { throw "PFXファイルが見つかりません: $PfxPath" }
    $certArgs += @('/f', $PfxPath)
    if ($PfxPassword) {
        $plain = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
            [Runtime.InteropServices.Marshal]::SecureStringToBSTR($PfxPassword))
        $certArgs += @('/p', $plain)
    }
} else {
    $certArgs += @('/sha1', $Thumbprint)
}

# --- 署名 + 検証 ---
$failed = 0
foreach ($file in $Files) {
    Write-Host ''
    Write-Host "署名中: $file"
    & $signtool sign /fd SHA256 /td SHA256 /tr $TimestampUrl @certArgs "$file"
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  → 署名失敗" -ForegroundColor Red
        $failed++
        continue
    }

    & $signtool verify /pa "$file"
    if ($LASTEXITCODE -ne 0) {
        # 自己署名証明書の場合、検証は失敗するが署名自体は付与されている
        Write-Host '  → 署名は付与されましたが、信頼チェーンの検証に失敗しました(自己署名証明書の場合は正常)。' -ForegroundColor Yellow
    } else {
        Write-Host '  → 署名・検証OK' -ForegroundColor Green
    }
}

Write-Host ''
if ($failed -gt 0) {
    Write-Host "$failed 件のファイルで署名に失敗しました。" -ForegroundColor Red
    exit 1
}
Write-Host 'すべてのファイルの署名が完了しました。' -ForegroundColor Green
