@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo   RapidPhotoWatcher v1.1.0 インストーラー
echo ==========================================
echo.

REM 管理者権限チェック
net session >nul 2>&1
if %errorLevel% == 0 (
    echo 管理者権限で実行されています。
) else (
    echo 注意: このスクリプトは管理者権限で実行することを推奨します。
    echo 続行しますか？ [Y/N]
    set /p choice=
    if /i not "!choice!"=="Y" exit /b 1
)

REM インストール先ディレクトリを設定
set "INSTALL_DIR=%ProgramFiles%\RapidPhotoWatcher"
echo.
echo インストール先: %INSTALL_DIR%

REM ディレクトリ作成
if not exist "%INSTALL_DIR%" (
    echo ディレクトリを作成しています...
    mkdir "%INSTALL_DIR%"
    if !errorlevel! neq 0 (
        echo エラー: ディレクトリの作成に失敗しました。
        pause
        exit /b 1
    )
)

REM ファイルコピー
echo ファイルをコピーしています...
xcopy /E /Y ".\publish\*" "%INSTALL_DIR%\"
if !errorlevel! neq 0 (
    echo エラー: ファイルのコピーに失敗しました。
    pause
    exit /b 1
)

REM スタートメニューショートカット作成
echo スタートメニューにショートカットを作成しています...
set "START_MENU=%ProgramData%\Microsoft\Windows\Start Menu\Programs"
powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%START_MENU%\RapidPhotoWatcher.lnk'); $Shortcut.TargetPath = '%INSTALL_DIR%\RapidPhotoWatcher.exe'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.IconLocation = '%INSTALL_DIR%\app_icon.ico'; $Shortcut.Save()"

REM デスクトップショートカット作成（オプション）
echo.
echo デスクトップにショートカットを作成しますか？ [Y/N]
set /p desktop_choice=
if /i "!desktop_choice!"=="Y" (
    echo デスクトップにショートカットを作成しています...
    powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\Desktop\RapidPhotoWatcher.lnk'); $Shortcut.TargetPath = '%INSTALL_DIR%\RapidPhotoWatcher.exe'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.IconLocation = '%INSTALL_DIR%\app_icon.ico'; $Shortcut.Save()"
)

echo.
echo ==========================================
echo   インストールが完了しました！
echo ==========================================
echo.
echo RapidPhotoWatcher を起動しますか？ [Y/N]
set /p launch_choice=
if /i "!launch_choice!"=="Y" (
    start "" "%INSTALL_DIR%\RapidPhotoWatcher.exe"
)

echo.
echo インストール完了。
pause