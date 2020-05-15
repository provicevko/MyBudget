using System;
using System.IO;
using BudgetLib.Account;
using BudgetLib.Budget;
using PConsole.EventHandlers;

namespace PConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Budget<Account> budget = new Budget<Account>("Provice budget");
            budget.FindAccountEvent += BudgetHandler.FindAccountHandler;
            budget.AccountInfo += BudgetHandler.AccountInfoHandler;
            budget.OpenAccountEvent += BudgetHandler.OpenAccountHandler;
            budget.ChangeTypeAccountEvent += BudgetHandler.ChangeTypeAccountAccount;
           
            string description = "Commands:\n<> 'new' - open a new account.\n<> 'put' - put on the account.\n<> 'withdraw' - withdraw from the account.\n"+
                                 "<> 'transfer' - transfer to another account.\n<> 'search' - account search.\n<> 'mylist' - list of my accounts.\n"+
            "<> 'ainfo' - account information.\n<> 'hinfo' - transaction history.\n<> 'tchange' - change account type.\n<> 'close' - close account.\n" +
                "<> 'help' - list of commands.\n<> 'exit' - exit.";
            Console.WriteLine(description);
            
            while (true)
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
                            Console.WriteLine("Unrecognized command.");
                            break;
                    }
                }
                catch (ArgumentNullException e)
                {
                    ErrorHandler.Logs(e);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Runtime error. Repeat the procedure again!");
                    ErrorHandler.Logs(e);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Runtime error. Check that the input values are correct!");
                    ErrorHandler.Logs(e);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("The procedure was aborted. Check that the input values are correct.");
                    ErrorHandler.Logs(e);
                }
                catch (IOException)
                {
                    Console.WriteLine("The program does not work correctly. Try restarting!");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("The procedure was aborted. Check that the input is correct.");
                    ErrorHandler.Logs(e);
                }
            }
        }
    }
}