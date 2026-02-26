using System;
using System.Threading;

namespace prcthd
{
    public class BankAccount
    {
        private int balance = 1000;
        private readonly object _locker = new object();

        public void WithDraw(int amount)
        {
            lock (_locker)
            {
                if (balance >= amount)
                {
                    Thread.Sleep(10);
                    balance -= amount;
                    Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: снято {amount} (осталось {balance})");
                }
                else
                {
                    Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: ОТКАЗ (осталось {balance})");
                }
            }
        }
    }
}