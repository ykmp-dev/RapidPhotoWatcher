# RapidPhotoWatcher v2.0.0

[![CI Build](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/ci.yml/badge.svg)](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/ci.yml)
[![Release](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/build-and-release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

写真ファイルの高速監視・自動整理を行うクロスプラットフォーム対応デスクトップアプリケーションです。

## ✨ v2.0.0の新機能

- **🌐 クロスプラットフォーム対応**: Avalonia UIフレームワークによるWindows/macOS/Linux対応
- **🎨 モダンUI**: 新しいユーザーインターフェースデザイン
- **🔧 強化された設定管理**: より直感的な設定画面
- **📱 レスポンシブデザイン**: 様々な画面サイズに対応
- **🛡️ 信頼性向上**: クロスプラットフォーム対応のファイル監視とフォールバック機構

## 機能

- **ファイル監視**: 指定フォルダ内のJPEG/RAWファイルを自動監視
- **自動リネーム**: 連番付きファイル名で自動リネーム
- **自動移動**: 指定した移動先フォルダへの自動移動
- **プレビュー連携**: 外部プレビューアプリケーションとの連携
- **ログ機能**: 処理結果とエラーの詳細ログ
- **設定保存**: アプリケーション設定の永続化

## システム要件

### Windows
- Windows 10/11 (x64)
- .NET 8.0 Runtime（自己完結型配布により不要）

### macOS
- macOS 10.15 Catalina以降
- .NET 8.0 Runtime

### Linux
- Ubuntu 18.04 LTS以降、Debian、Fedora、CentOS/RHEL等
- .NET 8.0 Runtime

## サポート対象ファイル形式

### JPEG
- .jpg
- .jpeg

### RAW
- .cr2 (Canon RAW Version 2)
- .cr3 (Canon RAW Version 3)
- .arw (Sony Alpha Raw)
- .nef (Nikon Electronic Format)
- .rw2 (Panasonic RAW Version 2)
- .orf (Olympus RAW Format)
- .dng (Digital Negative)

## 監視モード

### 即座監視
FileSystemWatcherを使用したリアルタイム監視
- v1.1.1より監視開始時の既存ファイル自動処理機能を追加

### ポーリング監視
指定間隔でのフォルダチェック（1-3600秒）

## 使用方法

1. 監視フォルダを選択
2. 移動先フォルダを選択
3. ファイル名接頭辞を設定（オプション）
4. 監視するファイル形式を選択
5. 監視モードを選択
6. 「開始」ボタンクリックで監視開始

## ダウンロード

最新版は [Releases](https://github.com/ykmp-dev/RapidPhotoWatcher/releases) からダウンロードできます。

### インストール方法

#### Windows - インストーラ版（推奨）
1. [最新リリース](https://github.com/ykmp-dev/RapidPhotoWatcher/releases/latest) から `RapidPhotoWatcher_v2.0.0_Setup.exe` をダウンロード
2. インストーラを実行してウィザードに従う
3. インストール完了後、デスクトップアイコンまたはスタートメニューから起動

#### クロスプラットフォーム - 手動インストール
1. [最新リリース](https://github.com/ykmp-dev/RapidPhotoWatcher/releases/latest) から対応するプラットフォーム版をダウンロード
   - **Windows x64**: `RapidPhotoWatcher-v2.0.0-win-x64.zip`
   - **macOS Intel**: `RapidPhotoWatcher-v2.0.0-osx-x64.zip`
   - **macOS Apple Silicon**: `RapidPhotoWatcher-v2.0.0-osx-arm64.zip`
   - **Linux x64**: `RapidPhotoWatcher-v2.0.0-linux-x64.zip`
   - **Linux ARM64**: `RapidPhotoWatcher-v2.0.0-linux-arm64.zip`
2. アーカイブを任意のフォルダに展開
3. 実行ファイルを起動
   - **Windows**: `RapidPhotoWatcher.AvaloniaUI.exe`をダブルクリック
   - **macOS**: ターミナルで`./RapidPhotoWatcher.AvaloniaUI`を実行
   - **Linux**: ターミナルで`./RapidPhotoWatcher.AvaloniaUI`を実行（実行権限付与が必要な場合があります）

#### 注意事項（クロスプラットフォーム版）
- **macOS**: 初回起動時にセキュリティ警告が表示される場合があります。「システム環境設定」→「セキュリティとプライバシー」で許可してください
- **Linux**: 実行権限を付与する必要がある場合があります：`chmod +x RapidPhotoWatcher.AvaloniaUI`
- **ファイル監視**: 各プラットフォームのネイティブAPIを使用しますが、問題が発生した場合は自動的にポーリングモードにフォールバックします

## 開発者向け

### プロジェクト構造

```
RapidPhotoWatcher/
├── RapidPhotoWatcher.AvaloniaUI/    # メインのAvalonia UIアプリケーション
├── Shared/                          # 共有ビジネスロジック
├── WindowsForms-Original/           # 旧版（Windows Forms）
├── Installer/                       # インストーラーファイル
└── setup.iss                       # Inno Setupスクリプト
```

### ビルド方法

```bash
# 開発用ビルド
cd RapidPhotoWatcher.AvaloniaUI
dotnet build

# リリース用ビルド（Windows x64、自己完結型）
dotnet publish -c Release -r win-x64 --self-contained

# クロスプラットフォーム向けビルド
dotnet publish -c Release -r osx-x64 --self-contained     # macOS Intel
dotnet publish -c Release -r osx-arm64 --self-contained   # macOS Apple Silicon
dotnet publish -c Release -r linux-x64 --self-contained   # Linux x64
dotnet publish -c Release -r linux-arm64 --self-contained # Linux ARM64

# すべてのプラットフォーム向けに一括ビルド
# Windows
build-cross-platform.bat
# macOS/Linux  
./build-cross-platform.sh
```

### 開発環境での実行

```bash
cd RapidPhotoWatcher.AvaloniaUI
dotnet run
```

### インストーラー作成

```bash
# Inno Setup 6が必要
"C:\Program Files (x86)\Inno Setup 6\iscc.exe" setup.iss
```

## 設定ファイル

設定は以下の場所に自動保存されます：
```
%APPDATA%\RapidPhotoWatcher\settings.json
```

## ログファイル

ログは以下の場所に記録されます：
```
%APPDATA%\RapidPhotoWatcher\Logs\application.log
```

## 注意事項

- ファイルが他のアプリケーションで使用中の場合、処理が遅延する場合があります
- 移動先フォルダが存在しない場合、自動作成されます
- 同名ファイルが存在する場合、連番を付加して重複を回避します

## 更新履歴

### v2.0.0 (2024-10-02) - Avalonia UI Cross-Platform Edition
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

### v1.1.1 (2024-09-30)
- **新機能**: 即座監視モードで監視開始時の既存ファイル自動処理機能を追加
- **改善**: 即座監視・ポーリング監視の両モードで一貫した動作を実現
- **修正**: 即座監視モードで監視フォルダ内の既存ファイルが処理されない問題を解決

### v1.1.0 (2024-09-29)
- プロジェクト構造の完全な再構築
- 外部ソフトウェア連携機能の強化
- 高DPI対応の追加
- Inno Setupインストーラーの実装

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。