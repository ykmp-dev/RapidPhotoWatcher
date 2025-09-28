using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPEGFolderMonitor
{
    /// <summary>
    /// 外部ソフトウェア制御クラス - アクティブ化と画像選択機能
    /// </summary>
    public class ExternalSoftwareController
    {
        private readonly LogManager _logManager;
        private string? _externalSoftwarePath;
        private readonly SemaphoreSlim _activationSemaphore = new SemaphoreSlim(1, 1);

        // Windows API
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const int SW_RESTORE = 9;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMAXIMIZED = 3;
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_SHOWWINDOW = 0x0040;

        public ExternalSoftwareController(LogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        /// <summary>
        /// 外部ソフトウェアを起動または既存プロセスをアクティブ化し、画像位置移動を実行
        /// </summary>
        public async Task<bool> LaunchOrActivateAndNavigateAsync(string softwarePath, ImageNavigationDirection direction, string? targetFolder = null)
        {
            // 同時実行を防ぐ
            await _activationSemaphore.WaitAsync();
            try
            {
                _logManager.LogInfo("外部ソフトウェア連携処理開始（排他制御済み）");
                _externalSoftwarePath = softwarePath;

                if (!File.Exists(softwarePath))
                {
                    _logManager.LogError($"外部ソフトウェアが見つかりません: {softwarePath}");
                    return false;
                }

                _logManager.LogInfo($"外部ソフトウェアパス: {softwarePath}");
                
                // 外部ソフトウェアが起動しているかチェック
                if (!IsExternalSoftwareRunning(softwarePath))
                {
                    _logManager.LogInfo("外部ソフトウェアが起動していないため、起動します");
                    // 起動していない場合は起動
                    var launched = await LaunchExternalSoftwareAsync(softwarePath, targetFolder);
                    if (!launched)
                    {
                        return false;
                    }
                    
                    // 起動後少し待つ
                    await Task.Delay(2000);
                }
                else
                {
                    _logManager.LogInfo("外部ソフトウェアは既に起動中です");
                }

                // 外部ソフトウェアのウィンドウを探す
                var hwnd = FindExternalSoftwareWindow();
                
                if (hwnd == IntPtr.Zero)
                {
                    _logManager.LogWarning("外部ソフトウェアのウィンドウが見つかりません。フォルダ指定で再起動を試行します。");
                    
                    // ウィンドウが見つからない場合、フォルダ指定で再起動を試みる
                    var relaunched = await LaunchExternalSoftwareAsync(softwarePath, targetFolder);
                    if (!relaunched)
                    {
                        _logManager.LogError("外部ソフトウェアの再起動に失敗しました");
                        return false;
                    }
                    
                    // 再起動後、ウィンドウが表示されるまで待つ
                    await Task.Delay(3000);
                    
                    // 再度ウィンドウを検索
                    hwnd = FindExternalSoftwareWindow();
                    if (hwnd == IntPtr.Zero)
                    {
                        _logManager.LogError("再起動後もウィンドウが見つかりません");
                        return false;
                    }
                }
                
                _logManager.LogInfo($"ウィンドウハンドル発見: {hwnd}");

                // 確実なフォアグラウンド化
                var success = ForceWindowToForeground(hwnd);
                _logManager.LogInfo($"ウィンドウをフォアグラウンド化しました: {(success ? "成功" : "失敗")}");
                
                // 少し待ってから画像位置移動コマンドを送信
                await Task.Delay(500);
                
                // 画像位置移動を実行
                await NavigateToLatestImage(direction);
                
                _logManager.LogInfo($"外部ソフトウェアをフォアグラウンド化し、画像位置移動を実行しました");
                return true;
            }
            catch (Exception ex)
            {
                _logManager.LogError($"外部ソフトウェア起動/フォアグラウンド化エラー: {ex.Message}");
                return false;
            }
            finally
            {
                _activationSemaphore.Release();
                _logManager.LogInfo("外部ソフトウェア連携処理終了（排他制御解除）");
            }
        }

        /// <summary>
        /// 外部ソフトウェアを起動
        /// </summary>
        private Task<bool> LaunchExternalSoftwareAsync(string softwarePath, string? targetFolder)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = softwarePath,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                // ターゲットフォルダが指定されている場合は引数として渡す
                if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
                {
                    startInfo.Arguments = $"\"{targetFolder}\"";
                }

                var process = Process.Start(startInfo);
                
                if (process != null)
                {
                    _logManager.LogInfo($"外部ソフトウェアを起動しました: {Path.GetFileName(softwarePath)}");
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logManager.LogError($"外部ソフトウェア起動エラー: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 最新画像に移動
        /// </summary>
        private async Task NavigateToLatestImage(ImageNavigationDirection direction)
        {
            try
            {
                // ナビゲーション方向に応じてキーを送信
                var navigationKey = direction == ImageNavigationDirection.End ? "{END}" : "{HOME}";
                var directionText = direction == ImageNavigationDirection.End ? "最下部（最新）" : "最上部（最古）";
                
                _logManager.LogInfo($"画像位置移動準備: {directionText}へ移動 ({navigationKey})");

                // 確実にフォーカスが当たるまで少し待つ
                await Task.Delay(750);

                // キー送信をリトライ付きで実行
                var success = await SendKeyWithRetryAsync(navigationKey, maxRetries: 3);
                
                if (success)
                {
                    _logManager.LogInfo($"画像位置移動コマンド送信成功: {directionText}");
                }
                else
                {
                    _logManager.LogWarning($"画像位置移動コマンド送信失敗: {directionText}");
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"画像ナビゲーションエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// リトライ機能付きキー送信
        /// </summary>
        private async Task<bool> SendKeyWithRetryAsync(string key, int maxRetries = 3)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logManager.LogInfo($"キー送信試行 {attempt}/{maxRetries}: {key}");
                    
                    SendKeys.SendWait(key);
                    
                    _logManager.LogInfo($"キー送信成功 (試行{attempt})");
                    return true;
                }
                catch (Exception ex)
                {
                    _logManager.LogWarning($"キー送信失敗 (試行{attempt}): {ex.Message}");
                    
                    if (attempt < maxRetries)
                    {
                        // 次の試行前に少し待つ
                        await Task.Delay(200);
                    }
                }
            }
            
            _logManager.LogError($"キー送信が{maxRetries}回とも失敗しました: {key}");
            return false;
        }

        /// <summary>
        /// 外部ソフトウェアのウィンドウを探す
        /// </summary>
        private IntPtr FindExternalSoftwareWindow()
        {
            if (string.IsNullOrEmpty(_externalSoftwarePath))
            {
                return IntPtr.Zero;
            }

            var processName = Path.GetFileNameWithoutExtension(_externalSoftwarePath);
            _logManager.LogInfo($"プロセス検索: {processName}");
            var processes = Process.GetProcessesByName(processName);
            _logManager.LogInfo($"見つかったプロセス数: {processes.Length}");

            IntPtr foundWindow = IntPtr.Zero;

            foreach (var process in processes)
            {
                _logManager.LogInfo($"プロセス ID: {process.Id}, メインウィンドウハンドル: {process.MainWindowHandle}");
                
                // まずMainWindowHandleをチェック
                if (process.MainWindowHandle != IntPtr.Zero && IsWindowVisible(process.MainWindowHandle))
                {
                    _logManager.LogInfo($"メインウィンドウを発見: {process.MainWindowHandle}");
                    return process.MainWindowHandle;
                }

                // MainWindowHandleが0の場合、すべてのウィンドウを列挙してプロセスIDで検索
                _logManager.LogInfo($"プロセス {process.Id} のすべてのウィンドウを検索中...");
                
                EnumWindows((hWnd, lParam) =>
                {
                    GetWindowThreadProcessId(hWnd, out uint windowProcessId);
                    
                    if (windowProcessId == process.Id && IsWindowVisible(hWnd) && GetWindowTextLength(hWnd) > 0)
                    {
                        var windowTitle = GetWindowTitle(hWnd);
                        _logManager.LogInfo($"ウィンドウ発見: ハンドル={hWnd}, タイトル='{windowTitle}'");
                        
                        // FastStone Image Viewerの特定のウィンドウタイトルパターンをチェック
                        if (IsValidViewerWindow(windowTitle, processName))
                        {
                            foundWindow = hWnd;
                            _logManager.LogInfo($"有効なビューアウィンドウを発見: {hWnd}");
                            return false; // 検索終了
                        }
                    }
                    return true; // 検索続行
                }, IntPtr.Zero);

                if (foundWindow != IntPtr.Zero)
                {
                    return foundWindow;
                }
            }

            _logManager.LogWarning("有効なウィンドウが見つかりませんでした");
            return IntPtr.Zero;
        }

        /// <summary>
        /// ウィンドウタイトルを取得
        /// </summary>
        private string GetWindowTitle(IntPtr hWnd)
        {
            var length = GetWindowTextLength(hWnd);
            if (length == 0) return string.Empty;

            var builder = new System.Text.StringBuilder(length + 1);
            GetWindowText(hWnd, builder, builder.Capacity);
            return builder.ToString();
        }

        /// <summary>
        /// 有効な画像ビューアウィンドウかチェック
        /// </summary>
        private bool IsValidViewerWindow(string windowTitle, string processName)
        {
            if (string.IsNullOrEmpty(windowTitle))
                return false;

            // FastStone Image Viewer の一般的なウィンドウタイトルパターン
            var validPatterns = new[]
            {
                "FastStone Image Viewer",
                "FSViewer",
                ".jpg",
                ".jpeg", 
                ".png",
                ".bmp",
                ".gif",
                ".cr2",
                ".cr3",
                ".arw",
                ".nef",
                ".rw2",
                ".orf",
                ".dng"
            };

            foreach (var pattern in validPatterns)
            {
                if (windowTitle.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ウィンドウを確実にフォアグラウンドに持ってくる
        /// </summary>
        private bool ForceWindowToForeground(IntPtr hWnd)
        {
            try
            {
                _logManager.LogInfo("フォアグラウンド化処理開始");

                // ウィンドウが有効かチェック
                if (!IsWindow(hWnd))
                {
                    _logManager.LogWarning("無効なウィンドウハンドル");
                    return false;
                }

                // 現在のフォアグラウンドウィンドウを取得
                var foregroundWindow = GetForegroundWindow();
                
                // 既にフォアグラウンドの場合
                if (foregroundWindow == hWnd)
                {
                    _logManager.LogInfo("既にフォアグラウンドです");
                    return true;
                }

                // ウィンドウのスレッドIDを取得
                GetWindowThreadProcessId(hWnd, out uint targetProcessId);
                var targetThreadId = GetWindowThreadProcessId(hWnd, out _);
                var currentThreadId = GetCurrentThreadId();

                _logManager.LogInfo($"ターゲットプロセスID: {targetProcessId}, 現在のスレッドID: {currentThreadId}");

                bool attached = false;
                try
                {
                    // 異なるスレッドの場合、スレッドを結合
                    if (targetThreadId != currentThreadId)
                    {
                        attached = AttachThreadInput(currentThreadId, targetThreadId, true);
                        _logManager.LogInfo($"スレッド結合: {(attached ? "成功" : "失敗")}");
                    }

                    // ウィンドウの現在の状態を確認
                    bool isMaximized = IsZoomed(hWnd);
                    _logManager.LogInfo($"ウィンドウ状態確認: 最大化={isMaximized}");

                    // ウィンドウを適切に表示（最大化状態を維持）
                    if (isMaximized)
                    {
                        ShowWindow(hWnd, SW_SHOWMAXIMIZED);
                        _logManager.LogInfo("最大化状態でウィンドウを表示");
                    }
                    else
                    {
                        ShowWindow(hWnd, SW_RESTORE);
                        _logManager.LogInfo("通常状態でウィンドウを復元");
                    }
                    
                    // ウィンドウを最前面に移動
                    BringWindowToTop(hWnd);
                    
                    // フォアグラウンドに設定
                    SetForegroundWindow(hWnd);
                    
                    // SetWindowPosで確実に最前面に
                    SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

                    _logManager.LogInfo("フォアグラウンド化処理完了");
                    return true;
                }
                finally
                {
                    // スレッドの結合を解除
                    if (attached)
                    {
                        AttachThreadInput(currentThreadId, targetThreadId, false);
                        _logManager.LogInfo("スレッド結合解除");
                    }
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"フォアグラウンド化エラー: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 外部ソフトウェアが実行中かチェック
        /// </summary>
        public bool IsExternalSoftwareRunning(string softwarePath)
        {
            try
            {
                var processName = Path.GetFileNameWithoutExtension(softwarePath);
                var processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            _activationSemaphore?.Dispose();
        }
    }
}