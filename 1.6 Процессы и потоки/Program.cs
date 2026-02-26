using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {

        while (true)
        {
            Console.WriteLine("Команды:\n/list\n/kill [pid]\n/start [path]\n/info [pid|name]\nq");
            var input = Console.ReadLine();
            var command = input.Split(' ');

            if (command[0] == "/list")
            {
                Console.Clear();
                Console.WriteLine("ID\tИмя\t\t\t\tВремя запуска\t\tПриоритет");

                foreach (var p in Process.GetProcesses())
                {
                    try
                    {
                        Console.WriteLine($"{p.Id}\t{p.ProcessName,-25}\t{p.StartTime}\t{p.BasePriority}");
                    }
                    catch
                    {
                        Console.WriteLine($"{p.Id}\t{p.ProcessName,-25}\t{"---",-20}\t{p.BasePriority}");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nНажмите на любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }

            if (command[0] == "/kill")
            {
                try
                {
                    int id = int.Parse(command[1]);
                    var p = Process.GetProcessById(id);

                    if (p.MainWindowHandle != IntPtr.Zero)
                        p.CloseMainWindow();
                    else
                        p.Kill();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Процесс {command[1]} завершён");
                }
                catch 
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка!");
                }
                Console.ResetColor();
            }

            if (command[0] == "/start")
            {
                Console.Clear();
                Process.Start(command[1]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Процесс запущен");
                Console.ResetColor();
            }

            if (command[0] == "/info")
            {
                Console.Clear();
                Process prc;
                try
                {
                    if (int.TryParse(command[1], out int id))
                        prc = Process.GetProcessById(id);
                    else
                        prc = Process.GetProcesses().First(x => x.ProcessName == command[1]);

                    Console.WriteLine($"ID: {prc.Id}");
                    Console.WriteLine($"Имя: {prc.ProcessName}");
                    Console.WriteLine($"Время работы: {DateTime.Now - prc.StartTime}");
                    Console.WriteLine($"Память: {prc.PagedMemorySize64 / 1024 / 1024} МБ");
                    Console.WriteLine($"Потоки: {prc.Threads.Count}");
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка!");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nНажмите на любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }

            if (command[0] == "q")
                break;
        }
    }
}
