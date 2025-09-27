# リリースガイド

## リリース手順

### 1. バージョン更新
1. `JPEGFolderMonitor.csproj` のバージョン番号を更新
   - `<Version>1.0.0</Version>`
   - `<AssemblyVersion>1.0.0.0</AssemblyVersion>`
   - `<FileVersion>1.0.0.0</FileVersion>`

### 2. Git タグの作成とプッシュ
```bash
git add .
git commit -m "Release v1.0.0"
git tag v1.0.0
git push origin main
git push origin v1.0.0
```

### 3. 自動ビルドとリリース
- GitHub Actionsが自動的に実行されます
- Windows x64用のEXEファイルが生成されます
- GitHubリリースページに自動的にアップロードされます

### 4. ローカルでのテストビルド
```bash
# Windows
build.bat

# または手動で
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 配布ファイル構成

### GitHub Releases
- `JPEGFolderMonitor-win-x64.zip` 
  - 単一EXEファイル (セルフコンテインド)
  - .NET Runtime不要
  - Windows 10/11 x64対応

### ファイルサイズ
- 約 100-150MB (全依存関係含む)
- 初回起動時の展開で数秒要する場合があります

## ユーザー向け情報

### システム要件
- Windows 10 バージョン 1809 以降
- Windows 11 (全バージョン)
- x64 アーキテクチャ
- 約 200MB の空き容量

### インストール方法
1. [Releases](https://github.com/yubertokyo/jpeg-raw-folder-monitor/releases) から最新版をダウンロード
2. ZIPファイルを任意のフォルダに展開
3. `JPEGFolderMonitor.exe` をダブルクリックして実行

### アンインストール方法
1. アプリケーションを終了
2. 展開したフォルダを削除
3. `%APPDATA%\JPEGFolderMonitor` フォルダを削除（設定とログを完全削除したい場合）

## トラブルシューティング

### Windows Defenderの警告
初回実行時にWindows Defenderが警告を表示する場合があります。
「詳細情報」→「実行」で起動できます。

### 起動しない場合
1. Windows Updateを最新にする
2. 管理者権限で実行してみる
3. ウイルス対策ソフトの除外設定を確認

## 開発者向け情報

### ビルド環境
- .NET 6.0 SDK
- Windows 10/11
- Visual Studio 2022 または VS Code (推奨)

### デバッグ実行
```bash
dotnet run
```

### 依存関係の更新
```bash
dotnet list package --outdated
dotnet add package [PackageName] --version [Version]
```