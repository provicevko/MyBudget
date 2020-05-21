using BudgetLib.Account;

namespace BudgetLib.Budget
{
    internal interface IBudget<T> where T : Account.Account
    {
        public event BudgetStateHandler FindAccountEvent;
        public string Name { get; }

        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler,
            AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler,AccountStateHandler changeTypeHandler,AccountStateHandler accountInfo);

        public void CloseAccount(int id);
        public T FindAccount(int id);
        private void ToHistory(Account.Account account, Account.Account.TypeHistoryEvent type, string message, decimal sum){}
        public void GetAccountInfo(int id);
        public void Put(int id,Item item);
        public void Withdraw(int id,Item item);
        public void Transfer(int id1, int id2,Item item);
    }
}