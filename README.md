# RapidPhotoWatcher v2.1

[![CI Build](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/ci.yml/badge.svg)](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/ci.yml)
[![Release](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/ykmp-dev/RapidPhotoWatcher/actions/workflows/build-and-release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

写真ファイルの高速監視・自動整理を行うクロスプラットフォーム対応デスクトップアプリケーションです。

## ✨ v2.1の新機能

### 🚀 **Smart UI Enhancement** - EOS Utility風の革新的インターフェース

**ついに登場！** プロカメラマンが愛用するEOS Utilityのような、直感的で美しいユーザーインターフェースを実現しました。

#### 🎯 **スマート連番システム**
- **📊 リアルタイム表示**: 撮影現場で重要な「現在何番まで進んだか」が一目でわかる
- **🔄 自動同期**: ファイル処理と同時に画面の数字が更新される、ライブ感のあるUI
- **✨ 直感的操作**: 番号を変更すると、そこから新しく撮影が始まる自然な動作

#### 🖼️ **プロフェッショナルなデザイン**
- **美しいヘッダー**: ブランドアイデンティティを強化する洗練されたヘッダーエリア
- **最適化レイアウト**: 800×650pxの黄金比に近い使いやすいウィンドウサイズ
- **視覚的フィードバック**: 重要な情報がハイライトされ、見逃しにくい

#### ⚡ **ワークフロー革命**
- **監視開始時の自動同期**: アプリ起動と同時に正確な状態を表示
- **ゼロラグ更新**: ファイル処理完了と同時にUI更新
- **エラーレス運用**: 「今何番？」の迷いを完全排除

### 🐛 バグ修正（v2.0.2）
- **区切り文字選択の修正**: UIで区切り文字を変更した時に設定が即座に反映されるように修正
- **設定同期の改善**: ViewModelの全ての設定項目でAppSettingsとの即座同期を実装
- **ログ機能強化**: 設定変更時のより詳細なログ出力を追加
- **ウィンドウタイトル更新**: バージョン番号をv2.0.2に更新

### 🐛 バグ修正（v2.0.1）
- **連番システムの重要な修正**: 開始番号を変更した時に現在の連番が正しくリセットされるように修正
- **設定の整合性強化**: 設定読み込み時に開始番号と現在の連番の整合性をチェック
- **UIフィードバック改善**: 開始番号変更時の即座反映とログ出力の強化

## ✨ v2.0.0の主な機能

- **🌐 クロスプラットフォーム対応**: Avalonia UIフレームワークによるWindows/macOS/Linux対応
- **🎨 モダンUI**: 新しいユーザーインターフェースデザイン
- **🔧 強化された設定管理**: より直感的な設定画面
- **📱 レスポンシブデザイン**: 様々な画面サイズに対応
- **🛡️ 信頼性向上**: クロスプラットフォーム対応のファイル監視とフォールバック機構

## 🎯 主要機能

### 📁 **高性能ファイル監視**
- **瞬間検知**: FileSystemWatcherによるミリ秒レベルの高速検知
- **多形式対応**: JPEG（.jpg/.jpeg）、RAW（CR2/CR3/ARW/NEF/RW2/ORF/DNG）
- **フォールバック機能**: ネットワークドライブ等でも確実に動作するポーリングモード

### ✨ **スマート自動処理**
- **🔄 自動リネーム**: 撮影日時＋連番の柔軟なファイル名生成
- **📦 自動移動**: 整理されたフォルダ構造での自動ファイル移動
- **🎯 重複回避**: 同名ファイル検知時の自動連番付加
- **📊 リアルタイム進行表示**: v2.1の新機能！

### 🔗 **プロフェッショナル連携**
- **👁️ プレビュー連携**: Adobe Bridge、FastStone等の外部ビューアー自動起動
- **📝 詳細ログ**: 処理履歴の完全記録とエラートラッキング
- **💾 設定記憶**: プロジェクトごとの設定保存・復元

### 🌐 **クロスプラットフォーム**
- **Windows**: フル機能サポート（Win10/11）
- **macOS**: ネイティブ対応（Intel/Apple Silicon）
- **Linux**: 主要ディストリビューション対応

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

## 🚀 クイックスタート

### 📸 **基本的な使用方法**
1. **📁 フォルダ設定**: 監視フォルダと移動先フォルダを選択
2. **📝 ファイル名設定**: お好みの接頭辞（IMG_、撮影日など）を設定
3. **📋 形式選択**: JPEG、RAW（CR2/CR3/ARW/NEF等）から監視対象を選択
4. **⚡ 監視開始**: 「開始」ボタンで即座に監視スタート

### 🎯 **v2.1の新機能活用法**
- **現在番号確認**: メイン画面で常に「次のファイルが何番になるか」を確認
- **途中から再開**: 撮影を中断して再開する場合も、番号を見ながら適切に設定
- **番号調整**: 撮影済みファイルがある場合は、現在番号を調整して重複回避

### 💡 **プロ向けのワークフロー**
1. **事前設定**: 撮影前にフォルダとファイル名規則を決定
2. **監視開始**: 撮影開始と同時に監視をスタート
3. **リアルタイム確認**: 画面で進行状況をモニタリング
4. **自動処理**: ファイル追加と同時に自動リネーム・移動実行

## ダウンロード

最新版は [Releases](https://github.com/ykmp-dev/RapidPhotoWatcher/releases) からダウンロードできます。

### インストール方法

#### Windows - インストーラ版（推奨）
1. [最新リリース](https://github.com/ykmp-dev/RapidPhotoWatcher/releases/latest) から `RapidPhotoWatcher_v2.1.0_Setup.exe` をダウンロード
2. インストーラを実行してウィザードに従う
3. インストール完了後、デスクトップアイコンまたはスタートメニューから起動

#### クロスプラットフォーム - 手動インストール
1. [最新リリース](https://github.com/ykmp-dev/RapidPhotoWatcher/releases/latest) から対応するプラットフォーム版をダウンロード
   - **Windows x64**: `RapidPhotoWatcher-v2.1-win-x64.zip`
   - **macOS Intel**: `RapidPhotoWatcher-v2.1-osx-x64.zip`
   - **macOS Apple Silicon**: `RapidPhotoWatcher-v2.1-osx-arm64.zip`
   - **Linux x64**: `RapidPhotoWatcher-v2.1-linux-x64.zip`
   - **Linux ARM64**: `RapidPhotoWatcher-v2.1-linux-arm64.zip`
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

### v2.1 (2024-10-05) - 🚀 Smart UI Enhancement
**「プロカメラマンが求めた理想のUI」** - EOS Utilityにインスパイアされた革命的アップデート

#### 🎯 **UI/UX革命**
- **📊 リアルタイム連番表示**: 「現在何番？」の疑問を完全解決
  - 開始番号フィールドが現在の進行状況をライブ表示
  - ファイル処理と同時に画面更新される快適なレスポンス
  - 撮影現場での迷いを排除する直感的インターフェース

#### 🖼️ **プロフェッショナルデザイン**
- **美しいヘッダーエリア**: ブランドアイデンティティを強化
- **最適化レイアウト**: 800×650px黄金比ウィンドウで使いやすさ向上
- **視覚的フィードバック**: 重要情報のハイライト表示

#### ⚡ **ワークフロー最適化**
- **ゼロラグ同期**: 監視開始時の即座状態反映
- **自動UI更新**: ファイル処理完了と同時のリアルタイム更新
- **エラーレス運用**: 手動確認不要の自動化されたワークフロー

**🎥 撮影現場での生産性が劇的に向上します！**

### v2.0.2 (2024-10-02) - UI Settings Bug Fix
- **🐛 区切り文字選択の修正**: UIで区切り文字を変更した時に設定が即座に反映されない問題を修正
- **🔄 設定同期の改善**: ViewModelの全ての設定項目でAppSettingsとの即座同期を実装
- **📝 ログ機能強化**: 設定変更時のより詳細で分かりやすいログ出力を追加
- **🎯 ウィンドウタイトル更新**: バージョン番号をタイトルバーに表示するように更新（v2.0.2）

### v2.0.1 (2024-10-02) - Sequence Number Bug Fix
- **🐛 重要なバグ修正**: 連番開始番号変更時の動作を修正
  - 開始番号を変更すると現在の連番が自動的に新しい開始番号にリセットされるように修正
  - 例：0002まで進んだ後に開始番号を11に変更 → 次のファイルが正しく0011になる
- **🔧 設定システムの強化**: 設定読み込み時の整合性チェック機能追加
- **📊 バリデーション改善**: 連番に関するバリデーション機能の強化
- **🎯 UIフィードバック**: 開始番号変更時の即座反映とログ出力の改善

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