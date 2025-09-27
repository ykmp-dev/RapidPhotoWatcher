@echo off
echo Building JPEG/RAW Folder Monitor...

REM Clean previous builds
if exist "bin\Release" (
    echo Cleaning previous builds...
    rmdir /s /q "bin\Release"
)

REM Restore dependencies
echo Restoring dependencies...
dotnet restore

REM Build and publish for Windows x64
echo Building for Windows x64...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -o "bin\Release\win-x64"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build completed successfully!
    echo Executable location: bin\Release\win-x64\JPEGFolderMonitor.exe
    echo.
    pause
) else (
    echo.
    echo Build failed!
    echo.
    pause
    exit /b 1
)