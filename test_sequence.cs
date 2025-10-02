using System;
using RapidPhotoWatcher;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== RapidPhotoWatcher 連番テスト ===");
        
        var settings = new AppSettings();
        
        // 初期状態のテスト
        Console.WriteLine($"初期状態: 開始番号={settings.SequenceStartNumber}, 現在の連番={settings.CurrentSequenceNumber}");
        
        // 連番を進める
        Console.WriteLine($"1枚目: {settings.GetNextSequenceNumber()}");
        Console.WriteLine($"2枚目: {settings.GetNextSequenceNumber()}");
        Console.WriteLine($"現在の連番: {settings.CurrentSequenceNumber}");
        
        // 開始番号を11に変更（バグ修正前：0003のまま、修正後：11にリセット）
        Console.WriteLine("\n--- 開始番号を11に変更 ---");
        settings.SequenceStartNumber = 11;
        Console.WriteLine($"変更後: 開始番号={settings.SequenceStartNumber}, 現在の連番={settings.CurrentSequenceNumber}");
        
        // 次の連番取得
        Console.WriteLine($"次の写真: {settings.GetNextSequenceNumber()}");
        Console.WriteLine($"その次: {settings.GetNextSequenceNumber()}");
        
        // 開始番号を5に変更
        Console.WriteLine("\n--- 開始番号を5に変更 ---");
        settings.SequenceStartNumber = 5;
        Console.WriteLine($"変更後: 開始番号={settings.SequenceStartNumber}, 現在の連番={settings.CurrentSequenceNumber}");
        Console.WriteLine($"次の写真: {settings.GetNextSequenceNumber()}");
        
        // リセット機能のテスト
        Console.WriteLine("\n--- リセット機能テスト ---");
        settings.ResetSequenceNumber();
        Console.WriteLine($"リセット後: 現在の連番={settings.CurrentSequenceNumber}");
        Console.WriteLine($"次の写真: {settings.GetNextSequenceNumber()}");
        
        // バリデーションテスト
        Console.WriteLine("\n--- バリデーションテスト ---");
        settings.SequenceStartNumber = -1;
        if (settings.Validate(out string error))
        {
            Console.WriteLine("バリデーション成功");
        }
        else
        {
            Console.WriteLine($"バリデーションエラー: {error}");
        }
        
        // 正常値に戻す
        settings.SequenceStartNumber = 1;
        if (settings.Validate(out error))
        {
            Console.WriteLine("バリデーション成功");
        }
        else
        {
            Console.WriteLine($"バリデーションエラー: {error}");
        }
        
        Console.WriteLine("\nテスト完了。何かキーを押してください...");
        Console.ReadKey();
    }
}