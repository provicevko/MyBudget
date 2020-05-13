using System;
using System.IO;
using BudgetLib;
using MoneySpendingItems;

namespace PConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PutItems putItems = new PutItems();
            SpendItems spendItems = new SpendItems();
            MoneyItems.AddItems(putItems);
            MoneyItems.SpendItems(spendItems);
            
            Budget<Account> budget = new Budget<Account>("Provice budget");
            budget.FindAccountEvent += BudgetHandler.FindAccountHandler;
            budget.AccountInfo += BudgetHandler.AccountInfoHandler;
            budget.OpenAccountEvent += BudgetHandler.OpenAccountHandler;
            budget.ChangeTypeEvent += BudgetHandler.ChangeTypeAccount;
           
            string description = "Команди:\n<> 'new' - відкрити новий рахунок.\n<> 'put' - покласти на рахунок.\n<> 'withdraw' - вивести з рахунку.\n" +
                                 "<> 'transfer' - перевести на інший рахунок.\n<> 'search' - пошук рахунку.\n<> 'mylist' - список моїх рахунків.\n" +
                                 "<> 'ainfo' - інформація про рахунок.\n<> 'hinfo' - історія операцій.\n<> 'tchange' - змінити тип рахунку.\n<> 'close' - закрити рахунок.\n" +
                                 "<> 'help' - список команд.\n<> 'exit' - вихід.";
            Console.WriteLine(description);

            bool alive = true;
            while (alive)
            {
                try
                {
                    string command = Convert.ToString(Console.ReadLine());
                    switch (command)
                    {
                        case "new":
                            BudgetUSOperations.OpenAccount(budget);
                            break;
                        case "put":
                            BudgetUSOperations.Put(budget, putItems);
                            break;
                        case "withdraw":
                            BudgetUSOperations.Withdraw(budget, spendItems);
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
                        case "tchange":
                            BudgetUSOperations.ChangeTypeAccount(budget);
                            break;
                        case "help":
                            Console.WriteLine(description);
                            break;
                        case "close":
                            BudgetUSOperations.CloseAccount(budget);
                            break;
                        case "exit":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Нерозпізнана команда.");
                            break;
                    }
                }
                catch (ArgumentNullException e)
                {
                    ErrorHandler.Logs(e);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Невідома помилка під час виконання. Повторіть процедуру ще раз!");
                    ErrorHandler.Logs(e);
                }
                catch (ArgumentException e)
                {
                    ErrorHandler.Logs(e);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("Виконання процедури було перервано. Перевірте коректність вводу.");
                    ErrorHandler.Logs(e);
                }
                catch (FileLoadException)
                {
                    Console.WriteLine("Програма не працює коректно. Спробуйте перезапустити!");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Виконання процедури було перервано. Перевірте коректність вводу.");
                    ErrorHandler.Logs(e);
                }
            }
        }
    }
}