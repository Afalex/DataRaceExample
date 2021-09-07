using System;
using System.Threading.Tasks;

namespace DataRace.Example
{
    struct Account
    {
        public decimal Balance { get; set; }
        public Account(decimal Balance)
        {
            this.Balance = Balance;
        }
    }
    class Program
    {
        static decimal Spent;
        static void Main(string[] args)
        {
            var account = new Account(1000000);

            var wifeTask = new Task(() => Spend(ref account, 1));
            var sonTask = new Task(() => Spend(ref account, 1));
            var husBandTask = new Task(() => Spend(ref account, 1));

            wifeTask.Start();
            sonTask.Start();
            husBandTask.Start();

            Task.WaitAll(new Task[] { wifeTask, sonTask, husBandTask });

            Console.WriteLine("Balance: " + account.Balance);
            Console.WriteLine("Spent: " + Spent);

            Console.ReadKey();
        }

        static void Spend(ref Account account, int spent)
        {
            for (int i = 0; i < 1000000; i++)
                if (spent <= account.Balance)
                {
                    account.Balance -= spent;
                    Spent++;
                }
        }
    }
}
