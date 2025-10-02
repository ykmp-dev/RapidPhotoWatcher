@echo off
REM RapidPhotoWatcher Cross-Platform Build Script for Windows

set PROJECT_DIR=RapidPhotoWatcher.AvaloniaUI
set OUTPUT_DIR=dist
set VERSION=2.0.2

echo 🚀 Building RapidPhotoWatcher v%VERSION% for multiple platforms...

REM Create output directory
if not exist %OUTPUT_DIR% mkdir %OUTPUT_DIR%

REM Build for Windows x64
echo 📦 Building for Windows x64...
dotnet publish %PROJECT_DIR% -c Release -r win-x64 --self-contained -o %OUTPUT_DIR%\win-x64
cd %OUTPUT_DIR%
powershell Compress-Archive -Path win-x64 -DestinationPath RapidPhotoWatcher-v%VERSION%-win-x64.zip -Force
cd ..

REM Build for macOS x64
echo 📦 Building for macOS x64...
dotnet publish %PROJECT_DIR% -c Release -r osx-x64 --self-contained -o %OUTPUT_DIR%\osx-x64
cd %OUTPUT_DIR%
powershell Compress-Archive -Path osx-x64 -DestinationPath RapidPhotoWatcher-v%VERSION%-osx-x64.zip -Force
cd ..

REM Build for macOS ARM64 (Apple Silicon)
echo 📦 Building for macOS ARM64 (Apple Silicon)...
dotnet publish %PROJECT_DIR% -c Release -r osx-arm64 --self-contained -o %OUTPUT_DIR%\osx-arm64
cd %OUTPUT_DIR%
powershell Compress-Archive -Path osx-arm64 -DestinationPath RapidPhotoWatcher-v%VERSION%-osx-arm64.zip -Force
cd ..

REM Build for Linux x64
echo 📦 Building for Linux x64...
dotnet publish %PROJECT_DIR% -c Release -r linux-x64 --self-contained -o %OUTPUT_DIR%\linux-x64
cd %OUTPUT_DIR%
powershell Compress-Archive -Path linux-x64 -DestinationPath RapidPhotoWatcher-v%VERSION%-linux-x64.zip -Force
cd ..

REM Build for Linux ARM64
echo 📦 Building for Linux ARM64...
dotnet publish %PROJECT_DIR% -c Release -r linux-arm64 --self-contained -o %OUTPUT_DIR%\linux-arm64
cd %OUTPUT_DIR%
powershell Compress-Archive -Path linux-arm64 -DestinationPath RapidPhotoWatcher-v%VERSION%-linux-arm64.zip -Force
cd ..

echo ✅ Build completed! Packages available in %OUTPUT_DIR%\
echo 📁 Available packages:
dir %OUTPUT_DIR%\*.zip

echo.
echo 📋 Installation instructions:
echo Windows: Extract zip and run RapidPhotoWatcher.AvaloniaUI.exe
echo macOS: Extract zip and run ./RapidPhotoWatcher.AvaloniaUI
echo Linux: Extract zip and run ./RapidPhotoWatcher.AvaloniaUI

pause