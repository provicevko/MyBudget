using System;
using System.Collections.Generic;
using BudgetLib.Account;
using BudgetLib.Budget;
using PConsole.EventHandlers;

namespace PConsole
{
    public static class BudgetUSOperations
    {
        internal static void OpenAccount(Budget<Account> budget)
        {
            Console.WriteLine("*The procedure for opening a new account*");
            Console.WriteLine("Enter the account type:\n\t1. 'small' - SMALL type (limit 1,000 UAH).\n\t2. 'middle' - MIDDLE type (limit 20,000 UAH).\n\t3. 'premium' - PREMIUM type (limit 1,000,000 UAH).");

            AccountType acType;
            string type = Convert.ToString(Console.ReadLine());
            switch (type)
            {
                case "small":
                    acType = AccountType.Small;
                    break;
                case "middle":
                    acType = AccountType.Middle;
                    break;
                case "premium":
                    acType = AccountType.Premium;
                    break;
                default:
                    Console.WriteLine("Invalid account type specified. Please check your input.");
                    throw new ArgumentException("acType must be: 'small', 'middle' or 'premium'");
            }
            Console.WriteLine("Enter the initial amount of money in the account:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.OpenAccount(acType,sum,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.ChangeTypeAccount, AccountHandler.AccountInfoHandler);
        }
        internal static void AccountIdsList(Budget<Account> budget)
        {
            List<int> ids = budget.GetListAccountsId;
            if (ids.Count == 0)
            {
                Console.WriteLine("No account found.");
                return;
            }
            Console.WriteLine("My accounts (id):");
            for (int i = 0; i < ids.Count; i++)
            {
                Console.WriteLine($"\t{ids[i]}");
            }
        }
        
        internal static void Put(Budget<Account> budget)
        {
            Console.WriteLine("Select account (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a comment (desirable 1 word less than 18 symbols):");
            string comment = Convert.ToString(Console.ReadLine()).ToLower();
            if (comment.Length > 18 || comment.Replace(" ", "").Length == 0)
            {
                Console.WriteLine("Incorrectly entered comment. Repeat the procedure again.");
                throw new ArgumentException("Keyword must be > 0 and <= 18");
            }
            Console.WriteLine("Enter amount of money to replenish:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Put(id,new Item(comment,sum));
        }

        internal static void Withdraw(Budget<Account> budget)
        {
            Console.WriteLine("Select account (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a comment (desirable 1 word less than 18 symbols):");
            string comment = Convert.ToString(Console.ReadLine()).ToLower();
            if (comment.Length > 18 || comment.Replace(" ", "").Length == 0)
            {
                Console.WriteLine("Incorrectly entered comment. Repeat the procedure again.");
                throw new ArgumentException("Keyword must be > 0 and <= 18");
            }
            Console.WriteLine("Enter amount of money to replenish:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Withdraw(id,new Item(comment,sum));
        }
 
        internal static void Transfer(Budget<Account> budget)
        {
            Console.WriteLine(@"\Select the account from which the transfer will take place (id):");
            int id1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Select the account to which the transfer will take place (id):");
            int id2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a comment (desirable 1 word less than 18 symbols):");
            string comment = Convert.ToString(Console.ReadLine()).ToLower();
            if (comment.Length > 18 || comment.Replace(" ", "").Length == 0)
            {
                Console.WriteLine("Incorrectly entered comment. Repeat the procedure again.");
                throw new ArgumentException("Keyword must be > 0 and <= 18");
            }
            Console.WriteLine("Enter amount of money to replenish:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Transfer(id1,id2,new Item(comment,sum));
        }

        internal static void FindAccountInMyBudget(Budget<Account> budget)
        {
            Console.WriteLine("Enter account id:");
            int id = Convert.ToInt32(Console.ReadLine());
            if (budget.FindAccount(id) != null)
            {
                Console.WriteLine("The account exists.");
            }
        }

        internal static void GetAccountInfo(Budget<Account> budget)
        {
            Console.WriteLine("Enter the account id:");
            int id = Convert.ToInt32(Console.ReadLine());
            budget.GetAccountInfo(id);
        }

        internal static void HistoryInfo(Budget<Account> budget)
        {
            Console.WriteLine("Enter account number (id):");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Select a search type:\n\t1. 'get' - received.\n\t2. 'give' - withdrawn.");
            string command = Convert.ToString(Console.ReadLine());
            Account.TypeHistoryEvent type;
            string specificator;
            switch (command)
            {
                case "get":
                    type = Account.TypeHistoryEvent.GetMoney;
                    Console.WriteLine("Select the query type:\n\t1. 'all' - show all.\n\t2. 'search' - search by keyword.");
                    specificator = Convert.ToString(Console.ReadLine());
                    break;
                case "give":
                    type = Account.TypeHistoryEvent.GivenMoney;
                    Console.WriteLine("Select the query type:\n\t1. 'all' - show all.\n\t2. 'search' - search by keyword.");
                    specificator = Convert.ToString(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Unknown command. Please try again!");
                    throw new ArgumentException("Command can be 'get' or 'give'");
            }

            if (specificator != "all" && specificator != "search")
            {
                Console.WriteLine("Invalid query type.");
                throw new ArgumentException("Specificator can be 'all' or 'search'");
            }
            Console.WriteLine("History of operations:");
            Account.HistoryAccount hst = budget.HistoryInfo(id);
            decimal sum = 0;
            if (specificator == "search")
            {
                Console.WriteLine("Enter a keyword to search for (reserved words: 'opening'):");
                string sword = Convert.ToString(Console.ReadLine()).ToLower();
                if (sword.Length > 18 || sword.Replace(" ", "").Length == 0)
                {
                    Console.WriteLine("Incorrectly entered comment. Repeat the procedure again.");
                    throw new ArgumentException("Keyword must be > 0 and <= 18");
                }
                foreach (var val in hst.HistoryList)
                {
                    if (val.Type == type && val.Item.Comment == sword)
                    {
                        Console.WriteLine(val.Message + "\t" + val.Item.Sum + " UAH" + "\t" + $"[ {val.Item.Comment} ]");
                        sum += val.Item.Sum;
                    }
                }
            }
            else
            {
                foreach (var val in hst.HistoryList)
                {
                    if (val.Type == type)
                    {
                        Console.WriteLine(val.Message + "\t" + val.Item.Sum + " UAH" + "\t" + $"[ {val.Item.Comment} ]");
                        sum += val.Item.Sum;
                    }
                }
            }
            Console.WriteLine($"Total: {sum} UAH");
        }

        internal static void CloseAccount(Budget<Account> budget)
        {
            Console.WriteLine("Enter account number to close (id):");
            int id = Convert.ToInt32(Console.ReadLine());
            budget.CloseAccount(id);
        }

        internal static void ChangeTypeAccount(Budget<Account> budget)
        {
            Console.WriteLine("Enter account number (id):");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the account type:\n\t1. 'small' - SMALL type (limit 1,000 UAH).\n\t2. 'middle' - MIDDLE type (limit 20,000 UAH).\n\t3. 'premium' - PREMIUM type (limit 1,000,000 UAH).");
            AccountType acType;
            string type = Convert.ToString(Console.ReadLine());
            switch (type)
            {
                case "small":
                    acType = AccountType.Small;
                    break;
                case "middle":
                    acType = AccountType.Middle;
                    break;
                case "premium":
                    acType = AccountType.Premium;
                    break;
                default:
                    Console.WriteLine("Invalid account type specified. Please check your input.");
                    throw new ArgumentException("acType must be: 'small', 'middle' or 'premium'");
            }
            budget.ChangeTypeAccount(id,acType);
        }
    }
}