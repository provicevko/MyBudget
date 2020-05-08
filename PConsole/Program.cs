using System;
using BudgetLib;

namespace PConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Budget<Account> budget = new Budget<Account>("Provice budget!");
            budget.FindAccountEvent += BudgetHandler.FindAccountHandler;
            budget.ChooseAccountEvent += BudgetHandler.ChooseAccountHandler;
            budget.OpenAccount(AccountType.Small,500,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            budget.OpenAccount(AccountType.Middle,500,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            budget.OpenAccount(AccountType.Premium,500,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            // budget.ChooseCurrentAccount(2);
            budget.CloseAccount(2);
            foreach (var id in budget.GetListAccountsId)
            {
                Console.WriteLine(id);
            }





            // bool alive = true;
            // while (alive)
            // {
            //     string description = "Команди:\n1. 'new' - відкрити новий рахунок.\n2. 'put' - покласти на рахунок.\n3. 'withdraw' - вивести з рахунку.\n" +
            //                          "4. Перевести на інший рахунок.\n5. Пошук рахунку.\n6. Змінити поточний рахунок.\n7. Закрити рахунок.";
            //     Console.WriteLine(description);

            // }
        }
        
    }
}