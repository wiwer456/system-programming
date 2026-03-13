using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        List<string> links = new List<string>
        {
            "https://learn.microsoft.com/ru-ru/",
            "https://www.github.com/",
            "https://www.mos.ru/",
            "https://github.com/wiwer456/system-programming"
        };

        var ct = new CancellationTokenSource();
        Console.WriteLine("Нажмите любую клавишу для отмены\n");
        var cancelTask = Task.Run(() =>
        {
            Console.ReadKey();
            ct.Cancel();
        });

        var progress = new Progress<int>(prog =>
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {prog}%");
        });

        try
        {
            int total = await Processlinks(links, progress, ct.Token);
        }
        catch (OperationCanceledException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nОперация отменена");
            Console.ResetColor();
        }
    }

    static async Task<int> Processlinks(List<string> links, IProgress<int> progress, CancellationToken token)
    {
        List<Task<int>> tasks = new();

        foreach (var link in links)
        {
            tasks.Add(DownloadData(link, progress, token));
        }

        int totalLength = 0;
        var results = await Task.WhenAll(tasks);

        foreach (var r in results)
            totalLength += r;

        return totalLength;
    }

    static async Task<int> DownloadData(string link, IProgress<int> progress, CancellationToken token)
    {
        using HttpClient client = new HttpClient();

        Console.WriteLine($"Начало загрузки: {link} (поток {Thread.CurrentThread.ManagedThreadId})");

        token.ThrowIfCancellationRequested();

        string data = await client.GetStringAsync(link, token);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Загрузка завершена: {link} (поток {Thread.CurrentThread.ManagedThreadId})");
        Console.ResetColor();

        int length = data.Length;

        for (int i = 0; i <= 100; i += 25)
        {
            token.ThrowIfCancellationRequested();
            progress.Report(i);
            await Task.Delay(500, token);
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Обработано: {link} (поток {Thread.CurrentThread.ManagedThreadId}) | {length} символов");
        Console.ResetColor();
        return length;
    }
}