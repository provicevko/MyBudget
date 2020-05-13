using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public partial class Budget<T> : IBudget<T> where T : Account
    {
        public void Put(int id,decimal sum,string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            account.Put(sum);
            ToHistory(account, Account.TypeHistoryEvent.GetMoney, $"<Отримано {DateTime.Now}>", sum,item);
        }

        public void Withdraw(int id,decimal sum,string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            account.Withdraw(sum);
            ToHistory(account, Account.TypeHistoryEvent.GivenMoney, $"<Знято {DateTime.Now}>", sum,item);
        }

        public void Transfer(int id1,int id2, decimal sum)
        {
            T account1 = FindAccount(id1);
            if (account1 == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id1}");
            }

            T account2 = FindAccount(id2);
            if (account2 == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id2}");
            }
            account1.Transfer(account2,sum);
            ToHistory(account1, Account.TypeHistoryEvent.GivenMoney,
                $"<Переведено на рахунок (id {id2}) {DateTime.Now}>", sum,"переведення");
            ToHistory(account2, Account.TypeHistoryEvent.GetMoney,
                $"<Отримано переведенням з рахунку (id {id1}) {DateTime.Now}>", sum,"переведення");
        }
    }
}