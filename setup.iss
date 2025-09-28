[Setup]
AppId={{B8E8C8D8-1A2B-4C3D-8E9F-1234567890AB}
AppName=RapidPhotoWatcher
AppVersion=1.1.0
AppVerName=RapidPhotoWatcher 1.1.0
AppPublisher=YuberTokyo
AppPublisherURL=https://github.com/yuber-tokyo
AppSupportURL=https://github.com/yuber-tokyo/rapidphotowatcher
AppUpdatesURL=https://github.com/yuber-tokyo/rapidphotowatcher
DefaultDirName={autopf}\RapidPhotoWatcher
DefaultGroupName=RapidPhotoWatcher
AllowNoIcons=yes
LicenseFile=LICENSE
InfoBeforeFile=
InfoAfterFile=
OutputDir=.\Installer
OutputBaseFilename=RapidPhotoWatcher_v1.1.0_Setup
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

[Files]
Source: ".\bin\Release\net6.0-windows\win-x64\publish\RapidPhotoWatcher.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.exe"
Name: "{group}\{cm:UninstallProgram,RapidPhotoWatcher}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\RapidPhotoWatcher"; Filename: "{app}\RapidPhotoWatcher.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\RapidPhotoWatcher.exe"; Description: "{cm:LaunchProgram,RapidPhotoWatcher}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}\RapidPhotoWatcher"

[Code]
procedure InitializeWizard;
begin
  WizardForm.WelcomeLabel2.Caption := 
    '写真ファイルの高速監視・自動整理を行うアプリケーション' + #13#10#13#10 +
    'このアプリケーションは以下の機能を提供します：' + #13#10 +
    '• フォルダ監視によるファイル自動処理' + #13#10 +
    '• カスタマイズ可能なファイル名規則' + #13#10 +
    '• 外部ソフトウェア連携機能' + #13#10 +
    '• 詳細なログ機能';
end;