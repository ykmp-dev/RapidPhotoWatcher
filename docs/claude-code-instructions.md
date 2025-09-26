# JPEG/RAW Folder Monitor - Claude Code 開発指示書

## プロジェクト概要
EOSUtility2風のJPEG/RAWファイル監視・自動リネーム・移動を行うWindowsデスクトップアプリケーション

## 確定仕様
- **対象ファイル**: JPEG (.jpg, .jpeg) および RAW (.cr2, .cr3, .arw, .nef, .rw2, .orf, .dng)
- **プラットフォーム**: Windows 10/11 Pro
- **フレームワーク**: .NET 6.0 + Windows Forms
- **監視モード**: FileSystemWatcher（即時）またはポーリング（秒間隔）

## 自動実行タスクリスト

### 1. リポジトリセットアップ
```
✅ 新規GitHubリポジトリ作成
✅ 基本ファイル構造作成 (.gitignore, README.md, LICENSE)
✅ Issue/PR テンプレート追加
✅ 開発ブランチ (develop) 作成
```

### 2. プロジェクト基盤構築
```
✅ .NET 6.0 WinForms プロジェクト作成
✅ 必要NuGetパッケージ追加:
   - System.Configuration.ConfigurationManager
   - System.Drawing.Common (必要に応じて)
✅ app.config テンプレート作成
✅ フォルダ構造整備 (/src, /docs, /tests)
```

### 3. UI実装
```
✅ メインフォーム作成 (MainForm.cs)
   - 監視モード選択 (ラジオボタン)
   - ポーリング間隔設定 (数値入力)
   - ファイル種別選択 (チェックボックス: JPEG, RAW)
   - 監視フォルダ設定 (テキストボックス + 参照ボタン)
   - 保存先フォルダ設定 (テキストボックス + 参照ボタン)
   - プレフィックス設定 (JPEG用、RAW用)
   - プレビューアプリ設定 (テキストボックス + 参照ボタン)
   - 制御ボタン (監視開始/停止/プレビュー/設定保存)
   - ステータスバー
   - ログ表示エリア (スクロール可能)
✅ フォルダ選択ダイアログ実装
✅ ファイル選択ダイアログ実装 (プレビューアプリ用)
```

### 4. 設定管理実装
```
✅ 設定クラス作成 (AppSettings.cs)
✅ app.config 読み書き機能
✅ 設定項目:
   - MonitorMode (Immediate/Polling)
   - PollInterval (秒)
   - WatchJPEG (bool)
   - WatchRAW (bool)
   - MonitorFolder (string)
   - DestinationFolder (string)
   - FilePrefixJPEG (string, default: "IMG_")
   - FilePrefixRAW (string, default: "RAW_")
   - PreviewApp (string)
✅ 起動時設定読み込み
✅ 終了時設定保存
```

### 5. ファイル監視実装
```
✅ FileSystemWatcher実装 (ImmediateMonitor.cs)
   - Filter = "*.*"
   - Created/Renamed イベント処理
   - 対象拡張子判定
✅ ポーリング監視実装 (PollingMonitor.cs)
   - Timer-based フォルダスキャン
   - 新規ファイル検出ロジック
✅ 監視モード切り替え機能
✅ 対象拡張子管理 (JPEG: .jpg/.jpeg, RAW: .cr2/.cr3/.arw/.nef/.rw2/.orf/.dng)
```

### 6. ファイル処理実装
```
✅ ファイル書き込み完了待機 (最大10秒)
✅ 連番管理クラス (SequenceManager.cs)
   - JPEG用連番 (IMG_0001.jpg)
   - RAW用連番 (RAW_0001.cr2)
   - 既存ファイルから最大連番取得
✅ ファイルリネーム処理
✅ ファイル移動処理 (File.Move)
✅ 重複回避ロジック
✅ プロセス状態表示 (ステータスバー更新)
```

### 7. 安定性・例外処理実装
```
✅ UnhandledException ハンドラ
✅ 自動再起動機能
✅ FileSystemWatcher エラーハンドリング
✅ Timer例外処理
✅ ファイルアクセス例外処理
✅ リソースDispose (using文/Dispose pattern)
```

### 8. ログ機能実装
```
✅ ログクラス作成 (Logger.cs)
✅ エラーログファイル出力 (error.log)
✅ クラッシュログファイル出力 (crash.log)
✅ ログフォーマット: "YYYY-MM-DD HH:MM:SS - Message"
✅ UIログ表示 (エラーのみ)
✅ ログローテーション (オプション)
```

### 9. プレビュー機能実装
```
✅ 外部プレビューアプリ起動
✅ 保存先フォルダを引数として渡す
✅ Process.Start エラーハンドリング
```

### 10. ビルド・デプロイ
```
✅ Release ビルド設定
✅ PowerShell ビルドスクリプト (build.ps1)
✅ バッチファイル ビルドスクリプト (build.bat)
✅ GitHub Actions ワークフロー (.github/workflows/build.yml)
✅ Release用パッケージング
```

### 11. テスト
```
✅ 単体テスト項目作成
✅ 結合テスト項目作成
✅ テストデータ準備 (サンプルJPEG/RAWファイル)
✅ 連続処理テスト (100ファイル)
```

### 12. ドキュメント
```
✅ README.md 更新
✅ ユーザーマニュアル作成 (USER_MANUAL.md)
✅ 開発者ガイド作成 (DEVELOPER_GUIDE.md)
✅ トラブルシューティングガイド (TROUBLESHOOTING.md)
✅ API仕様書 (必要に応じて)
```

## コード品質基準
- C# コーディング規約準拠
- XML ドキュメントコメント必須
- 例外処理必須
- using文でリソース管理
- async/await パターン使用 (I/O処理)

## 完了条件
1. 全機能が正常に動作する
2. 連続100ファイル処理が安定動作する
3. エラー発生時にログが正しく出力される
4. 自動再起動が正常に機能する
5. 設定が正しく永続化される
6. ユーザーマニュアルが完備されている

## 成果物
- 実行可能ファイル (.exe)
- ソースコード (GitHubリポジトリ)
- ドキュメント一式
- ビルドスクリプト
- テスト項目書

## 注意事項
- Windows 11 Pro環境での動作を最優先
- エラーログ以外の冗長な出力は避ける
- メモリリークを避けるためリソース管理を徹底
- ユーザビリティを重視したUI設計
