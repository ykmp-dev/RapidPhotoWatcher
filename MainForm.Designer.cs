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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBasicSettings = new System.Windows.Forms.TabPage();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblDestinationFolder = new System.Windows.Forms.Label();
            this.txtSourceFolder = new System.Windows.Forms.TextBox();
            this.lblSourceFolder = new System.Windows.Forms.Label();
            this.grpFileTypes = new System.Windows.Forms.GroupBox();
            this.chkRAW = new System.Windows.Forms.CheckBox();
            this.chkJPEG = new System.Windows.Forms.CheckBox();
            this.grpMonitorMode = new System.Windows.Forms.GroupBox();
            this.numPollingInterval = new System.Windows.Forms.NumericUpDown();
            this.lblPollingInterval = new System.Windows.Forms.Label();
            this.radioPolling = new System.Windows.Forms.RadioButton();
            this.radioImmediate = new System.Windows.Forms.RadioButton();
            this.tabFileNaming = new System.Windows.Forms.TabPage();
            this.grpPrefixType = new System.Windows.Forms.GroupBox();
            this.radioPrefixDateTime = new System.Windows.Forms.RadioButton();
            this.radioPrefixCustom = new System.Windows.Forms.RadioButton();
            this.grpCustomPrefix = new System.Windows.Forms.GroupBox();
            this.txtFilePrefix = new System.Windows.Forms.TextBox();
            this.lblFilePrefix = new System.Windows.Forms.Label();
            this.grpDateTimeFormat = new System.Windows.Forms.GroupBox();
            this.cmbDateTimeFormat = new System.Windows.Forms.ComboBox();
            this.lblDateTimeFormat = new System.Windows.Forms.Label();
            this.grpSequencing = new System.Windows.Forms.GroupBox();
            this.numSequenceStart = new System.Windows.Forms.NumericUpDown();
            this.lblSequenceStart = new System.Windows.Forms.Label();
            this.numSequenceDigits = new System.Windows.Forms.NumericUpDown();
            this.lblSequenceDigits = new System.Windows.Forms.Label();
            this.btnResetSequence = new System.Windows.Forms.Button();
            this.tabExternalSoftware = new System.Windows.Forms.TabPage();
            this.btnBrowseExternal = new System.Windows.Forms.Button();
            this.txtExternalSoftware = new System.Windows.Forms.TextBox();
            this.lblExternalSoftware = new System.Windows.Forms.Label();
            this.grpSoftwareSettings = new System.Windows.Forms.GroupBox();
            this.chkAutoActivate = new System.Windows.Forms.CheckBox();
            this.cmbNavigationDirection = new System.Windows.Forms.ComboBox();
            this.lblNavigationDirection = new System.Windows.Forms.Label();
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            
            this.tabControl.SuspendLayout();
            this.tabBasicSettings.SuspendLayout();
            this.grpFileTypes.SuspendLayout();
            this.grpMonitorMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).BeginInit();
            this.tabFileNaming.SuspendLayout();
            this.grpPrefixType.SuspendLayout();
            this.grpCustomPrefix.SuspendLayout();
            this.grpDateTimeFormat.SuspendLayout();
            this.grpSequencing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceDigits)).BeginInit();
            this.tabExternalSoftware.SuspendLayout();
            this.grpSoftwareSettings.SuspendLayout();
            this.grpControls.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBasicSettings);
            this.tabControl.Controls.Add(this.tabFileNaming);
            this.tabControl.Controls.Add(this.tabExternalSoftware);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(580, 280);
            this.tabControl.TabIndex = 0;
            
            // 
            // tabBasicSettings
            // 
            this.tabBasicSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tabBasicSettings.Controls.Add(this.btnBrowseDestination);
            this.tabBasicSettings.Controls.Add(this.btnBrowseSource);
            this.tabBasicSettings.Controls.Add(this.txtDestinationFolder);
            this.tabBasicSettings.Controls.Add(this.lblDestinationFolder);
            this.tabBasicSettings.Controls.Add(this.txtSourceFolder);
            this.tabBasicSettings.Controls.Add(this.lblSourceFolder);
            this.tabBasicSettings.Controls.Add(this.grpFileTypes);
            this.tabBasicSettings.Controls.Add(this.grpMonitorMode);
            this.tabBasicSettings.Location = new System.Drawing.Point(4, 24);
            this.tabBasicSettings.Name = "tabBasicSettings";
            this.tabBasicSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabBasicSettings.Size = new System.Drawing.Size(572, 252);
            this.tabBasicSettings.TabIndex = 0;
            this.tabBasicSettings.Text = "基本設定";
            
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(520, 20);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.Location = new System.Drawing.Point(120, 20);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.Size = new System.Drawing.Size(394, 23);
            this.txtSourceFolder.TabIndex = 1;
            this.txtSourceFolder.TextChanged += new System.EventHandler(this.OnSettingsChanged);
            
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
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(520, 50);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDestination.TabIndex = 5;
            this.btnBrowseDestination.Text = "...";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(120, 50);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(394, 23);
            this.txtDestinationFolder.TabIndex = 4;
            this.txtDestinationFolder.TextChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // lblDestinationFolder
            // 
            this.lblDestinationFolder.AutoSize = true;
            this.lblDestinationFolder.Location = new System.Drawing.Point(10, 53);
            this.lblDestinationFolder.Name = "lblDestinationFolder";
            this.lblDestinationFolder.Size = new System.Drawing.Size(67, 15);
            this.lblDestinationFolder.TabIndex = 3;
            this.lblDestinationFolder.Text = "移動先フォルダ:";
            
            // 
            // grpMonitorMode
            // 
            this.grpMonitorMode.Controls.Add(this.numPollingInterval);
            this.grpMonitorMode.Controls.Add(this.lblPollingInterval);
            this.grpMonitorMode.Controls.Add(this.radioPolling);
            this.grpMonitorMode.Controls.Add(this.radioImmediate);
            this.grpMonitorMode.Location = new System.Drawing.Point(10, 90);
            this.grpMonitorMode.Name = "grpMonitorMode";
            this.grpMonitorMode.Size = new System.Drawing.Size(280, 80);
            this.grpMonitorMode.TabIndex = 6;
            this.grpMonitorMode.TabStop = false;
            this.grpMonitorMode.Text = "監視モード";
            
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
            this.radioImmediate.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // radioPolling
            // 
            this.radioPolling.AutoSize = true;
            this.radioPolling.Location = new System.Drawing.Point(10, 50);
            this.radioPolling.Name = "radioPolling";
            this.radioPolling.Size = new System.Drawing.Size(14, 13);
            this.radioPolling.TabIndex = 1;
            this.radioPolling.UseVisualStyleBackColor = true;
            this.radioPolling.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            
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
            // numPollingInterval
            // 
            this.numPollingInterval.Location = new System.Drawing.Point(150, 50);
            this.numPollingInterval.Maximum = new decimal(new int[] { 3600, 0, 0, 0});
            this.numPollingInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numPollingInterval.Name = "numPollingInterval";
            this.numPollingInterval.Size = new System.Drawing.Size(60, 23);
            this.numPollingInterval.TabIndex = 3;
            this.numPollingInterval.Value = new decimal(new int[] { 5, 0, 0, 0});
            this.numPollingInterval.ValueChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // grpFileTypes
            // 
            this.grpFileTypes.Controls.Add(this.chkRAW);
            this.grpFileTypes.Controls.Add(this.chkJPEG);
            this.grpFileTypes.Location = new System.Drawing.Point(300, 90);
            this.grpFileTypes.Name = "grpFileTypes";
            this.grpFileTypes.Size = new System.Drawing.Size(120, 80);
            this.grpFileTypes.TabIndex = 7;
            this.grpFileTypes.TabStop = false;
            this.grpFileTypes.Text = "ファイル形式";
            
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
            this.chkJPEG.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            
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
            this.chkRAW.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // tabFileNaming
            // 
            this.tabFileNaming.BackColor = System.Drawing.SystemColors.Control;
            this.tabFileNaming.Controls.Add(this.grpPrefixType);
            this.tabFileNaming.Controls.Add(this.grpCustomPrefix);
            this.tabFileNaming.Controls.Add(this.grpDateTimeFormat);
            this.tabFileNaming.Controls.Add(this.grpSequencing);
            this.tabFileNaming.Location = new System.Drawing.Point(4, 24);
            this.tabFileNaming.Name = "tabFileNaming";
            this.tabFileNaming.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileNaming.Size = new System.Drawing.Size(572, 252);
            this.tabFileNaming.TabIndex = 1;
            this.tabFileNaming.Text = "ファイル命名";
            
            // 
            // grpPrefixType
            // 
            this.grpPrefixType.Controls.Add(this.radioPrefixDateTime);
            this.grpPrefixType.Controls.Add(this.radioPrefixCustom);
            this.grpPrefixType.Location = new System.Drawing.Point(10, 10);
            this.grpPrefixType.Name = "grpPrefixType";
            this.grpPrefixType.Size = new System.Drawing.Size(200, 60);
            this.grpPrefixType.TabIndex = 0;
            this.grpPrefixType.TabStop = false;
            this.grpPrefixType.Text = "プレフィックスタイプ";
            
            // 
            // radioPrefixCustom
            // 
            this.radioPrefixCustom.AutoSize = true;
            this.radioPrefixCustom.Checked = true;
            this.radioPrefixCustom.Location = new System.Drawing.Point(10, 20);
            this.radioPrefixCustom.Name = "radioPrefixCustom";
            this.radioPrefixCustom.Size = new System.Drawing.Size(91, 19);
            this.radioPrefixCustom.TabIndex = 0;
            this.radioPrefixCustom.TabStop = true;
            this.radioPrefixCustom.Text = "カスタムテキスト";
            this.radioPrefixCustom.UseVisualStyleBackColor = true;
            this.radioPrefixCustom.CheckedChanged += new System.EventHandler(this.OnPrefixTypeChanged);
            
            // 
            // radioPrefixDateTime
            // 
            this.radioPrefixDateTime.AutoSize = true;
            this.radioPrefixDateTime.Location = new System.Drawing.Point(120, 20);
            this.radioPrefixDateTime.Name = "radioPrefixDateTime";
            this.radioPrefixDateTime.Size = new System.Drawing.Size(61, 19);
            this.radioPrefixDateTime.TabIndex = 1;
            this.radioPrefixDateTime.Text = "撮影日時";
            this.radioPrefixDateTime.UseVisualStyleBackColor = true;
            this.radioPrefixDateTime.CheckedChanged += new System.EventHandler(this.OnPrefixTypeChanged);
            
            // 
            // grpCustomPrefix
            // 
            this.grpCustomPrefix.Controls.Add(this.txtFilePrefix);
            this.grpCustomPrefix.Controls.Add(this.lblFilePrefix);
            this.grpCustomPrefix.Location = new System.Drawing.Point(220, 10);
            this.grpCustomPrefix.Name = "grpCustomPrefix";
            this.grpCustomPrefix.Size = new System.Drawing.Size(240, 60);
            this.grpCustomPrefix.TabIndex = 1;
            this.grpCustomPrefix.TabStop = false;
            this.grpCustomPrefix.Text = "カスタムプレフィックス";
            
            // 
            // lblFilePrefix
            // 
            this.lblFilePrefix.AutoSize = true;
            this.lblFilePrefix.Location = new System.Drawing.Point(10, 23);
            this.lblFilePrefix.Name = "lblFilePrefix";
            this.lblFilePrefix.Size = new System.Drawing.Size(45, 15);
            this.lblFilePrefix.TabIndex = 0;
            this.lblFilePrefix.Text = "接頭辞:";
            
            // 
            // txtFilePrefix
            // 
            this.txtFilePrefix.Location = new System.Drawing.Point(70, 20);
            this.txtFilePrefix.Name = "txtFilePrefix";
            this.txtFilePrefix.Size = new System.Drawing.Size(160, 23);
            this.txtFilePrefix.TabIndex = 1;
            this.txtFilePrefix.Text = "IMG_";
            this.txtFilePrefix.TextChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // grpDateTimeFormat
            // 
            this.grpDateTimeFormat.Controls.Add(this.cmbDateTimeFormat);
            this.grpDateTimeFormat.Controls.Add(this.lblDateTimeFormat);
            this.grpDateTimeFormat.Enabled = false;
            this.grpDateTimeFormat.Location = new System.Drawing.Point(10, 80);
            this.grpDateTimeFormat.Name = "grpDateTimeFormat";
            this.grpDateTimeFormat.Size = new System.Drawing.Size(450, 60);
            this.grpDateTimeFormat.TabIndex = 2;
            this.grpDateTimeFormat.TabStop = false;
            this.grpDateTimeFormat.Text = "日時フォーマット";
            
            // 
            // lblDateTimeFormat
            // 
            this.lblDateTimeFormat.AutoSize = true;
            this.lblDateTimeFormat.Location = new System.Drawing.Point(10, 23);
            this.lblDateTimeFormat.Name = "lblDateTimeFormat";
            this.lblDateTimeFormat.Size = new System.Drawing.Size(67, 15);
            this.lblDateTimeFormat.TabIndex = 0;
            this.lblDateTimeFormat.Text = "フォーマット:";
            
            // 
            // cmbDateTimeFormat
            // 
            this.cmbDateTimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateTimeFormat.FormattingEnabled = true;
            this.cmbDateTimeFormat.Items.AddRange(new object[] {
            "20241127 (YYYYMMDD)",
            "2024_11_27 (YYYY_MM_DD)",
            "2024-11-27 (YYYY-MM-DD)",
            "20241127_143052 (YYYYMMDD_HHMMSS)",
            "2024_11_27_14_30_52 (YYYY_MM_DD_HH_MM_SS)"});
            this.cmbDateTimeFormat.Location = new System.Drawing.Point(90, 20);
            this.cmbDateTimeFormat.Name = "cmbDateTimeFormat";
            this.cmbDateTimeFormat.Size = new System.Drawing.Size(350, 23);
            this.cmbDateTimeFormat.TabIndex = 1;
            this.cmbDateTimeFormat.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // grpSequencing
            // 
            this.grpSequencing.Controls.Add(this.btnResetSequence);
            this.grpSequencing.Controls.Add(this.numSequenceStart);
            this.grpSequencing.Controls.Add(this.lblSequenceStart);
            this.grpSequencing.Controls.Add(this.numSequenceDigits);
            this.grpSequencing.Controls.Add(this.lblSequenceDigits);
            this.grpSequencing.Location = new System.Drawing.Point(10, 150);
            this.grpSequencing.Name = "grpSequencing";
            this.grpSequencing.Size = new System.Drawing.Size(450, 90);
            this.grpSequencing.TabIndex = 3;
            this.grpSequencing.TabStop = false;
            this.grpSequencing.Text = "連番設定";
            
            // 
            // lblSequenceDigits
            // 
            this.lblSequenceDigits.AutoSize = true;
            this.lblSequenceDigits.Location = new System.Drawing.Point(10, 23);
            this.lblSequenceDigits.Name = "lblSequenceDigits";
            this.lblSequenceDigits.Size = new System.Drawing.Size(43, 15);
            this.lblSequenceDigits.TabIndex = 0;
            this.lblSequenceDigits.Text = "桁数:";
            
            // 
            // numSequenceDigits
            // 
            this.numSequenceDigits.Location = new System.Drawing.Point(70, 20);
            this.numSequenceDigits.Maximum = new decimal(new int[] { 10, 0, 0, 0});
            this.numSequenceDigits.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numSequenceDigits.Name = "numSequenceDigits";
            this.numSequenceDigits.Size = new System.Drawing.Size(60, 23);
            this.numSequenceDigits.TabIndex = 1;
            this.numSequenceDigits.Value = new decimal(new int[] { 4, 0, 0, 0});
            this.numSequenceDigits.ValueChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // lblSequenceStart
            // 
            this.lblSequenceStart.AutoSize = true;
            this.lblSequenceStart.Location = new System.Drawing.Point(160, 23);
            this.lblSequenceStart.Name = "lblSequenceStart";
            this.lblSequenceStart.Size = new System.Drawing.Size(55, 15);
            this.lblSequenceStart.TabIndex = 2;
            this.lblSequenceStart.Text = "開始番号:";
            
            // 
            // numSequenceStart
            // 
            this.numSequenceStart.Location = new System.Drawing.Point(230, 20);
            this.numSequenceStart.Maximum = new decimal(new int[] { 9999999, 0, 0, 0});
            this.numSequenceStart.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numSequenceStart.Name = "numSequenceStart";
            this.numSequenceStart.Size = new System.Drawing.Size(80, 23);
            this.numSequenceStart.TabIndex = 3;
            this.numSequenceStart.Value = new decimal(new int[] { 1, 0, 0, 0});
            this.numSequenceStart.ValueChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // btnResetSequence
            // 
            this.btnResetSequence.Location = new System.Drawing.Point(10, 50);
            this.btnResetSequence.Name = "btnResetSequence";
            this.btnResetSequence.Size = new System.Drawing.Size(100, 30);
            this.btnResetSequence.TabIndex = 4;
            this.btnResetSequence.Text = "連番リセット";
            this.btnResetSequence.UseVisualStyleBackColor = true;
            this.btnResetSequence.Click += new System.EventHandler(this.btnResetSequence_Click);
            
            // 
            // tabExternalSoftware
            // 
            this.tabExternalSoftware.BackColor = System.Drawing.SystemColors.Control;
            this.tabExternalSoftware.Controls.Add(this.grpSoftwareSettings);
            this.tabExternalSoftware.Controls.Add(this.btnBrowseExternal);
            this.tabExternalSoftware.Controls.Add(this.txtExternalSoftware);
            this.tabExternalSoftware.Controls.Add(this.lblExternalSoftware);
            this.tabExternalSoftware.Location = new System.Drawing.Point(4, 24);
            this.tabExternalSoftware.Name = "tabExternalSoftware";
            this.tabExternalSoftware.Padding = new System.Windows.Forms.Padding(3);
            this.tabExternalSoftware.Size = new System.Drawing.Size(572, 252);
            this.tabExternalSoftware.TabIndex = 2;
            this.tabExternalSoftware.Text = "連携ソフトウェア";
            
            // 
            // lblExternalSoftware
            // 
            this.lblExternalSoftware.AutoSize = true;
            this.lblExternalSoftware.Location = new System.Drawing.Point(10, 23);
            this.lblExternalSoftware.Name = "lblExternalSoftware";
            this.lblExternalSoftware.Size = new System.Drawing.Size(103, 15);
            this.lblExternalSoftware.TabIndex = 0;
            this.lblExternalSoftware.Text = "連携ソフトウェアパス:";
            
            // 
            // txtExternalSoftware
            // 
            this.txtExternalSoftware.Location = new System.Drawing.Point(120, 20);
            this.txtExternalSoftware.Name = "txtExternalSoftware";
            this.txtExternalSoftware.Size = new System.Drawing.Size(394, 23);
            this.txtExternalSoftware.TabIndex = 1;
            this.txtExternalSoftware.TextChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // btnBrowseExternal
            // 
            this.btnBrowseExternal.Location = new System.Drawing.Point(520, 20);
            this.btnBrowseExternal.Name = "btnBrowseExternal";
            this.btnBrowseExternal.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseExternal.TabIndex = 2;
            this.btnBrowseExternal.Text = "...";
            this.btnBrowseExternal.UseVisualStyleBackColor = true;
            this.btnBrowseExternal.Click += new System.EventHandler(this.btnBrowseExternal_Click);
            
            
            // 
            // grpSoftwareSettings
            // 
            this.grpSoftwareSettings.Controls.Add(this.cmbNavigationDirection);
            this.grpSoftwareSettings.Controls.Add(this.lblNavigationDirection);
            this.grpSoftwareSettings.Controls.Add(this.chkAutoActivate);
            this.grpSoftwareSettings.Location = new System.Drawing.Point(10, 100);
            this.grpSoftwareSettings.Name = "grpSoftwareSettings";
            this.grpSoftwareSettings.Size = new System.Drawing.Size(540, 100);
            this.grpSoftwareSettings.TabIndex = 4;
            this.grpSoftwareSettings.TabStop = false;
            this.grpSoftwareSettings.Text = "連携ソフトウェア設定";
            
            // 
            // chkAutoActivate
            // 
            this.chkAutoActivate.AutoSize = true;
            this.chkAutoActivate.Checked = true;
            this.chkAutoActivate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoActivate.Location = new System.Drawing.Point(10, 25);
            this.chkAutoActivate.Name = "chkAutoActivate";
            this.chkAutoActivate.Size = new System.Drawing.Size(200, 19);
            this.chkAutoActivate.TabIndex = 0;
            this.chkAutoActivate.Text = "撮影時に連携ソフトウェアをアクティブ化";
            this.chkAutoActivate.UseVisualStyleBackColor = true;
            this.chkAutoActivate.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // lblNavigationDirection
            // 
            this.lblNavigationDirection.AutoSize = true;
            this.lblNavigationDirection.Location = new System.Drawing.Point(10, 55);
            this.lblNavigationDirection.Name = "lblNavigationDirection";
            this.lblNavigationDirection.Size = new System.Drawing.Size(91, 15);
            this.lblNavigationDirection.TabIndex = 1;
            this.lblNavigationDirection.Text = "画像位置移動方式:";
            
            // 
            // cmbNavigationDirection
            // 
            this.cmbNavigationDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNavigationDirection.FormattingEnabled = true;
            this.cmbNavigationDirection.Items.AddRange(new object[] {
            "Endキーで最下部へ移動",
            "Homeキーで最上部へ移動"});
            this.cmbNavigationDirection.Location = new System.Drawing.Point(120, 52);
            this.cmbNavigationDirection.Name = "cmbNavigationDirection";
            this.cmbNavigationDirection.Size = new System.Drawing.Size(250, 23);
            this.cmbNavigationDirection.TabIndex = 2;
            this.cmbNavigationDirection.SelectedIndex = 0;
            this.cmbNavigationDirection.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.btnExit);
            this.grpControls.Controls.Add(this.btnStop);
            this.grpControls.Controls.Add(this.btnStart);
            this.grpControls.Location = new System.Drawing.Point(12, 300);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(250, 60);
            this.grpControls.TabIndex = 1;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "操作";
            
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(10, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(90, 21);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 30);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(170, 21);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 30);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "終了";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Location = new System.Drawing.Point(270, 300);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(320, 120);
            this.grpLog.TabIndex = 2;
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
            this.txtLog.Size = new System.Drawing.Size(300, 90);
            this.txtLog.TabIndex = 0;
            
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 430);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(604, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "準備完了";
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 452);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpControls);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Name = "MainForm";
            this.Text = "JPEG/RAW フォルダ監視";
            
            this.tabControl.ResumeLayout(false);
            this.tabBasicSettings.ResumeLayout(false);
            this.tabBasicSettings.PerformLayout();
            this.grpFileTypes.ResumeLayout(false);
            this.grpFileTypes.PerformLayout();
            this.grpMonitorMode.ResumeLayout(false);
            this.grpMonitorMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).EndInit();
            this.tabFileNaming.ResumeLayout(false);
            this.grpPrefixType.ResumeLayout(false);
            this.grpPrefixType.PerformLayout();
            this.grpCustomPrefix.ResumeLayout(false);
            this.grpCustomPrefix.PerformLayout();
            this.grpDateTimeFormat.ResumeLayout(false);
            this.grpDateTimeFormat.PerformLayout();
            this.grpSequencing.ResumeLayout(false);
            this.grpSequencing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceDigits)).EndInit();
            this.tabExternalSoftware.ResumeLayout(false);
            this.tabExternalSoftware.PerformLayout();
            this.grpSoftwareSettings.ResumeLayout(false);
            this.grpSoftwareSettings.PerformLayout();
            this.grpControls.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBasicSettings;
        private System.Windows.Forms.TabPage tabFileNaming;
        private System.Windows.Forms.TabPage tabExternalSoftware;
        
        // Basic Settings Tab
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblDestinationFolder;
        private System.Windows.Forms.TextBox txtSourceFolder;
        private System.Windows.Forms.Label lblSourceFolder;
        private System.Windows.Forms.GroupBox grpFileTypes;
        private System.Windows.Forms.CheckBox chkRAW;
        private System.Windows.Forms.CheckBox chkJPEG;
        private System.Windows.Forms.GroupBox grpMonitorMode;
        private System.Windows.Forms.NumericUpDown numPollingInterval;
        private System.Windows.Forms.Label lblPollingInterval;
        private System.Windows.Forms.RadioButton radioPolling;
        private System.Windows.Forms.RadioButton radioImmediate;
        
        // File Naming Tab
        private System.Windows.Forms.GroupBox grpPrefixType;
        private System.Windows.Forms.RadioButton radioPrefixDateTime;
        private System.Windows.Forms.RadioButton radioPrefixCustom;
        private System.Windows.Forms.GroupBox grpCustomPrefix;
        private System.Windows.Forms.TextBox txtFilePrefix;
        private System.Windows.Forms.Label lblFilePrefix;
        private System.Windows.Forms.GroupBox grpDateTimeFormat;
        private System.Windows.Forms.ComboBox cmbDateTimeFormat;
        private System.Windows.Forms.Label lblDateTimeFormat;
        private System.Windows.Forms.GroupBox grpSequencing;
        private System.Windows.Forms.NumericUpDown numSequenceStart;
        private System.Windows.Forms.Label lblSequenceStart;
        private System.Windows.Forms.NumericUpDown numSequenceDigits;
        private System.Windows.Forms.Label lblSequenceDigits;
        private System.Windows.Forms.Button btnResetSequence;
        
        // External Software Tab
        private System.Windows.Forms.Button btnBrowseExternal;
        private System.Windows.Forms.TextBox txtExternalSoftware;
        private System.Windows.Forms.Label lblExternalSoftware;
        private System.Windows.Forms.GroupBox grpSoftwareSettings;
        private System.Windows.Forms.CheckBox chkAutoActivate;
        private System.Windows.Forms.ComboBox cmbNavigationDirection;
        private System.Windows.Forms.Label lblNavigationDirection;
        
        // Control and Log areas
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        // Legacy controls for backward compatibility
        private System.Windows.Forms.TextBox txtPreviewApp => txtExternalSoftware;
        private System.Windows.Forms.Label lblPreviewApp => lblExternalSoftware;
        private System.Windows.Forms.Button btnBrowsePreview => btnBrowseExternal;
    }
}