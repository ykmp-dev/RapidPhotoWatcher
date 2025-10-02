#!/bin/bash

# RapidPhotoWatcher Cross-Platform Build Script

set -e

PROJECT_DIR="RapidPhotoWatcher.AvaloniaUI"
OUTPUT_DIR="dist"
VERSION="2.0.2"

echo "🚀 Building RapidPhotoWatcher v${VERSION} for multiple platforms..."

# Create output directory
mkdir -p ${OUTPUT_DIR}

# Build for Windows x64
echo "📦 Building for Windows x64..."
dotnet publish ${PROJECT_DIR} -c Release -r win-x64 --self-contained -o ${OUTPUT_DIR}/win-x64
cd ${OUTPUT_DIR}
zip -r RapidPhotoWatcher-v${VERSION}-win-x64.zip win-x64/
cd ..

# Build for macOS x64
echo "📦 Building for macOS x64..."
dotnet publish ${PROJECT_DIR} -c Release -r osx-x64 --self-contained -o ${OUTPUT_DIR}/osx-x64
cd ${OUTPUT_DIR}
tar -czf RapidPhotoWatcher-v${VERSION}-osx-x64.tar.gz osx-x64/
cd ..

# Build for macOS ARM64 (Apple Silicon)
echo "📦 Building for macOS ARM64 (Apple Silicon)..."
dotnet publish ${PROJECT_DIR} -c Release -r osx-arm64 --self-contained -o ${OUTPUT_DIR}/osx-arm64
cd ${OUTPUT_DIR}
tar -czf RapidPhotoWatcher-v${VERSION}-osx-arm64.tar.gz osx-arm64/
cd ..

# Build for Linux x64
echo "📦 Building for Linux x64..."
dotnet publish ${PROJECT_DIR} -c Release -r linux-x64 --self-contained -o ${OUTPUT_DIR}/linux-x64
cd ${OUTPUT_DIR}
tar -czf RapidPhotoWatcher-v${VERSION}-linux-x64.tar.gz linux-x64/
cd ..

# Build for Linux ARM64
echo "📦 Building for Linux ARM64..."
dotnet publish ${PROJECT_DIR} -c Release -r linux-arm64 --self-contained -o ${OUTPUT_DIR}/linux-arm64
cd ${OUTPUT_DIR}
tar -czf RapidPhotoWatcher-v${VERSION}-linux-arm64.tar.gz linux-arm64/
cd ..

echo "✅ Build completed! Packages available in ${OUTPUT_DIR}/"
echo "📁 Available packages:"
ls -la ${OUTPUT_DIR}/*.zip ${OUTPUT_DIR}/*.tar.gz

echo ""
echo "📋 Installation instructions:"
echo "Windows: Extract zip and run RapidPhotoWatcher.AvaloniaUI.exe"
echo "macOS: Extract tar.gz and run ./RapidPhotoWatcher.AvaloniaUI"
echo "Linux: Extract tar.gz and run ./RapidPhotoWatcher.AvaloniaUI"