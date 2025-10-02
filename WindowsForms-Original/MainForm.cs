using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// メインフォーム - JPEG/RAWファイル監視アプリケーションのメインUI
    /// </summary>
    public partial class MainForm : Form
    {
        private FileMonitorService? _fileMonitorService;
        private AppSettings _settings;
        private LogManager _logManager;

        public MainForm()
        {
            InitializeComponent();
            _settings = new AppSettings();
            _logManager = new LogManager();
            InitializeServices();
            LoadSettings();
            
            // タスクバーアイコンを設定
            try
            {
                // 実行ファイルと同じディレクトリのアイコンファイルを使用
                string iconPath = Path.Combine(Application.StartupPath, "app_icon.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    // フォールバック: プロジェクトディレクトリから読み込み
                    string fallbackPath = @"D:\Self-work\JPEGFolderMonitor\app_icon.ico";
                    if (File.Exists(fallbackPath))
                    {
                        this.Icon = new Icon(fallbackPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // アイコンファイルが見つからない場合は無視
                _logManager?.LogError($"アイコン読み込みエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// サービス初期化
        /// </summary>
        private void InitializeServices()
        {
            _fileMonitorService = new FileMonitorService(_logManager);
            _fileMonitorService.FileProcessed += OnFileProcessed;
            _fileMonitorService.ErrorOccurred += OnErrorOccurred;
        }

        /// <summary>
        /// 設定読み込み
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                _settings.Load();
                ApplySettingsToUI();
                _logManager.LogInfo("設定ファイルを読み込みました");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"設定読み込みエラー: {ex.Message}");
                MessageBox.Show($"設定の読み込みに失敗しました: {ex.Message}\nデフォルト設定を使用します。", "警告", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // デフォルト設定を適用
                ApplySettingsToUI();
            }
        }

        /// <summary>
        /// 設定をUIに反映
        /// </summary>
        private void ApplySettingsToUI()
        {
            txtSourceFolder.Text = _settings.SourceFolder;
            txtDestinationFolder.Text = _settings.DestinationFolder;
            txtFilePrefix.Text = _settings.FilePrefix;
            numPollingInterval.Value = _settings.PollingInterval;
            txtExternalSoftware.Text = _settings.ExternalSoftwarePath; // 外部ソフトウェアパスに変更
            
            radioImmediate.Checked = _settings.MonitorMode == MonitorMode.Immediate;
            radioPolling.Checked = _settings.MonitorMode == MonitorMode.Polling;
            
            chkJPEG.Checked = _settings.MonitorJPEG;
            chkRAW.Checked = _settings.MonitorRAW;
            
            // 新しい設定項目のUI反映
            radioPrefixCustom.Checked = _settings.PrefixType == PrefixType.CustomText;
            radioPrefixDateTime.Checked = _settings.PrefixType == PrefixType.DateTime;
            
            cmbDateTimeFormat.SelectedIndex = (int)_settings.DateTimeFormatType;
            
            cmbSeparatorType.SelectedIndex = (int)_settings.SeparatorType;
            
            numSequenceDigits.Value = _settings.SequenceDigits;
            numSequenceStart.Value = _settings.SequenceStartNumber;
            
            // 連携ソフトウェア設定
            chkAutoActivate.Checked = _settings.AutoActivateExternalSoftware;
            
            UpdatePrefixTypeUI();
            UpdateCurrentSequenceDisplay();
            UpdateFileNamePreview();
        }

        /// <summary>
        /// 監視開始
        /// </summary>
        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateSettings())
                    return;

                SaveCurrentSettings();
                
                await _fileMonitorService!.StartMonitoringAsync(_settings);

                SetMonitoringState(true);
                _logManager.LogInfo("監視を開始しました");
                UpdateStatusBar("監視中...");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"監視開始エラー: {ex.Message}");
                MessageBox.Show($"監視の開始に失敗しました: {ex.Message}", "エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 監視停止
        /// </summary>
        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                await _fileMonitorService!.StopMonitoringAsync();
                SetMonitoringState(false);
                _logManager.LogInfo("監視を停止しました");
                UpdateStatusBar("停止中");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"監視停止エラー: {ex.Message}");
                MessageBox.Show($"監視の停止に失敗しました: {ex.Message}", "エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 設定検証
        /// </summary>
        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(txtSourceFolder.Text))
            {
                MessageBox.Show("監視フォルダを選択してください。", "入力エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Directory.Exists(txtSourceFolder.Text))
            {
                MessageBox.Show("監視フォルダが存在しません。", "入力エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestinationFolder.Text))
            {
                MessageBox.Show("移動先フォルダを選択してください。", "入力エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!chkJPEG.Checked && !chkRAW.Checked)
            {
                MessageBox.Show("監視するファイル形式を選択してください。", "入力エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ファイル自動削除の警告
            if (chkJPEG.Checked && !chkRAW.Checked)
            {
                // JPEGのみ監視時のRAWファイル削除警告
                return ShowRawFileDeletionWarning();
            }
            else if (!chkJPEG.Checked && chkRAW.Checked)
            {
                // RAWのみ監視時のJPEGファイル削除警告
                return ShowJpegFileDeletionWarning();
            }
            else
            {
                // 両方監視する場合は削除フラグをリセット
                _settings.AutoDeleteRawFiles = false;
                _settings.AutoDeleteJpegFiles = false;
            }

            return true;
        }

        /// <summary>
        /// RAWファイル削除警告ダイアログ
        /// </summary>
        private bool ShowRawFileDeletionWarning()
        {
            var message = "JPEGのみの監視が選択されています。\n\n" +
                         "カメラがRAW+JPEG形式で撮影している場合、\n" +
                         "監視停止時にRAWファイルは自動的に削除されます。\n\n" +
                         "RAWファイルが必要な場合は、RAWファイル監視にもチェックを入れてください。\n\n" +
                         "監視を開始しますか？";

            var result = MessageBox.Show(message, "RAWファイル削除の確認", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _settings.AutoDeleteRawFiles = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// JPEGファイル削除警告ダイアログ
        /// </summary>
        private bool ShowJpegFileDeletionWarning()
        {
            var message = "RAWのみの監視が選択されています。\n\n" +
                         "カメラがRAW+JPEG形式で撮影している場合、\n" +
                         "監視停止時にJPEGファイルは自動的に削除されます。\n\n" +
                         "JPEGファイルが必要な場合は、JPEGファイル監視にもチェックを入れてください。\n\n" +
                         "監視を開始しますか？";

            var result = MessageBox.Show(message, "JPEGファイル削除の確認", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _settings.AutoDeleteJpegFiles = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 現在の設定を保存
        /// </summary>
        private void SaveCurrentSettings()
        {
            _settings.SourceFolder = txtSourceFolder.Text;
            _settings.DestinationFolder = txtDestinationFolder.Text;
            _settings.FilePrefix = txtFilePrefix.Text;
            _settings.PollingInterval = (int)numPollingInterval.Value;
            _settings.ExternalSoftwarePath = txtExternalSoftware.Text; // 外部ソフトウェアパスに変更
            _settings.MonitorMode = radioImmediate.Checked ? MonitorMode.Immediate : MonitorMode.Polling;
            _settings.MonitorJPEG = chkJPEG.Checked;
            _settings.MonitorRAW = chkRAW.Checked;
            
            // 新しい設定項目
            _settings.PrefixType = radioPrefixCustom.Checked ? PrefixType.CustomText : PrefixType.DateTime;
            _settings.DateTimeFormatType = (DateTimeFormatType)cmbDateTimeFormat.SelectedIndex;
            _settings.SeparatorType = (SeparatorType)cmbSeparatorType.SelectedIndex;
            _settings.SequenceDigits = (int)numSequenceDigits.Value;
            _settings.SequenceStartNumber = (int)numSequenceStart.Value;
            
            // 連携ソフトウェア設定
            _settings.AutoActivateExternalSoftware = chkAutoActivate.Checked;
            
            _logManager.LogInfo($"設定保存: AutoActivate={_settings.AutoActivateExternalSoftware}, AutoDeleteRaw={_settings.AutoDeleteRawFiles}, AutoDeleteJpeg={_settings.AutoDeleteJpegFiles}, ExternalPath='{_settings.ExternalSoftwarePath}'");
        }

        /// <summary>
        /// 監視状態変更
        /// </summary>
        private void SetMonitoringState(bool isMonitoring)
        {
            // 全ての開始ボタン
            btnStart.Enabled = !isMonitoring;
            btnBasicStart.Enabled = !isMonitoring;
            btnExternalStart.Enabled = !isMonitoring;
            
            // 全ての停止ボタン
            btnStop.Enabled = isMonitoring;
            btnBasicStop.Enabled = isMonitoring;
            btnExternalStop.Enabled = isMonitoring;
            
            // 全ての終了ボタン（監視停止中のみ終了可能）
            btnExit.Enabled = !isMonitoring;
            btnBasicExit.Enabled = !isMonitoring;
            btnExternalExit.Enabled = !isMonitoring;
            
            // 個別の設定コントロールを無効化/有効化（操作ボタンは除く）
            SetSettingsControlsEnabled(!isMonitoring);
        }

        /// <summary>
        /// 設定コントロールの有効/無効状態を設定
        /// </summary>
        private void SetSettingsControlsEnabled(bool enabled)
        {
            // 基本設定タブの設定項目
            txtSourceFolder.Enabled = enabled;
            txtDestinationFolder.Enabled = enabled;
            btnBrowseSource.Enabled = enabled;
            btnBrowseDestination.Enabled = enabled;
            grpMonitorMode.Enabled = enabled;
            grpFileTypes.Enabled = enabled;
            
            // ファイル命名タブの設定項目
            grpPrefixType.Enabled = enabled;
            grpCustomPrefix.Enabled = enabled;
            grpDateTimeFormat.Enabled = enabled;
            grpSeparator.Enabled = enabled;
            grpSequencing.Enabled = enabled;
            
            // 連携ソフトウェアタブの設定項目
            txtExternalSoftware.Enabled = enabled;
            btnBrowseExternal.Enabled = enabled;
            grpSoftwareSettings.Enabled = enabled;
        }

        /// <summary>
        /// ファイル処理完了イベント
        /// </summary>
        private void OnFileProcessed(object? sender, FileProcessedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object?, FileProcessedEventArgs>(OnFileProcessed), sender, e);
                return;
            }

            var message = $"{e.OriginalPath} → {e.NewPath}";
            _logManager.LogInfo($"ファイル処理完了: {message}");
            AppendLog($"[{DateTime.Now:HH:mm:ss}] {message}");
            
            // 連番表示を更新
            UpdateCurrentSequenceDisplay();
        }

        /// <summary>
        /// エラー発生イベント
        /// </summary>
        private void OnErrorOccurred(object? sender, ErrorEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object?, ErrorEventArgs>(OnErrorOccurred), sender, e);
                return;
            }

            _logManager.LogError($"エラー: {e.Message}");
            AppendLog($"[{DateTime.Now:HH:mm:ss}] エラー: {e.Message}");
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        private void AppendLog(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        /// <summary>
        /// ステータスバー更新
        /// </summary>
        private void UpdateStatusBar(string message)
        {
            lblStatus.Text = message;
        }



        /// <summary>
        /// フォルダ選択（監視フォルダ）
        /// </summary>
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "監視するフォルダを選択してください";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSourceFolder.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// フォルダ選択（移動先フォルダ）
        /// </summary>
        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "移動先フォルダを選択してください";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDestinationFolder.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 設定変更時の処理
        /// </summary>
        private void OnSettingsChanged(object sender, EventArgs e)
        {
            // プレビューを更新
            UpdateFileNamePreview();
        }

        /// <summary>
        /// プレフィックスタイプ変更時の処理
        /// </summary>
        private void OnPrefixTypeChanged(object sender, EventArgs e)
        {
            UpdatePrefixTypeUI();
            UpdateFileNamePreview();
        }

        /// <summary>
        /// プレフィックスタイプに応じてUIを更新
        /// </summary>
        private void UpdatePrefixTypeUI()
        {
            grpCustomPrefix.Enabled = radioPrefixCustom.Checked;
            grpDateTimeFormat.Enabled = radioPrefixDateTime.Checked;
        }

        /// <summary>
        /// 連番リセット
        /// </summary>
        private void btnResetSequence_Click(object sender, EventArgs e)
        {
            _settings.ResetSequenceNumber();
            UpdateCurrentSequenceDisplay();
            UpdateFileNamePreview();
            MessageBox.Show("連番を1にリセットしました。", "情報", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 現在の連番を表示更新
        /// </summary>
        private void UpdateCurrentSequenceDisplay()
        {
            // 開始番号の設定値を現在の連番に更新
            numSequenceStart.Value = _settings.CurrentSequenceNumber;
        }

        /// <summary>
        /// ファイル名プレビューを更新
        /// </summary>
        private void UpdateFileNamePreview()
        {
            try
            {
                // 現在のUI設定を一時的にAppSettingsに反映してプレビューを生成
                var tempSettings = new AppSettings
                {
                    PrefixType = radioPrefixCustom.Checked ? PrefixType.CustomText : PrefixType.DateTime,
                    FilePrefix = txtFilePrefix.Text,
                    DateTimeFormatType = (DateTimeFormatType)cmbDateTimeFormat.SelectedIndex,
                    SeparatorType = (SeparatorType)cmbSeparatorType.SelectedIndex,
                    SequenceDigits = (int)numSequenceDigits.Value,
                    CurrentSequenceNumber = (int)numSequenceStart.Value
                };

                var preview = tempSettings.GenerateFileNamePreview();
                lblFileNamePreview.Text = preview;
            }
            catch (Exception ex)
            {
                lblFileNamePreview.Text = "プレビューエラー";
                _logManager?.LogError($"プレビュー生成エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 外部ソフトウェア選択
        /// </summary>
        private void btnBrowseExternal_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Title = "連携する外部ソフトウェアを選択してください";
            dialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtExternalSoftware.Text = dialog.FileName;
            }
        }



        /// <summary>
        /// ファイル選択（外部ソフトウェア） - 後方互換性
        /// </summary>
        private void btnBrowsePreview_Click(object sender, EventArgs e)
        {
            btnBrowseExternal_Click(sender, e);
        }

        /// <summary>
        /// 終了ボタンクリック
        /// </summary>
        private async void btnExit_Click(object sender, EventArgs e)
        {
            await ExitApplicationAsync();
        }

        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        private async Task ExitApplicationAsync()
        {
            try
            {
                // 監視が実行中の場合は先に停止
                if (_fileMonitorService != null && btnStop.Enabled)
                {
                    _logManager.LogInfo("監視を停止しています...");
                    await _fileMonitorService.StopMonitoringAsync();
                }

                // 全ての設定を現在のUIの状態で保存
                SaveCurrentSettings();
                _settings.Save();
                _logManager.LogInfo("設定ファイルを保存しました");

                // アプリケーション終了
                _logManager.LogInfo("アプリケーションを終了します");
                Application.Exit();
            }
            catch (Exception ex)
            {
                _logManager.LogError($"終了処理エラー: {ex.Message}");
                // エラーが発生してもアプリケーションを強制終了
                try
                {
                    _settings.Save(); // 最後の試み
                }
                catch
                {
                    // 無視
                }
                Application.Exit();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Xボタンを無効化しているため、このメソッドは通常呼ばれない
            base.OnFormClosing(e);
        }

    }
}