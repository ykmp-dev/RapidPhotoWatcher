using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            
            numSequenceDigits.Value = _settings.SequenceDigits;
            numSequenceStart.Value = _settings.SequenceStartNumber;
            
            // 連携ソフトウェア設定
            chkAutoActivate.Checked = _settings.AutoActivateExternalSoftware;
            cmbNavigationDirection.SelectedIndex = (int)_settings.NavigationDirection;
            
            UpdatePrefixTypeUI();
            UpdateCurrentSequenceDisplay();
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
            _settings.ExternalSoftwarePath = txtExternalSoftware.Text; // 外部ソフトウェアパスに変更
            _settings.MonitorMode = radioImmediate.Checked ? MonitorMode.Immediate : MonitorMode.Polling;
            _settings.MonitorJPEG = chkJPEG.Checked;
            _settings.MonitorRAW = chkRAW.Checked;
            
            // 新しい設定項目
            _settings.PrefixType = radioPrefixCustom.Checked ? PrefixType.CustomText : PrefixType.DateTime;
            _settings.DateTimeFormatType = (DateTimeFormatType)cmbDateTimeFormat.SelectedIndex;
            _settings.SequenceDigits = (int)numSequenceDigits.Value;
            _settings.SequenceStartNumber = (int)numSequenceStart.Value;
            
            // 連携ソフトウェア設定
            _settings.AutoActivateExternalSoftware = chkAutoActivate.Checked;
            _settings.NavigationDirection = (ImageNavigationDirection)cmbNavigationDirection.SelectedIndex;
            
            _logManager.LogInfo($"設定保存: AutoActivate={_settings.AutoActivateExternalSoftware}, ExternalPath='{_settings.ExternalSoftwarePath}'");
        }

        /// <summary>
        /// 監視状態変更
        /// </summary>
        private void SetMonitoringState(bool isMonitoring)
        {
            btnStart.Enabled = !isMonitoring;
            btnStop.Enabled = isMonitoring;
            btnExit.Enabled = !isMonitoring; // 監視停止中のみ終了可能
            
            // タブ全体を無効化/有効化
            tabControl.Enabled = !isMonitoring;
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
        /// 設定変更時の処理（空の実装）
        /// </summary>
        private void OnSettingsChanged(object sender, EventArgs e)
        {
            // リアルタイム保存は行わない - 終了時のみ保存
        }

        /// <summary>
        /// プレフィックスタイプ変更時の処理
        /// </summary>
        private void OnPrefixTypeChanged(object sender, EventArgs e)
        {
            UpdatePrefixTypeUI();
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
            MessageBox.Show($"連番を{_settings.SequenceStartNumber}にリセットしました。", "情報", 
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