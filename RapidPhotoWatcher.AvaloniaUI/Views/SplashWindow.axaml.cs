using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace RapidPhotoWatcher.AvaloniaUI.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();
        
        // フェードインアニメーション開始
        _ = StartFadeInAnimation();
    }
    
    private async Task StartFadeInAnimation()
    {
        // 少し待ってからフェードイン開始
        await Task.Delay(100);
        
        // フェードインアニメーション（段階的に透明度を上げる）
        int steps = 20;
        for (int i = 0; i <= steps; i++)
        {
            double opacity = (double)i / steps;
            Opacity = opacity;
            await Task.Delay(800 / steps);
        }
        
        // 2秒間表示
        await Task.Delay(2000);
        
        // フェードアウトアニメーション（段階的に透明度を下げる）
        steps = 10;
        for (int i = steps; i >= 0; i--)
        {
            double opacity = (double)i / steps;
            Opacity = opacity;
            await Task.Delay(500 / steps);
        }
        
        // スプラッシュ完了を通知してウィンドウを閉じる
        SplashCompleted?.Invoke();
        Close();
    }
    
    public event Action? SplashCompleted;
}