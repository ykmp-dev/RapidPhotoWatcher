# 🌟 RapidPhotoWatcher v2.0.0 - Avalonia UI Cross-Platform Edition

## ✨ 主な新機能

### 🌐 クロスプラットフォーム対応
- **Windows**: Windows 10/11 (x64) 対応
- **macOS**: macOS 10.15 Catalina以降対応  
- **Linux**: Ubuntu、Debian、Fedora、CentOS/RHEL等対応

### 🎨 モダンUI
- Avalonia UIフレームワークによる新しいユーザーインターフェース
- レスポンシブデザインで様々な画面サイズに対応
- 高DPI環境での最適化

### 🏗️ アーキテクチャ刷新
- 共有ビジネスロジック（Shared/）の分離
- より保守性の高いコード構造
- 旧版（Windows Forms）もWindowsForms-Original/で保持

## 📦 ダウンロード

### Windows（推奨）
- **RapidPhotoWatcher_v2.0.0_Setup.exe**: Windowsインストーラー版（自己完結型）

### クロスプラットフォーム版（手動インストール）
- **RapidPhotoWatcher-v2.0.0-win-x64.zip**: Windows x64版
- **RapidPhotoWatcher-v2.0.0-osx-x64.zip**: macOS Intel版
- **RapidPhotoWatcher-v2.0.0-osx-arm64.zip**: macOS Apple Silicon版
- **RapidPhotoWatcher-v2.0.0-linux-x64.zip**: Linux x64版
- **RapidPhotoWatcher-v2.0.0-linux-arm64.zip**: Linux ARM64版

## 🔧 システム要件

### Windows
- Windows 10/11 (x64)
- .NET Runtime不要（自己完結型配布）

### macOS  
- macOS 10.15 Catalina以降
- .NET 8.0 Runtime

### Linux
- Ubuntu 18.04 LTS以降、Debian、Fedora、CentOS/RHEL等
- .NET 8.0 Runtime

## 📝 詳細な変更点

- **🌟 メジャーアップデート**: Avalonia UIフレームワークへの完全移行
- **🌐 クロスプラットフォーム対応**: Windows/macOS/Linux対応（x64/ARM64アーキテクチャ）
- **🎨 新UI**: モダンでレスポンシブなユーザーインターフェース
- **🏗️ アーキテクチャ刷新**: 共有ビジネスロジック（Shared/）の分離
- **📦 自己完結型配布**: .NET Runtimeが不要なパッケージ
- **🔧 設定管理の改善**: より直感的な設定画面
- **📱 高DPI対応**: 様々な解像度に対応
- **🛡️ 信頼性向上**: FileSystemWatcherのクロスプラットフォーム対応とフォールバック機構
- **⚡ パフォーマンス向上**: エラーハンドリングとポーリングフォールバックの強化
- **🔄 後方互換性**: 旧版（Windows Forms）もWindowsForms-Original/で保持

## 🚀 使用方法

### Windows（インストーラー版）
1. `RapidPhotoWatcher_v2.0.0_Setup.exe`をダウンロード
2. インストーラーを実行
3. インストール完了後、デスクトップアイコンまたはスタートメニューから起動

### 手動インストール
1. 対応するプラットフォーム版をダウンロード
2. 任意のフォルダに展開
3. 実行ファイルを起動
   - Windows: `RapidPhotoWatcher.AvaloniaUI.exe`
   - macOS/Linux: `./RapidPhotoWatcher.AvaloniaUI`

## 📋 注意事項

- 設定ファイルは従来と同じ場所（`%APPDATA%\RapidPhotoWatcher\settings.json`）に保存されます
- Windows Forms版からの設定は自動的に引き継がれます
- 初回起動時に若干時間がかかる場合があります

---

**Full Changelog**: https://github.com/ykmp-dev/RapidPhotoWatcher/compare/v1.1.1...v2.0.0