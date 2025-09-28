@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo   RapidPhotoWatcher アンインストーラー
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
echo RapidPhotoWatcher をアンインストールしますか？ [Y/N]
echo インストール先: %INSTALL_DIR%
set /p uninstall_choice=
if /i not "!uninstall_choice!"=="Y" (
    echo アンインストールを中止しました。
    pause
    exit /b 0
)

REM プロセス終了
echo RapidPhotoWatcher のプロセスを終了しています...
taskkill /f /im RapidPhotoWatcher.exe >nul 2>&1

REM ショートカット削除
echo ショートカットを削除しています...
del "%ProgramData%\Microsoft\Windows\Start Menu\Programs\RapidPhotoWatcher.lnk" >nul 2>&1
del "%USERPROFILE%\Desktop\RapidPhotoWatcher.lnk" >nul 2>&1
del "%PUBLIC%\Desktop\RapidPhotoWatcher.lnk" >nul 2>&1

REM ユーザーデータ削除確認
echo.
echo 設定ファイルとログファイルも削除しますか？ [Y/N]
echo （削除しない場合、再インストール時に設定が復元されます）
set /p data_choice=
if /i "!data_choice!"=="Y" (
    echo ユーザーデータを削除しています...
    rmdir /s /q "%APPDATA%\RapidPhotoWatcher" >nul 2>&1
)

REM アプリケーションファイル削除
echo アプリケーションファイルを削除しています...
if exist "%INSTALL_DIR%" (
    rmdir /s /q "%INSTALL_DIR%"
    if !errorlevel! neq 0 (
        echo 警告: 一部のファイルを削除できませんでした。
        echo 手動で削除してください: %INSTALL_DIR%
    )
)

echo.
echo ==========================================
echo   アンインストールが完了しました
echo ==========================================
echo.
pause