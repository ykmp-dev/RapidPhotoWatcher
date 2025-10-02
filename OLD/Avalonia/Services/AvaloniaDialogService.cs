using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace RapidPhotoWatcher.Avalonia.Services
{
    /// <summary>
    /// Avalonia UI向けダイアログサービス実装
    /// </summary>
    public class AvaloniaDialogService : IDialogService
    {
        private Window? GetMainWindow()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }

        public async Task<string?> ShowFolderDialogAsync(string title)
        {
            var dialog = new OpenFolderDialog
            {
                Title = title
            };

            var mainWindow = GetMainWindow();
            return await dialog.ShowAsync(mainWindow);
        }

        public async Task<string?> ShowOpenFileDialogAsync(string title, string filter)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                AllowMultiple = false
            };

            // フィルターを解析してAvalonia形式に変換
            if (!string.IsNullOrEmpty(filter))
            {
                dialog.Filters = ParseFileFilters(filter);
            }

            var mainWindow = GetMainWindow();
            var result = await dialog.ShowAsync(mainWindow);
            return result?.FirstOrDefault();
        }

        public async Task ShowInfoAsync(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
        }

        public async Task ShowWarningAsync(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning);
        }

        public async Task ShowErrorAsync(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
        }

        public async Task<bool> ShowConfirmAsync(string title, string message)
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question);
            return result == MsBox.Avalonia.Enums.ButtonResult.Yes;
        }

        private List<FileDialogFilter> ParseFileFilters(string filter)
        {
            var filters = new List<FileDialogFilter>();
            
            try
            {
                // Windows Forms形式のフィルター文字列を解析
                // 例: "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*"
                var parts = filter.Split('|');
                
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (i + 1 < parts.Length)
                    {
                        var name = parts[i];
                        var extensions = parts[i + 1].Replace("*.", "").Split(';').ToList();
                        
                        filters.Add(new FileDialogFilter
                        {
                            Name = name,
                            Extensions = extensions
                        });
                    }
                }
            }
            catch
            {
                // フィルター解析に失敗した場合はデフォルトフィルター
                filters.Add(new FileDialogFilter
                {
                    Name = "すべてのファイル",
                    Extensions = { "*" }
                });
            }
            
            return filters;
        }
    }

    /// <summary>
    /// MessageBox管理用のヘルパークラス
    /// </summary>
    public static class MessageBoxManager
    {
        public static System.Threading.Tasks.Task<MsBox.Avalonia.Enums.ButtonResult> GetMessageBoxStandard(string title, string text, 
            MsBox.Avalonia.Enums.ButtonEnum button, MsBox.Avalonia.Enums.Icon icon)
        {
            return MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams
            {
                ButtonDefinitions = button,
                ContentTitle = title,
                ContentMessage = text,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }).ShowAsync();
        }
    }
}