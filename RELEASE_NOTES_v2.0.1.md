# 🐛 RapidPhotoWatcher v2.0.1 - Critical Bug Fix Release

## 🚨 重要なバグ修正

### 連番システムの修正
**問題**: 開始番号を変更しても、次のファイルの連番が前回の続きから採番されていた

**修正内容**: 
- 開始番号を変更すると現在の連番が自動的に新しい開始番号にリセットされるように修正
- 設定読み込み時の整合性チェック機能を追加

**具体例**:
```
修正前: 0001, 0002 → 開始番号を11に変更 → 次: 0003 ❌
修正後: 0001, 0002 → 開始番号を11に変更 → 次: 0011 ✅
```

## 📦 ダウンロード

### Windows（推奨）
- **RapidPhotoWatcher_v2.0.1_Setup.exe**: Windowsインストーラー版（自己完結型）

### クロスプラットフォーム版（手動インストール）
- **RapidPhotoWatcher-v2.0.1-win-x64.zip**: Windows x64版
- **RapidPhotoWatcher-v2.0.1-osx-x64.zip**: macOS Intel版
- **RapidPhotoWatcher-v2.0.1-osx-arm64.zip**: macOS Apple Silicon版
- **RapidPhotoWatcher-v2.0.1-linux-x64.zip**: Linux x64版
- **RapidPhotoWatcher-v2.0.1-linux-arm64.zip**: Linux ARM64版

## 🔧 詳細な修正内容

### 1. 連番システムの自動リセット
- `SequenceStartNumber`プロパティにセッターロジックを追加
- 開始番号変更時に`CurrentSequenceNumber`を自動的に新しい開始番号に設定
- UIからの変更を即座にAppSettingsに反映

### 2. 設定読み込み時の整合性チェック
- `CurrentSequenceNumber < SequenceStartNumber`の場合は自動調整
- `Math.Max()`を使用して最小値を保証
- 不正な状態の設定ファイルからの復旧機能

### 3. バリデーション機能の強化
- 開始番号の範囲チェック（0以上）
- 現在の連番と開始番号の整合性チェック
- UI側でのリアルタイムバリデーション

### 4. ユーザーエクスペリエンスの改善
- 開始番号変更時の詳細なログ出力
- UIフィードバックの改善
- エラーメッセージの明確化

## 🚀 アップグレード方法

### 既存ユーザー
1. v2.0.1インストーラーを実行（設定は自動的に引き継がれます）
2. または、手動インストール版をダウンロードして既存版を置き換え

### 新規ユーザー
1. Windowsユーザー: `RapidPhotoWatcher_v2.0.1_Setup.exe`をダウンロード・実行
2. その他のプラットフォーム: 対応するzipファイルをダウンロード・展開

## 📋 互換性

- **設定ファイル**: v2.0.0からの完全な互換性
- **ログファイル**: 既存のログは保持されます
- **プラットフォーム**: Windows/macOS/Linux対応

## 🛠️ 技術的詳細

### 修正されたファイル
- `Shared/AppSettings.cs`: プロパティセッターとバリデーション強化
- `RapidPhotoWatcher.AvaloniaUI/ViewModels/FileNamingViewModel.cs`: UI連携改善
- 各種設定ファイル: バージョン番号更新

### テスト済み環境
- Windows 10/11 (x64)
- Visual Studio 2022
- .NET 8.0

## ⚠️ 注意事項

- v2.0.0からv2.0.1へのアップグレードは安全です
- 設定は自動的に移行されます
- 既存の連番は現在の設定に基づいて調整されます

---

**Full Changelog**: https://github.com/ykmp-dev/RapidPhotoWatcher/compare/v2.0.0...v2.0.1