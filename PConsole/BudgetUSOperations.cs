using System;
using System.Collections.Generic;
using BudgetLib;
using MoneySpendingItems;

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
                AccountHandler.WithdrawHandler,AccountHandler.TransferHandler);
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
        
        internal static void Put(Budget<Account> budget,PutItems putItems)
        {
            Console.WriteLine("Виберіть рахунок (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Виберіть дію для поповнення (введіть цифру):");
            for (int i = 0; i < putItems.GetItems.Count; i++)
            {
                Console.WriteLine($"{i+1}:\t{putItems.GetItems[i].Name}");
            }
            int indexItem = Convert.ToInt32(Console.ReadLine());
            if (indexItem < 1 || indexItem > putItems.GetItems.Count)
            {
                throw new ArgumentOutOfRangeException("indexItem");
            }

            string item;
            if (indexItem == putItems.GetItems.Count) // other
            {
                Console.WriteLine("Введіть коментар поповнення (не більше 18 символів):");
                item = Convert.ToString(Console.ReadLine()).ToLower();
                if (item.Length > 18 || item.Replace(" ","").Length == 0)
                {
                    Console.WriteLine("Некоректно введений коментар. Повторіть процедуру ще раз.");
                    throw new ArgumentException("Not correctly input comment to put operation");
                }
            }
            else
            {
                item = putItems.GetItems[indexItem - 1].Name;
            }
            Console.WriteLine("Вкажіть суму поповнення:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Put(id,sum,item);
        }

        internal static void Withdraw(Budget<Account> budget,SpendItems spendItems)
        {
            Console.WriteLine("Виберіть рахунок (id):");
            AccountIdsList(budget);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Виберіть дію для зняття (введіть цифру):");
            for (int i = 0; i < spendItems.GetItems.Count; i++)
            {
                Console.WriteLine($"{i+1}:\t{spendItems.GetItems[i].Name}");
            }
            int indexItem = Convert.ToInt32(Console.ReadLine());
            if (indexItem < 1 || indexItem > spendItems.GetItems.Count)
            {
                throw new ArgumentOutOfRangeException("indexItem");
            }
            string item;
            if (indexItem == spendItems.GetItems.Count) // other
            {
                Console.WriteLine("Введіть коментар поповнення (не більше 18 символів):");
                item = Convert.ToString(Console.ReadLine()).ToLower();
                if (item.Length > 18 || item.Replace(" ","").Length == 0)
                {
                    Console.WriteLine("Некоректно введений коментар. Повторіть процедуру ще раз.");
                    throw new ArgumentException("Not correctly input comment to put operation");
                }
            }
            else
            {
                item = spendItems.GetItems[indexItem - 1].Name;
            }
            Console.WriteLine("Вкажіть суму зняття:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            budget.Withdraw(id,sum,item);
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
            string specificator;
            switch (command)
            {
                case "get":
                    type = Account.TypeHistoryEvent.GetMoney;
                    Console.WriteLine("Виберіть тип запиту:\n\t1. 'all' - показати всі.\n\t2. 'search' - пошук по ключовому слову");
                    specificator = Convert.ToString(Console.ReadLine());
                    break;
                case "give":
                    type = Account.TypeHistoryEvent.GivenMoney;
                    Console.WriteLine("Виберіть тип запиту:\n\t1. 'all' - показати всі.\n\t2. 'search' - пошук по ключовому слову");
                    specificator = Convert.ToString(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Невідома команда. Спробуйте ще раз!");
                    return;
            }

            if (specificator != "all" && specificator != "search")
            {
                Console.WriteLine("Некоректний тип запиту.");
                throw new ArgumentException("Specificator can be 'all' or 'search'");
            }
            Console.WriteLine("Історія операцій:");
            Account.HistoryAccount hst = budget.HistoryInfo(id);
            decimal sum = 0;
            if (specificator == "search")
            {
                Console.WriteLine("Введіть ключове слово для пошуку (зарезервовані слова: 'відкриття', 'переведення'):");
                string sword = Convert.ToString(Console.ReadLine()).ToLower();
                foreach (var val in hst.historyList)
                {
                    if (val.Type == type && val.Item == sword)
                    {
                        Console.WriteLine(val.Message + "\t" + val.Sum + " грн." + "\t" + $"[ {val.Item} ]");
                        sum += val.Sum;
                    }
                }
            }
            else
            {
                foreach (var val in hst.historyList)
                {
                    if (val.Type == type)
                    {
                        Console.WriteLine(val.Message + "\t" + val.Sum + " грн." + "\t" + $"[ {val.Item} ]");
                        sum += val.Sum;
                    }
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

        internal static void ChangeTypeAccount(Budget<Account> budget)
        {
            Console.WriteLine("Введіть номер рахунку (id):");
            int id = Convert.ToInt32(Console.ReadLine());
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
            budget.ChangeTypeAccount(id,acType);
        }
    }
}