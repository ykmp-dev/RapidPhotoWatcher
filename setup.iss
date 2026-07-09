[Setup]
AppId={{B8E8C8D8-1A2B-4C3D-8E9F-1234567890AB}
AppName=RapidPhotoWatcher
AppVersion=2.1.0
AppVerName=RapidPhotoWatcher 2.1.0
AppPublisher=YuberTokyo
AppPublisherURL=https://github.com/yubertokyo
AppSupportURL=https://github.com/yubertokyo/RapidPhotoWatcher
AppUpdatesURL=https://github.com/yubertokyo/RapidPhotoWatcher
DefaultDirName={autopf}\RapidPhotoWatcher
DefaultGroupName=RapidPhotoWatcher
AllowNoIcons=yes
LicenseFile=LICENSE
InfoBeforeFile=
InfoAfterFile=
OutputDir=.\Installer
OutputBaseFilename=RapidPhotoWatcher_v2.1.0_Setup
SetupIconFile=app_icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1
Name: "ftpsetup"; Description: "PC側FTPサーバー(IIS)を自動セットアップする（カメラからのFTP転送受信用・管理者権限が必要）"; GroupDescription: "FTPサーバー:"

[Files]
Source: ".\RapidPhotoWatcher.AvaloniaUI\bin\Release\net8.0\win-x64\publish\RapidPhotoWatcher.AvaloniaUI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\RapidPhotoWatcher.AvaloniaUI\bin\Release\net8.0\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".\Installer\Setup-FtpServer.ps1"; DestDir: "{app}\Scripts"; Flags: ignoreversion
Source: ".\Installer\Remove-FtpServer.ps1"; DestDir: "{app}\Scripts"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.AvaloniaUI.exe"
Name: "{group}\{cm:UninstallProgram,RapidPhotoWatcher}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.AvaloniaUI.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.AvaloniaUI.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\RapidPhotoWatcher.AvaloniaUI.exe"; Description: "{cm:LaunchProgram,RapidPhotoWatcher}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}\RapidPhotoWatcher"

[Code]
var
  FtpPage: TInputQueryPage;

procedure InitializeWizard;
begin
  WizardForm.WelcomeLabel2.Caption :=
    '写真ファイルの高速監視・自動整理を行うアプリケーション (v2.1.0 - Avalonia UI版)' + #13#10#13#10 +
    'このアプリケーションは以下の機能を提供します：' + #13#10 +
    '• フォルダ監視によるファイル自動処理' + #13#10 +
    '• カスタマイズ可能なファイル名規則' + #13#10 +
    '• 外部ソフトウェア連携機能' + #13#10 +
    '• 詳細なログ機能' + #13#10 +
    '• PC側FTPサーバーの自動セットアップ（カメラからのFTP転送受信）' + #13#10 +
    '• クロスプラットフォーム対応（Avalonia UI）';

  { FTPサーバー自動セットアップ用の入力ページ }
  FtpPage := CreateInputQueryPage(wpSelectTasks,
    'FTPサーバー自動セットアップ',
    'カメラからのFTP転送を受信するための設定',
    'カメラ側に設定するFTPアカウント情報を入力してください。' + #13#10 +
    'インストール完了時にIIS FTPサーバーを自動構成します（管理者権限の確認が表示されます）。');
  FtpPage.Add('FTPユーザー名:', False);
  FtpPage.Add('FTPパスワード:', True);
  FtpPage.Add('FTPポート番号:', False);
  FtpPage.Add('FTP受信フォルダ（RapidPhotoWatcherの監視フォルダ）:', False);
  FtpPage.Values[0] := 'camera';
  FtpPage.Values[1] := '';
  FtpPage.Values[2] := '21';
  FtpPage.Values[3] := ExpandConstant('{userpics}\RapidPhotoWatcher\FTP');
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  { FTPセットアップのタスクが選択されていない場合は入力ページをスキップ }
  Result := (PageID = FtpPage.ID) and (not WizardIsTaskSelected('ftpsetup'));
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  Port: Integer;
begin
  Result := True;
  if CurPageID = FtpPage.ID then
  begin
    if Trim(FtpPage.Values[0]) = '' then
    begin
      MsgBox('FTPユーザー名を入力してください。', mbError, MB_OK);
      Result := False;
      exit;
    end;
    if FtpPage.Values[1] = '' then
    begin
      MsgBox('FTPパスワードを入力してください。', mbError, MB_OK);
      Result := False;
      exit;
    end;
    if Pos('"', FtpPage.Values[0] + FtpPage.Values[1]) > 0 then
    begin
      MsgBox('FTPユーザー名とパスワードにダブルクォート ( " ) は使用できません。', mbError, MB_OK);
      Result := False;
      exit;
    end;
    Port := StrToIntDef(Trim(FtpPage.Values[2]), 0);
    if (Port < 1) or (Port > 65535) then
    begin
      MsgBox('FTPポート番号は 1～65535 の数値で入力してください。', mbError, MB_OK);
      Result := False;
      exit;
    end;
    if Trim(FtpPage.Values[3]) = '' then
    begin
      MsgBox('FTP受信フォルダを入力してください。', mbError, MB_OK);
      Result := False;
      exit;
    end;
  end;
end;

{ JSON用にパス区切りを / へ変換（.NET側は / も解釈可能） }
function ToJsonPath(const S: String): String;
var
  T: String;
begin
  T := S;
  StringChangeEx(T, '\', '/', True);
  Result := T;
end;

{ アプリが初回起動時に監視フォルダの初期値として読み込む設定ファイルを書き出す }
procedure WriteFtpConfigFile;
var
  Dir: String;
  Lines: TArrayOfString;
begin
  Dir := ExpandConstant('{userappdata}\RapidPhotoWatcher');
  ForceDirectories(Dir);
  SetArrayLength(Lines, 5);
  Lines[0] := '{';
  Lines[1] := '  "ftpFolder": "' + ToJsonPath(Trim(FtpPage.Values[3])) + '",';
  Lines[2] := '  "ftpUserName": "' + Trim(FtpPage.Values[0]) + '",';
  Lines[3] := '  "ftpPort": ' + Trim(FtpPage.Values[2]);
  Lines[4] := '}';
  SaveStringsToUTF8File(Dir + '\ftp-config.json', Lines, False);
end;

{ FTPセットアップスクリプトを管理者権限で実行 }
procedure RunFtpSetup;
var
  Script, Params: String;
  ResultCode: Integer;
begin
  Script := ExpandConstant('{app}\Scripts\Setup-FtpServer.ps1');
  Params := '-NoProfile -ExecutionPolicy Bypass -File "' + Script + '"' +
            ' -FtpRoot "' + Trim(FtpPage.Values[3]) + '"' +
            ' -UserName "' + Trim(FtpPage.Values[0]) + '"' +
            ' -Password "' + FtpPage.Values[1] + '"' +
            ' -Port ' + Trim(FtpPage.Values[2]);
  if (not ShellExec('runas', 'powershell.exe', Params, '', SW_SHOW, ewWaitUntilTerminated, ResultCode))
     or (ResultCode <> 0) then
  begin
    MsgBox('FTPサーバーの自動セットアップに失敗しました（またはキャンセルされました）。' + #13#10#13#10 +
           '後から手動でセットアップする場合は、管理者権限のPowerShellで以下を実行してください:' + #13#10#13#10 +
           'powershell -ExecutionPolicy Bypass -File "' + Script + '"',
           mbError, MB_OK);
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep = ssPostInstall) and WizardIsTaskSelected('ftpsetup') then
  begin
    WriteFtpConfigFile;
    RunFtpSetup;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  Script: String;
  ResultCode: Integer;
begin
  if CurUninstallStep = usUninstall then
  begin
    Script := ExpandConstant('{app}\Scripts\Remove-FtpServer.ps1');
    if FileExists(Script) and
       FileExists(ExpandConstant('{userappdata}\RapidPhotoWatcher\ftp-config.json')) then
    begin
      if MsgBox('RapidPhotoWatcher用に構成したFTPサーバー設定' + #13#10 +
                '（IIS FTPサイト・ファイアウォール規則）も削除しますか？' + #13#10#13#10 +
                '※ FTP受信フォルダ内の写真は削除されません。',
                mbConfirmation, MB_YESNO) = IDYES then
      begin
        ShellExec('runas', 'powershell.exe',
          '-NoProfile -ExecutionPolicy Bypass -File "' + Script + '" -NoPause',
          '', SW_SHOW, ewWaitUntilTerminated, ResultCode);
      end;
    end;
  end;
end;