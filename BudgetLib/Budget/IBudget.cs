using System;
using BudgetLib.Account;

namespace BudgetLib.Budget
{
    internal interface IBudget<T> where T : Account.Account
    {
        public string Name { get; }

        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler,
            AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler,AccountStateHandler changeTypeHandler);

        public void CloseAccount(int id);
        public T FindAccount(int id);
        public Tuple<string, int, AccountType, decimal, decimal, DateTime> GetAccountInfo(int id);
        public void Put(int id,Item item);
        public void Withdraw(int id,Item item);
        public void Transfer(int id1, int id2,Item item);
    }
}