using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using prcthd;

namespace prcthd
{
    public class Program
    {
        static void Main()
        {
            BankAccount account = new BankAccount();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => account.WithDraw(150)));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}