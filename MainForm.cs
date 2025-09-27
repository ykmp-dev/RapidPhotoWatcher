using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JPEGFolderMonitor
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
            }
            catch (Exception ex)
            {
                _logManager.LogError($"設定読み込みエラー: {ex.Message}");
                MessageBox.Show($"設定の読み込みに失敗しました: {ex.Message}", "エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            txtPreviewApp.Text = _settings.PreviewAppPath;
            
            radioImmediate.Checked = _settings.MonitorMode == MonitorMode.Immediate;
            radioPolling.Checked = _settings.MonitorMode == MonitorMode.Polling;
            
            chkJPEG.Checked = _settings.MonitorJPEG;
            chkRAW.Checked = _settings.MonitorRAW;
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
                
                var monitorMode = radioImmediate.Checked ? MonitorMode.Immediate : MonitorMode.Polling;
                
                await _fileMonitorService!.StartMonitoringAsync(
                    _settings.SourceFolder!,
                    _settings.DestinationFolder!,
                    _settings.FilePrefix!,
                    monitorMode,
                    _settings.PollingInterval,
                    _settings.MonitorJPEG,
                    _settings.MonitorRAW);

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

            return true;
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
            _settings.PreviewAppPath = txtPreviewApp.Text;
            _settings.MonitorMode = radioImmediate.Checked ? MonitorMode.Immediate : MonitorMode.Polling;
            _settings.MonitorJPEG = chkJPEG.Checked;
            _settings.MonitorRAW = chkRAW.Checked;
        }

        /// <summary>
        /// 監視状態変更
        /// </summary>
        private void SetMonitoringState(bool isMonitoring)
        {
            btnStart.Enabled = !isMonitoring;
            btnStop.Enabled = isMonitoring;
            
            grpSettings.Enabled = !isMonitoring;
            grpFileTypes.Enabled = !isMonitoring;
            grpMonitorMode.Enabled = !isMonitoring;
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
        /// 設定保存
        /// </summary>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCurrentSettings();
                _settings.Save();
                MessageBox.Show("設定を保存しました。", "情報", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logManager.LogError($"設定保存エラー: {ex.Message}");
                MessageBox.Show($"設定の保存に失敗しました: {ex.Message}", "エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// プレビュー実行
        /// </summary>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_settings.PreviewAppPath) || 
                    !File.Exists(_settings.PreviewAppPath))
                {
                    MessageBox.Show("プレビューアプリケーションが設定されていないか、存在しません。", 
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var previewService = new PreviewService(_settings.PreviewAppPath);
                var files = Directory.GetFiles(_settings.SourceFolder ?? "", "*.*", SearchOption.TopDirectoryOnly);
                
                foreach (var file in files)
                {
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (FileExtensions.IsJPEG(ext) || FileExtensions.IsRAW(ext))
                    {
                        previewService.OpenFile(file);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"プレビューエラー: {ex.Message}");
                MessageBox.Show($"プレビューの実行に失敗しました: {ex.Message}", "エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// ファイル選択（プレビューアプリ）
        /// </summary>
        private void btnBrowsePreview_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Title = "プレビューアプリケーションを選択してください";
            dialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPreviewApp.Text = dialog.FileName;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _fileMonitorService?.StopMonitoringAsync().Wait();
                _settings.Save();
            }
            catch (Exception ex)
            {
                _logManager.LogError($"終了処理エラー: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }
    }
}