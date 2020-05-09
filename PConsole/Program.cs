using System;
using System.Collections.Generic;
using BudgetLib;

namespace PConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Budget<Account> budget = new Budget<Account>("Provice budget");
            budget.FindAccountEvent += BudgetHandler.FindAccountHandler;
            budget.AccountInfo += BudgetHandler.AccountInfoHandler;
            budget.OpenAccount(AccountType.Small,500,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            budget.OpenAccount(AccountType.Middle,5000,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            budget.OpenAccount(AccountType.Premium,500000,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            budget.GetAccountInfo(2);
            budget.GetAccountInfo(3);
            budget.Put(1,200);
            budget.GetAccountInfo(1);
            budget.Withdraw(1,600);
            budget.GetAccountInfo(1);
            budget.Transfer(2,1,500);
            budget.GetAccountInfo(1);
            budget.GetAccountInfo(2);
            


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