using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public partial class Budget<T> where T : Account
    {
        public void Put(int id,decimal sum)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                OnFindAccount(new BudgetEventArgs("Неможливо знайти рахунок. Такого рахунка не існує."));
                throw new NullReferenceException("Unreal find account");
            }
            bool process = account.Put(sum);
            if (!process)
            {
                throw new Exception("money wasn't withdrawed. Process error!");
            }
            ToHistory(account, Account.TypeHistoryEvent.GetMoney, $"<Отримано {DateTime.Now}>", sum);
        }

        public void Withdraw(int id,decimal sum)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                OnFindAccount(new BudgetEventArgs("Неможливо знайти рахунок. Такого рахунка не існує."));
                throw new NullReferenceException("Unreal find account");
            }

            bool process = account.Withdraw(sum);
            if (!process)
            {
                throw new Exception("money wasn't withdrawed. Process error!");
            }
            ToHistory(account, Account.TypeHistoryEvent.GivenMoney, $"<Знято {DateTime.Now}>", sum);
        }

        public void Transfer(int id1,int id2, decimal sum)
        {
            T account1 = FindAccount(id1);
            if (account1 == null)
            {
                OnFindAccount(new BudgetEventArgs($"Неможливо знайти рахунок з id {id1}. Такого рахунка не існує."));
                throw new NullReferenceException("Unreal find account with such id");
            }

            T account2 = FindAccount(id2);
            if (account2 == null)
            {
                OnFindAccount(new BudgetEventArgs($"Неможливо знайти рахунок з id {id2}. Такого рахунка не існує."));
                throw new NullReferenceException("Not find account with such id");
            }
            bool process = account1.Transfer(account2,sum);
            if (process)
            {
                throw new Exception("money wasn't withdrawed. Process error!");
            }
            ToHistory(account1, Account.TypeHistoryEvent.GivenMoney,
                $"<Переведено на рахунок (id {id2}) {DateTime.Now}>", sum);
            ToHistory(account2, Account.TypeHistoryEvent.GetMoney,
                $"<Отримано переведенням з рахунку (id {id1}) {DateTime.Now}>", sum);
        }
    }
}