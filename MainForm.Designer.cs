namespace JPEGFolderMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.btnBrowsePreview = new System.Windows.Forms.Button();
            this.txtPreviewApp = new System.Windows.Forms.TextBox();
            this.lblPreviewApp = new System.Windows.Forms.Label();
            this.txtFilePrefix = new System.Windows.Forms.TextBox();
            this.lblFilePrefix = new System.Windows.Forms.Label();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblDestinationFolder = new System.Windows.Forms.Label();
            this.txtSourceFolder = new System.Windows.Forms.TextBox();
            this.lblSourceFolder = new System.Windows.Forms.Label();
            this.grpMonitorMode = new System.Windows.Forms.GroupBox();
            this.numPollingInterval = new System.Windows.Forms.NumericUpDown();
            this.lblPollingInterval = new System.Windows.Forms.Label();
            this.radioPolling = new System.Windows.Forms.RadioButton();
            this.radioImmediate = new System.Windows.Forms.RadioButton();
            this.grpFileTypes = new System.Windows.Forms.GroupBox();
            this.chkRAW = new System.Windows.Forms.CheckBox();
            this.chkJPEG = new System.Windows.Forms.CheckBox();
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpSettings.SuspendLayout();
            this.grpMonitorMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).BeginInit();
            this.grpFileTypes.SuspendLayout();
            this.grpControls.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnBrowseDestination);
            this.grpSettings.Controls.Add(this.btnBrowseSource);
            this.grpSettings.Controls.Add(this.btnBrowsePreview);
            this.grpSettings.Controls.Add(this.txtPreviewApp);
            this.grpSettings.Controls.Add(this.lblPreviewApp);
            this.grpSettings.Controls.Add(this.txtFilePrefix);
            this.grpSettings.Controls.Add(this.lblFilePrefix);
            this.grpSettings.Controls.Add(this.txtDestinationFolder);
            this.grpSettings.Controls.Add(this.lblDestinationFolder);
            this.grpSettings.Controls.Add(this.txtSourceFolder);
            this.grpSettings.Controls.Add(this.lblSourceFolder);
            this.grpSettings.Location = new System.Drawing.Point(12, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(500, 140);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "設定";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(460, 50);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDestination.TabIndex = 10;
            this.btnBrowseDestination.Text = "...";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(460, 20);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseSource.TabIndex = 9;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // btnBrowsePreview
            // 
            this.btnBrowsePreview.Location = new System.Drawing.Point(460, 110);
            this.btnBrowsePreview.Name = "btnBrowsePreview";
            this.btnBrowsePreview.Size = new System.Drawing.Size(30, 23);
            this.btnBrowsePreview.TabIndex = 8;
            this.btnBrowsePreview.Text = "...";
            this.btnBrowsePreview.UseVisualStyleBackColor = true;
            this.btnBrowsePreview.Click += new System.EventHandler(this.btnBrowsePreview_Click);
            // 
            // txtPreviewApp
            // 
            this.txtPreviewApp.Location = new System.Drawing.Point(120, 110);
            this.txtPreviewApp.Name = "txtPreviewApp";
            this.txtPreviewApp.Size = new System.Drawing.Size(334, 23);
            this.txtPreviewApp.TabIndex = 7;
            // 
            // lblPreviewApp
            // 
            this.lblPreviewApp.AutoSize = true;
            this.lblPreviewApp.Location = new System.Drawing.Point(10, 113);
            this.lblPreviewApp.Name = "lblPreviewApp";
            this.lblPreviewApp.Size = new System.Drawing.Size(75, 15);
            this.lblPreviewApp.TabIndex = 6;
            this.lblPreviewApp.Text = "プレビューアプリ:";
            // 
            // txtFilePrefix
            // 
            this.txtFilePrefix.Location = new System.Drawing.Point(120, 80);
            this.txtFilePrefix.Name = "txtFilePrefix";
            this.txtFilePrefix.Size = new System.Drawing.Size(334, 23);
            this.txtFilePrefix.TabIndex = 5;
            // 
            // lblFilePrefix
            // 
            this.lblFilePrefix.AutoSize = true;
            this.lblFilePrefix.Location = new System.Drawing.Point(10, 83);
            this.lblFilePrefix.Name = "lblFilePrefix";
            this.lblFilePrefix.Size = new System.Drawing.Size(77, 15);
            this.lblFilePrefix.TabIndex = 4;
            this.lblFilePrefix.Text = "ファイル接頭辞:";
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(120, 50);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(334, 23);
            this.txtDestinationFolder.TabIndex = 3;
            // 
            // lblDestinationFolder
            // 
            this.lblDestinationFolder.AutoSize = true;
            this.lblDestinationFolder.Location = new System.Drawing.Point(10, 53);
            this.lblDestinationFolder.Name = "lblDestinationFolder";
            this.lblDestinationFolder.Size = new System.Drawing.Size(67, 15);
            this.lblDestinationFolder.TabIndex = 2;
            this.lblDestinationFolder.Text = "移動先フォルダ:";
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.Location = new System.Drawing.Point(120, 20);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.Size = new System.Drawing.Size(334, 23);
            this.txtSourceFolder.TabIndex = 1;
            // 
            // lblSourceFolder
            // 
            this.lblSourceFolder.AutoSize = true;
            this.lblSourceFolder.Location = new System.Drawing.Point(10, 23);
            this.lblSourceFolder.Name = "lblSourceFolder";
            this.lblSourceFolder.Size = new System.Drawing.Size(67, 15);
            this.lblSourceFolder.TabIndex = 0;
            this.lblSourceFolder.Text = "監視フォルダ:";
            // 
            // grpMonitorMode
            // 
            this.grpMonitorMode.Controls.Add(this.numPollingInterval);
            this.grpMonitorMode.Controls.Add(this.lblPollingInterval);
            this.grpMonitorMode.Controls.Add(this.radioPolling);
            this.grpMonitorMode.Controls.Add(this.radioImmediate);
            this.grpMonitorMode.Location = new System.Drawing.Point(12, 158);
            this.grpMonitorMode.Name = "grpMonitorMode";
            this.grpMonitorMode.Size = new System.Drawing.Size(240, 80);
            this.grpMonitorMode.TabIndex = 1;
            this.grpMonitorMode.TabStop = false;
            this.grpMonitorMode.Text = "監視モード";
            // 
            // numPollingInterval
            // 
            this.numPollingInterval.Location = new System.Drawing.Point(150, 50);
            this.numPollingInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numPollingInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPollingInterval.Name = "numPollingInterval";
            this.numPollingInterval.Size = new System.Drawing.Size(60, 23);
            this.numPollingInterval.TabIndex = 3;
            this.numPollingInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblPollingInterval
            // 
            this.lblPollingInterval.AutoSize = true;
            this.lblPollingInterval.Location = new System.Drawing.Point(30, 52);
            this.lblPollingInterval.Name = "lblPollingInterval";
            this.lblPollingInterval.Size = new System.Drawing.Size(114, 15);
            this.lblPollingInterval.TabIndex = 2;
            this.lblPollingInterval.Text = "ポーリング間隔（秒）:";
            // 
            // radioPolling
            // 
            this.radioPolling.AutoSize = true;
            this.radioPolling.Location = new System.Drawing.Point(10, 50);
            this.radioPolling.Name = "radioPolling";
            this.radioPolling.Size = new System.Drawing.Size(14, 13);
            this.radioPolling.TabIndex = 1;
            this.radioPolling.UseVisualStyleBackColor = true;
            // 
            // radioImmediate
            // 
            this.radioImmediate.AutoSize = true;
            this.radioImmediate.Checked = true;
            this.radioImmediate.Location = new System.Drawing.Point(10, 22);
            this.radioImmediate.Name = "radioImmediate";
            this.radioImmediate.Size = new System.Drawing.Size(73, 19);
            this.radioImmediate.TabIndex = 0;
            this.radioImmediate.TabStop = true;
            this.radioImmediate.Text = "即座監視";
            this.radioImmediate.UseVisualStyleBackColor = true;
            // 
            // grpFileTypes
            // 
            this.grpFileTypes.Controls.Add(this.chkRAW);
            this.grpFileTypes.Controls.Add(this.chkJPEG);
            this.grpFileTypes.Location = new System.Drawing.Point(258, 158);
            this.grpFileTypes.Name = "grpFileTypes";
            this.grpFileTypes.Size = new System.Drawing.Size(120, 80);
            this.grpFileTypes.TabIndex = 2;
            this.grpFileTypes.TabStop = false;
            this.grpFileTypes.Text = "ファイル形式";
            // 
            // chkRAW
            // 
            this.chkRAW.AutoSize = true;
            this.chkRAW.Checked = true;
            this.chkRAW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRAW.Location = new System.Drawing.Point(10, 50);
            this.chkRAW.Name = "chkRAW";
            this.chkRAW.Size = new System.Drawing.Size(49, 19);
            this.chkRAW.TabIndex = 1;
            this.chkRAW.Text = "RAW";
            this.chkRAW.UseVisualStyleBackColor = true;
            // 
            // chkJPEG
            // 
            this.chkJPEG.AutoSize = true;
            this.chkJPEG.Checked = true;
            this.chkJPEG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJPEG.Location = new System.Drawing.Point(10, 22);
            this.chkJPEG.Name = "chkJPEG";
            this.chkJPEG.Size = new System.Drawing.Size(50, 19);
            this.chkJPEG.TabIndex = 0;
            this.chkJPEG.Text = "JPEG";
            this.chkJPEG.UseVisualStyleBackColor = true;
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.btnPreview);
            this.grpControls.Controls.Add(this.btnSaveSettings);
            this.grpControls.Controls.Add(this.btnStop);
            this.grpControls.Controls.Add(this.btnStart);
            this.grpControls.Location = new System.Drawing.Point(384, 158);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(128, 80);
            this.grpControls.TabIndex = 3;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "操作";
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(68, 50);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(50, 23);
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "プレビュー";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(68, 21);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(50, 23);
            this.btnSaveSettings.TabIndex = 2;
            this.btnSaveSettings.Text = "設定保存";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(10, 50);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(50, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(10, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(50, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Location = new System.Drawing.Point(12, 244);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(500, 180);
            this.grpLog.TabIndex = 4;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "ログ";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(10, 20);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(480, 150);
            this.txtLog.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 430);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(524, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(29, 17);
            this.lblStatus.Text = "準備完了";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 452);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpControls);
            this.Controls.Add(this.grpFileTypes);
            this.Controls.Add(this.grpMonitorMode);
            this.Controls.Add(this.grpSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "JPEG/RAW フォルダ監視";
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpMonitorMode.ResumeLayout(false);
            this.grpMonitorMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).EndInit();
            this.grpFileTypes.ResumeLayout(false);
            this.grpFileTypes.PerformLayout();
            this.grpControls.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox txtSourceFolder;
        private System.Windows.Forms.Label lblSourceFolder;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblDestinationFolder;
        private System.Windows.Forms.TextBox txtFilePrefix;
        private System.Windows.Forms.Label lblFilePrefix;
        private System.Windows.Forms.TextBox txtPreviewApp;
        private System.Windows.Forms.Label lblPreviewApp;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.Button btnBrowsePreview;
        private System.Windows.Forms.GroupBox grpMonitorMode;
        private System.Windows.Forms.RadioButton radioImmediate;
        private System.Windows.Forms.RadioButton radioPolling;
        private System.Windows.Forms.Label lblPollingInterval;
        private System.Windows.Forms.NumericUpDown numPollingInterval;
        private System.Windows.Forms.GroupBox grpFileTypes;
        private System.Windows.Forms.CheckBox chkJPEG;
        private System.Windows.Forms.CheckBox chkRAW;
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        private void btnBrowseSource_Click(object sender, System.EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "監視するフォルダを選択してください";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSourceFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnBrowseDestination_Click(object sender, System.EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "移動先フォルダを選択してください";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDestinationFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnBrowsePreview_Click(object sender, System.EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Title = "プレビューアプリケーションを選択してください";
            dialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPreviewApp.Text = dialog.FileName;
            }
        }
    }
}