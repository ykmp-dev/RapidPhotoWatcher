using System;
using System.Drawing;
using System.Windows.Forms;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// アプリケーション起動時のスプラッシュスクリーン
    /// </summary>
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            SetupSplash();
        }

        private void SetupSplash()
        {
            // フォームの基本設定
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 300);
            this.BackColor = Color.FromArgb(37, 99, 235); // Blue background
            this.TopMost = true;

            // メインラベル
            var titleLabel = new Label
            {
                Text = "RapidPhotoWatcher",
                Font = new Font("Microsoft Sans Serif", 24, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(460, 40),
                Location = new Point(20, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(titleLabel);

            // バージョンラベル
            var versionLabel = new Label
            {
                Text = "Version 1.1.0",
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular),
                ForeColor = Color.LightBlue,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(460, 25),
                Location = new Point(20, 110),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(versionLabel);

            // 説明ラベル
            var descLabel = new Label
            {
                Text = "写真ファイルの高速監視・自動整理アプリケーション",
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(460, 40),
                Location = new Point(20, 150),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(descLabel);

            // 読み込み中ラベル
            var loadingLabel = new Label
            {
                Text = "読み込み中...",
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                ForeColor = Color.LightGray,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(460, 25),
                Location = new Point(20, 220),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(loadingLabel);

            // プログレスバー
            var progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Size = new Size(300, 20),
                Location = new Point(100, 250)
            };
            this.Controls.Add(progressBar);

            // 著作権ラベル
            var copyrightLabel = new Label
            {
                Text = "Copyright © 2024 YuberTokyo",
                Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular),
                ForeColor = Color.LightGray,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(460, 15),
                Location = new Point(20, 275),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(copyrightLabel);

            // 角を丸くする
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// スプラッシュスクリーンを表示し、指定時間後に閉じる
        /// </summary>
        /// <param name="milliseconds">表示時間（ミリ秒）</param>
        public static void ShowSplash(int milliseconds = 3000)
        {
            var splash = new SplashForm();
            splash.Show();
            splash.Refresh();

            // 指定時間後に閉じる
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = milliseconds;
            timer.Tick += (s, e) => {
                timer.Stop();
                timer.Dispose();
                splash.Close();
                splash.Dispose();
            };
            timer.Start();

            // メッセージループを実行してスプラッシュを表示
            Application.DoEvents();
        }
    }
}