namespace RapidPhotoWatcher
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
            this.grpSeparator = new System.Windows.Forms.GroupBox();
            this.cmbSeparatorType = new System.Windows.Forms.ComboBox();
            this.lblSeparatorType = new System.Windows.Forms.Label();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblFileNamePreview = new System.Windows.Forms.Label();
            this.lblPreviewCaption = new System.Windows.Forms.Label();
            this.grpSequencing = new System.Windows.Forms.GroupBox();
            this.numSequenceStart = new System.Windows.Forms.NumericUpDown();
            this.lblSequenceStart = new System.Windows.Forms.Label();
            this.numSequenceDigits = new System.Windows.Forms.NumericUpDown();
            this.lblSequenceDigits = new System.Windows.Forms.Label();
            this.btnResetSequence = new System.Windows.Forms.Button();
            this.tabExternalSoftware = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.grpBasicControls = new System.Windows.Forms.GroupBox();
            this.btnBasicExit = new System.Windows.Forms.Button();
            this.btnBasicStop = new System.Windows.Forms.Button();
            this.btnBasicStart = new System.Windows.Forms.Button();
            this.grpExternalControls = new System.Windows.Forms.GroupBox();
            this.btnExternalExit = new System.Windows.Forms.Button();
            this.btnExternalStop = new System.Windows.Forms.Button();
            this.btnExternalStart = new System.Windows.Forms.Button();
            this.btnBrowseExternal = new System.Windows.Forms.Button();
            this.txtExternalSoftware = new System.Windows.Forms.TextBox();
            this.lblExternalSoftware = new System.Windows.Forms.Label();
            this.grpSoftwareSettings = new System.Windows.Forms.GroupBox();
            this.chkAutoActivate = new System.Windows.Forms.CheckBox();
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
            this.grpBasicControls.SuspendLayout();
            this.grpExternalControls.SuspendLayout();
            this.grpControls.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBasicSettings);
            this.tabControl.Controls.Add(this.tabFileNaming);
            this.tabControl.Controls.Add(this.tabExternalSoftware);
            this.tabControl.Controls.Add(this.tabLog);
            this.tabControl.Location = new System.Drawing.Point(15, 15);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(690, 400);
            this.tabControl.TabIndex = 0;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl.ItemSize = new System.Drawing.Size(150, 30);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            
            // 
            // tabBasicSettings
            // 
            this.tabBasicSettings.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabBasicSettings.Controls.Add(this.btnBrowseDestination);
            this.tabBasicSettings.Controls.Add(this.btnBrowseSource);
            this.tabBasicSettings.Controls.Add(this.txtDestinationFolder);
            this.tabBasicSettings.Controls.Add(this.lblDestinationFolder);
            this.tabBasicSettings.Controls.Add(this.txtSourceFolder);
            this.tabBasicSettings.Controls.Add(this.lblSourceFolder);
            this.tabBasicSettings.Controls.Add(this.grpFileTypes);
            this.tabBasicSettings.Controls.Add(this.grpMonitorMode);
            this.tabBasicSettings.Controls.Add(this.grpBasicControls);
            this.tabBasicSettings.Location = new System.Drawing.Point(4, 24);
            this.tabBasicSettings.Name = "tabBasicSettings";
            this.tabBasicSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabBasicSettings.Size = new System.Drawing.Size(672, 340);
            this.tabBasicSettings.TabIndex = 0;
            this.tabBasicSettings.Text = "⚙️ 基本設定";
            
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(520, 20);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(35, 25);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "📁";
            this.btnBrowseSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseSource.FlatAppearance.BorderSize = 1;
            this.btnBrowseSource.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 149, 237);
            this.btnBrowseSource.BackColor = System.Drawing.Color.FromArgb(245, 248, 255);
            this.btnBrowseSource.ForeColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnBrowseSource.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBrowseSource.UseVisualStyleBackColor = false;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.Location = new System.Drawing.Point(120, 20);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.Size = new System.Drawing.Size(394, 27);
            this.txtSourceFolder.TabIndex = 1;
            this.txtSourceFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourceFolder.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.txtSourceFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.btnBrowseDestination.Size = new System.Drawing.Size(35, 25);
            this.btnBrowseDestination.TabIndex = 5;
            this.btnBrowseDestination.Text = "📁";
            this.btnBrowseDestination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseDestination.FlatAppearance.BorderSize = 1;
            this.btnBrowseDestination.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 149, 237);
            this.btnBrowseDestination.BackColor = System.Drawing.Color.FromArgb(245, 248, 255);
            this.btnBrowseDestination.ForeColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnBrowseDestination.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBrowseDestination.UseVisualStyleBackColor = false;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(120, 50);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(394, 27);
            this.txtDestinationFolder.TabIndex = 4;
            this.txtDestinationFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestinationFolder.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.txtDestinationFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.tabFileNaming.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabFileNaming.Controls.Add(this.grpPrefixType);
            this.tabFileNaming.Controls.Add(this.grpCustomPrefix);
            this.tabFileNaming.Controls.Add(this.grpDateTimeFormat);
            this.tabFileNaming.Controls.Add(this.grpSeparator);
            this.tabFileNaming.Controls.Add(this.grpSequencing);
            this.tabFileNaming.Controls.Add(this.grpPreview);
            this.tabFileNaming.Controls.Add(this.grpControls);
            this.tabFileNaming.Location = new System.Drawing.Point(4, 24);
            this.tabFileNaming.Name = "tabFileNaming";
            this.tabFileNaming.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileNaming.Size = new System.Drawing.Size(672, 340);
            this.tabFileNaming.TabIndex = 1;
            this.tabFileNaming.Text = "📝 ファイル命名";
            
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
            this.grpCustomPrefix.Size = new System.Drawing.Size(340, 60);
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
            this.grpDateTimeFormat.Size = new System.Drawing.Size(420, 60);
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
            this.cmbDateTimeFormat.Size = new System.Drawing.Size(320, 23);
            this.cmbDateTimeFormat.TabIndex = 1;
            this.cmbDateTimeFormat.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            
            // 
            // grpSeparator
            // 
            this.grpSeparator.Controls.Add(this.cmbSeparatorType);
            this.grpSeparator.Controls.Add(this.lblSeparatorType);
            this.grpSeparator.Location = new System.Drawing.Point(440, 80);
            this.grpSeparator.Name = "grpSeparator";
            this.grpSeparator.Size = new System.Drawing.Size(180, 60);
            this.grpSeparator.TabIndex = 3;
            this.grpSeparator.TabStop = false;
            this.grpSeparator.Text = "区切り文字";
            
            // 
            // lblSeparatorType
            // 
            this.lblSeparatorType.AutoSize = true;
            this.lblSeparatorType.Location = new System.Drawing.Point(6, 25);
            this.lblSeparatorType.Name = "lblSeparatorType";
            this.lblSeparatorType.Size = new System.Drawing.Size(31, 15);
            this.lblSeparatorType.TabIndex = 0;
            this.lblSeparatorType.Text = "種類:";
            
            // 
            // cmbSeparatorType
            // 
            this.cmbSeparatorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSeparatorType.FormattingEnabled = true;
            this.cmbSeparatorType.Items.AddRange(new object[] {
            "なし",
            "_(アンダーバー)",
            "-(ハイフン)"});
            this.cmbSeparatorType.Location = new System.Drawing.Point(45, 22);
            this.cmbSeparatorType.Name = "cmbSeparatorType";
            this.cmbSeparatorType.Size = new System.Drawing.Size(120, 23);
            this.cmbSeparatorType.TabIndex = 1;
            this.cmbSeparatorType.SelectedIndex = 0;
            this.cmbSeparatorType.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            
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
            this.grpSequencing.Size = new System.Drawing.Size(650, 50);
            this.grpSequencing.TabIndex = 4;
            this.grpSequencing.TabStop = false;
            this.grpSequencing.Text = "連番設定";
            
            // 
            // lblSequenceDigits
            // 
            this.lblSequenceDigits.AutoSize = true;
            this.lblSequenceDigits.Location = new System.Drawing.Point(10, 22);
            this.lblSequenceDigits.Name = "lblSequenceDigits";
            this.lblSequenceDigits.Size = new System.Drawing.Size(43, 15);
            this.lblSequenceDigits.TabIndex = 0;
            this.lblSequenceDigits.Text = "桁数:";
            
            // 
            // numSequenceDigits
            // 
            this.numSequenceDigits.Location = new System.Drawing.Point(60, 20);
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
            this.lblSequenceStart.Location = new System.Drawing.Point(150, 22);
            this.lblSequenceStart.Name = "lblSequenceStart";
            this.lblSequenceStart.Size = new System.Drawing.Size(55, 15);
            this.lblSequenceStart.TabIndex = 2;
            this.lblSequenceStart.Text = "開始番号:";
            
            // 
            // numSequenceStart
            // 
            this.numSequenceStart.Location = new System.Drawing.Point(220, 20);
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
            this.btnResetSequence.Location = new System.Drawing.Point(320, 20);
            this.btnResetSequence.Name = "btnResetSequence";
            this.btnResetSequence.Size = new System.Drawing.Size(100, 25);
            this.btnResetSequence.TabIndex = 4;
            this.btnResetSequence.Text = "🔄 リセット";
            this.btnResetSequence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSequence.FlatAppearance.BorderSize = 1;
            this.btnResetSequence.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 85, 247);
            this.btnResetSequence.BackColor = System.Drawing.Color.FromArgb(250, 245, 255);
            this.btnResetSequence.ForeColor = System.Drawing.Color.FromArgb(147, 51, 234);
            this.btnResetSequence.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnResetSequence.UseVisualStyleBackColor = false;
            this.btnResetSequence.Click += new System.EventHandler(this.btnResetSequence_Click);
            
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.lblFileNamePreview);
            this.grpPreview.Controls.Add(this.lblPreviewCaption);
            this.grpPreview.Location = new System.Drawing.Point(10, 210);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(650, 50);
            this.grpPreview.TabIndex = 5;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "プレビュー";
            
            // 
            // lblPreviewCaption
            // 
            this.lblPreviewCaption.AutoSize = true;
            this.lblPreviewCaption.Location = new System.Drawing.Point(10, 20);
            this.lblPreviewCaption.Name = "lblPreviewCaption";
            this.lblPreviewCaption.Size = new System.Drawing.Size(67, 15);
            this.lblPreviewCaption.TabIndex = 0;
            this.lblPreviewCaption.Text = "ファイル名例:";
            
            // 
            // lblFileNamePreview
            // 
            this.lblFileNamePreview.AutoSize = true;
            this.lblFileNamePreview.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFileNamePreview.ForeColor = System.Drawing.Color.Blue;
            this.lblFileNamePreview.Location = new System.Drawing.Point(100, 20);
            this.lblFileNamePreview.Name = "lblFileNamePreview";
            this.lblFileNamePreview.Size = new System.Drawing.Size(108, 19);
            this.lblFileNamePreview.TabIndex = 1;
            this.lblFileNamePreview.Text = "IMG_0001.jpg";
            
            // 
            // tabExternalSoftware
            // 
            this.tabExternalSoftware.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabExternalSoftware.Controls.Add(this.grpSoftwareSettings);
            this.tabExternalSoftware.Controls.Add(this.btnBrowseExternal);
            this.tabExternalSoftware.Controls.Add(this.txtExternalSoftware);
            this.tabExternalSoftware.Controls.Add(this.lblExternalSoftware);
            this.tabExternalSoftware.Controls.Add(this.grpExternalControls);
            this.tabExternalSoftware.Location = new System.Drawing.Point(4, 24);
            this.tabExternalSoftware.Name = "tabExternalSoftware";
            this.tabExternalSoftware.Padding = new System.Windows.Forms.Padding(3);
            this.tabExternalSoftware.Size = new System.Drawing.Size(672, 340);
            this.tabExternalSoftware.TabIndex = 2;
            this.tabExternalSoftware.Text = "🔗 連携ソフト";
            
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
            this.btnBrowseExternal.Size = new System.Drawing.Size(35, 25);
            this.btnBrowseExternal.TabIndex = 2;
            this.btnBrowseExternal.Text = "📁";
            this.btnBrowseExternal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseExternal.FlatAppearance.BorderSize = 1;
            this.btnBrowseExternal.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 149, 237);
            this.btnBrowseExternal.BackColor = System.Drawing.Color.FromArgb(245, 248, 255);
            this.btnBrowseExternal.ForeColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnBrowseExternal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBrowseExternal.UseVisualStyleBackColor = false;
            this.btnBrowseExternal.Click += new System.EventHandler(this.btnBrowseExternal_Click);
            
            
            // 
            // grpSoftwareSettings
            // 
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
            // tabLog
            // 
            this.tabLog.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabLog.Controls.Add(this.grpLog);
            this.tabLog.Location = new System.Drawing.Point(4, 24);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(672, 340);
            this.tabLog.TabIndex = 3;
            this.tabLog.Text = "📄 ログ";
            
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.btnExit);
            this.grpControls.Controls.Add(this.btnStop);
            this.grpControls.Controls.Add(this.btnStart);
            this.grpControls.Location = new System.Drawing.Point(10, 270);
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
            this.grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLog.Location = new System.Drawing.Point(3, 3);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(666, 334);
            this.grpLog.TabIndex = 0;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "ログ";
            
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtLog.Location = new System.Drawing.Point(3, 19);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(660, 312);
            this.txtLog.TabIndex = 0;
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(720, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 17);
            this.lblStatus.Text = "✅ 準備完了";
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(720, 450);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.ControlBox = true;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Name = "MainForm";
            this.Text = "📷 RapidPhotoWatcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
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
            this.tabLog.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.grpBasicControls.ResumeLayout(false);
            this.grpExternalControls.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            
            // 
            // grpBasicControls
            // 
            this.grpBasicControls.Controls.Add(this.btnBasicExit);
            this.grpBasicControls.Controls.Add(this.btnBasicStop);
            this.grpBasicControls.Controls.Add(this.btnBasicStart);
            this.grpBasicControls.Location = new System.Drawing.Point(10, 270);
            this.grpBasicControls.Name = "grpBasicControls";
            this.grpBasicControls.Size = new System.Drawing.Size(250, 60);
            this.grpBasicControls.TabIndex = 8;
            this.grpBasicControls.TabStop = false;
            this.grpBasicControls.Text = "操作";
            
            // 
            // btnBasicStart
            // 
            this.btnBasicStart.Location = new System.Drawing.Point(10, 21);
            this.btnBasicStart.Name = "btnBasicStart";
            this.btnBasicStart.Size = new System.Drawing.Size(70, 35);
            this.btnBasicStart.TabIndex = 0;
            this.btnBasicStart.Text = "▶ 開始";
            this.btnBasicStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBasicStart.FlatAppearance.BorderSize = 0;
            this.btnBasicStart.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnBasicStart.ForeColor = System.Drawing.Color.White;
            this.btnBasicStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBasicStart.UseVisualStyleBackColor = false;
            this.btnBasicStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // 
            // btnBasicStop
            // 
            this.btnBasicStop.Enabled = false;
            this.btnBasicStop.Location = new System.Drawing.Point(90, 21);
            this.btnBasicStop.Name = "btnBasicStop";
            this.btnBasicStop.Size = new System.Drawing.Size(70, 35);
            this.btnBasicStop.TabIndex = 1;
            this.btnBasicStop.Text = "⏸ 停止";
            this.btnBasicStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBasicStop.FlatAppearance.BorderSize = 0;
            this.btnBasicStop.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnBasicStop.ForeColor = System.Drawing.Color.White;
            this.btnBasicStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBasicStop.UseVisualStyleBackColor = false;
            this.btnBasicStop.Click += new System.EventHandler(this.btnStop_Click);
            
            // 
            // btnBasicExit
            // 
            this.btnBasicExit.Location = new System.Drawing.Point(170, 21);
            this.btnBasicExit.Name = "btnBasicExit";
            this.btnBasicExit.Size = new System.Drawing.Size(70, 35);
            this.btnBasicExit.TabIndex = 2;
            this.btnBasicExit.Text = "✕ 終了";
            this.btnBasicExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBasicExit.FlatAppearance.BorderSize = 1;
            this.btnBasicExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.btnBasicExit.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.btnBasicExit.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnBasicExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBasicExit.UseVisualStyleBackColor = false;
            this.btnBasicExit.Click += new System.EventHandler(this.btnExit_Click);
            
            // 
            // grpExternalControls
            // 
            this.grpExternalControls.Controls.Add(this.btnExternalExit);
            this.grpExternalControls.Controls.Add(this.btnExternalStop);
            this.grpExternalControls.Controls.Add(this.btnExternalStart);
            this.grpExternalControls.Location = new System.Drawing.Point(10, 270);
            this.grpExternalControls.Name = "grpExternalControls";
            this.grpExternalControls.Size = new System.Drawing.Size(250, 60);
            this.grpExternalControls.TabIndex = 5;
            this.grpExternalControls.TabStop = false;
            this.grpExternalControls.Text = "操作";
            
            // 
            // btnExternalStart
            // 
            this.btnExternalStart.Location = new System.Drawing.Point(10, 21);
            this.btnExternalStart.Name = "btnExternalStart";
            this.btnExternalStart.Size = new System.Drawing.Size(70, 35);
            this.btnExternalStart.TabIndex = 0;
            this.btnExternalStart.Text = "▶ 開始";
            this.btnExternalStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExternalStart.FlatAppearance.BorderSize = 0;
            this.btnExternalStart.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnExternalStart.ForeColor = System.Drawing.Color.White;
            this.btnExternalStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExternalStart.UseVisualStyleBackColor = false;
            this.btnExternalStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // 
            // btnExternalStop
            // 
            this.btnExternalStop.Enabled = false;
            this.btnExternalStop.Location = new System.Drawing.Point(90, 21);
            this.btnExternalStop.Name = "btnExternalStop";
            this.btnExternalStop.Size = new System.Drawing.Size(70, 35);
            this.btnExternalStop.TabIndex = 1;
            this.btnExternalStop.Text = "⏸ 停止";
            this.btnExternalStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExternalStop.FlatAppearance.BorderSize = 0;
            this.btnExternalStop.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnExternalStop.ForeColor = System.Drawing.Color.White;
            this.btnExternalStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExternalStop.UseVisualStyleBackColor = false;
            this.btnExternalStop.Click += new System.EventHandler(this.btnStop_Click);
            
            // 
            // btnExternalExit
            // 
            this.btnExternalExit.Location = new System.Drawing.Point(170, 21);
            this.btnExternalExit.Name = "btnExternalExit";
            this.btnExternalExit.Size = new System.Drawing.Size(70, 35);
            this.btnExternalExit.TabIndex = 2;
            this.btnExternalExit.Text = "✕ 終了";
            this.btnExternalExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExternalExit.FlatAppearance.BorderSize = 1;
            this.btnExternalExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.btnExternalExit.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.btnExternalExit.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnExternalExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExternalExit.UseVisualStyleBackColor = false;
            this.btnExternalExit.Click += new System.EventHandler(this.btnExit_Click);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBasicSettings;
        private System.Windows.Forms.TabPage tabFileNaming;
        private System.Windows.Forms.TabPage tabExternalSoftware;
        private System.Windows.Forms.TabPage tabLog;
        
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
        private System.Windows.Forms.GroupBox grpSeparator;
        private System.Windows.Forms.ComboBox cmbSeparatorType;
        private System.Windows.Forms.Label lblSeparatorType;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.Label lblFileNamePreview;
        private System.Windows.Forms.Label lblPreviewCaption;
        
        // External Software Tab
        private System.Windows.Forms.Button btnBrowseExternal;
        private System.Windows.Forms.TextBox txtExternalSoftware;
        private System.Windows.Forms.Label lblExternalSoftware;
        private System.Windows.Forms.GroupBox grpSoftwareSettings;
        private System.Windows.Forms.CheckBox chkAutoActivate;
        
        // Control and Log areas
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        
        // Basic Settings Controls
        private System.Windows.Forms.GroupBox grpBasicControls;
        private System.Windows.Forms.Button btnBasicStop;
        private System.Windows.Forms.Button btnBasicStart;
        private System.Windows.Forms.Button btnBasicExit;
        
        // External Software Controls
        private System.Windows.Forms.GroupBox grpExternalControls;
        private System.Windows.Forms.Button btnExternalStop;
        private System.Windows.Forms.Button btnExternalStart;
        private System.Windows.Forms.Button btnExternalExit;
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