using System;
using System.Collections.Generic;
using BudgetLib;

namespace PConsole
{
    public static class BudgetUSOperations
    {
        internal static void OpenAccount(Budget<Account> budget)
        {
            Console.WriteLine("*Процедура відкриття нового рахунку*");
            Console.WriteLine("Введіть тип рахунку:\n\t1. 'small' - SMALL тип (ліміт 1000 грн.)\n\t2. 'middle' - MIDDLE тип (ліміт 20000 грн.)\n\t3. 'premium' - PREMIUM тип (ліміт 1000000 грн.)");

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
                    Console.WriteLine("Невірно вказаний тип рахунку. Будь-ласка перевірте коректність вводу.");
                    return;
            }
            Console.WriteLine("Введіть початкову суму на рахунку:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.OpenAccount(acType,sum,AccountHandler.OpenHandler,AccountHandler.CloseHandler, AccountHandler.PuHandler,
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler,AccountHandler.LimitHandler);
        }
        internal static void AccountIdsList(Budget<Account> budget)
        {
            List<int> ids = budget.GetListAccountsId;
            if (ids.Count == 0)
            {
                Console.WriteLine("Не знайдено жодного рахунку.");
                return;
            }
            Console.WriteLine("Мої рахунки (id):");
            for (int i = 0; i < ids.Count; i++)
            {
                Console.WriteLine($"\t{ids[i]}");
            }
        }
        
        internal static void Put(Budget<Account> budget)
        {
            Console.WriteLine("Виберіть рахунок (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Вкажіть суму поповнення:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Put(id,sum);
        }

        internal static void Withdraw(Budget<Account> budget)
        {
            Console.WriteLine("Виберіть рахунок (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Вкажіть суму зняття:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Withdraw(id,sum);
        }

        internal static void Transfer(Budget<Account> budget)
        {
            Console.WriteLine("Виберіть рахунок з якого відбуватиметься переказ (id):");
            int id1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Виберіть рахунок на який відбуватиметься переказ (id):");
            int id2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Вкажіть суму переказу:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Transfer(id1,id2,sum);
        }

        internal static void FindAccountInMyBudget(Budget<Account> budget)
        {
            Console.WriteLine("Вкажіть id рахунку:");
            int id = Convert.ToInt32(Console.ReadLine());
            if (budget.FindAccount(id) != null)
            {
                Console.WriteLine("Рахунок існує.");
            }
        }

        internal static void GetAccountInfo(Budget<Account> budget)
        {
            Console.WriteLine("Введіть id рахунку");
            int id = Convert.ToInt32(Console.ReadLine());
            budget.GetAccountInfo(id);
        }

        internal static void HistoryInfo(Budget<Account> budget)
        {
            Console.WriteLine("Введіть номер рахунку (id):");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Виберіть тип пошуку:\n\t1. 'get' - отримано.\n\t2. 'give' - знято.");
            string command = Convert.ToString(Console.ReadLine());
            Account.TypeHistoryEvent type;
            if (command == "get")
            {
                type = Account.TypeHistoryEvent.GetMoney;
            }
            else if (command == "give")
            {
                type = Account.TypeHistoryEvent.GivenMoney;
            }
            else
            {
                Console.WriteLine("Невідома команда. Спробуйте ще раз!");
                return;
            }
            Console.WriteLine("Історія операцій:");
            Account.HistoryAccount hst = budget.HistoryInfo(id);
            decimal sum = 0;
            foreach (var val in hst.historyList)
            {
                if (val.Type == type)
                {
                    Console.Write(val.Message+"\t");
                    Console.WriteLine(val.Sum+" грн.");
                    sum += val.Sum;
                }
            }
            Console.WriteLine($"Всього: {sum} грн.");
        }

        internal static void CloseAccount(Budget<Account> budget)
        {
            Console.WriteLine("ведіть номер рахунку для закриття (id):");
            int id = Convert.ToInt32(Console.ReadLine());
            budget.CloseAccount(id);
        }
    }
}