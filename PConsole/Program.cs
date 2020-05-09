using System;
using System.Collections.Generic;
using System.Threading;
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
            // budget.OpenAccount(AccountType.Small,100,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
            //     AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            // budget.OpenAccount(AccountType.Middle,5000,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
            //     AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            // budget.OpenAccount(AccountType.Premium,500000,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
            //     AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
            // budget.GetAccountInfo(2);
            // budget.GetAccountInfo(3);
            // budget.Put(1,200);
            // budget.GetAccountInfo(1);
            // budget.Put(1,100);
            // budget.Put(1,100);
            // budget.Withdraw(1,20);
            // budget.Withdraw(1,300);
            // budget.Transfer(1,2,150);
            // budget.GetAccountInfo(1);
            // Account.HistoryAccount hst = budget.HistoryInfo(1);
            // foreach (var item in hst.historyList)
            // {
            //     if (item.Type == Account.TypeHistoryEvent.GivenMoney)
            //     {
            //         Console.Write(item.Message+"\t");
            //         Console.WriteLine(item.Sum+" грн.");
            //     }
            // }
            

            bool alive = true;
            while (alive)
            {
                string description = "Команди:\n1. 'new' - відкрити новий рахунок.\n2. 'put' - покласти на рахунок.\n3. 'withdraw' - вивести з рахунку.\n" +
                                     "4. 'transfer' - перевести на інший рахунок.\n5. 'search' - пошук рахунку.\n6. 'mylist' - список моїх рахунків\n" +
                                     "7. 'ainfo' - інформація про рахунок.\n8. 'hinfo' - історія операцій.\n9. 'close' - закрити рахунок.\n 10. 'exit' - вихід.";
                Console.WriteLine(description);
                try
                {
                    string command = Convert.ToString(Console.ReadLine());
                    switch (command)
                    {
                        case "new":
                            BudgetUSOperations.OpenAccount(budget);
                            break;
                        case "put":
                            BudgetUSOperations.Put(budget);
                            break;
                        case "withdraw":
                            BudgetUSOperations.Withdraw(budget);
                            break;
                        case "transfer":
                            BudgetUSOperations.Transfer(budget);
                            break;
                        case "search":
                            BudgetUSOperations.FindAccountInMyBudget(budget);
                            break;
                        case "mylist":
                            BudgetUSOperations.AccountIdsList(budget);
                            break;
                        case "ainfo":
                            BudgetUSOperations.GetAccountInfo(budget);
                            break;
                        case "hinfo":
                            BudgetUSOperations.HistoryInfo(budget);
                            break;
                        case "close":
                            BudgetUSOperations.CloseAccount(budget);
                            break;
                        default:
                            Console.WriteLine("Нерозпізнана команда.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
    }
}