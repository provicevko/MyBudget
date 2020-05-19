using System;

namespace BudgetLib.Budget
{
    public partial class Budget<T> : IBudget<T> where T : Account.Account
    {
        public void Put(int id,Item item) // put money to the account
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
            account.Put(item.Sum);
            ToHistory(account, Account.Account.TypeHistoryEvent.GetMoney, $"<Received {DateTime.Now}>",item);
        }

        public void Withdraw(int id,Item item) // withdraw money from account
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
            account.Withdraw(item.Sum);
            ToHistory(account, Account.Account.TypeHistoryEvent.GivenMoney, $"<Withdrawn {DateTime.Now}>",item);
        }

        public void Transfer(int id1,int id2, Item item) // transfer money to other account
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
            account1.Transfer(account2,item.Sum);
            ToHistory(account1, Account.Account.TypeHistoryEvent.GivenMoney,
                $"<Transferred to account (id {id2}) {DateTime.Now}>", item);
            ToHistory(account2, Account.Account.TypeHistoryEvent.GetMoney,
                $"<Received by transfer from account (id {id1}) {DateTime.Now}>",item);
        }
    }
}