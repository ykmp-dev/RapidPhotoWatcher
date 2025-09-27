# JPEG/RAW フォルダ監視アプリケーション

[![CI Build](https://github.com/yubertokyo/jpeg-raw-folder-monitor/actions/workflows/ci.yml/badge.svg)](https://github.com/yubertokyo/jpeg-raw-folder-monitor/actions/workflows/ci.yml)
[![Release](https://github.com/yubertokyo/jpeg-raw-folder-monitor/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/yubertokyo/jpeg-raw-folder-monitor/actions/workflows/build-and-release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

EOS Utility2に似たJPEG/RAWファイルの自動監視・リネーム・移動アプリケーションです。

## 機能

- **ファイル監視**: 指定フォルダ内のJPEG/RAWファイルを自動監視
- **自動リネーム**: 連番付きファイル名で自動リネーム
- **自動移動**: 指定した移動先フォルダへの自動移動
- **プレビュー連携**: 外部プレビューアプリケーションとの連携
- **ログ機能**: 処理結果とエラーの詳細ログ
- **設定保存**: アプリケーション設定の永続化

## システム要件

- Windows 10/11 Pro
- .NET 6.0 Runtime

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

最新版は [Releases](https://github.com/yubertokyo/jpeg-raw-folder-monitor/releases) からダウンロードできます。

### インストール方法

1. [最新リリース](https://github.com/yubertokyo/jpeg-raw-folder-monitor/releases/latest) から `JPEGFolderMonitor-win-x64.zip` をダウンロード
2. 任意のフォルダに展開
3. `JPEGFolderMonitor.exe` を実行

## 開発者向け

### ビルド方法

```bash
# 開発用ビルド
dotnet build

# リリース用ビルド（EXE生成）
build.bat
# または
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 実行方法

```bash
dotnet run
```

## 設定ファイル

設定は以下の場所に自動保存されます：
```
%APPDATA%\JPEGFolderMonitor\settings.json
```

## ログファイル

ログは以下の場所に記録されます：
```
%APPDATA%\JPEGFolderMonitor\Logs\application.log
```

## 注意事項

- ファイルが他のアプリケーションで使用中の場合、処理が遅延する場合があります
- 移動先フォルダが存在しない場合、自動作成されます
- 同名ファイルが存在する場合、連番を付加して重複を回避します

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。