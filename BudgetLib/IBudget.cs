namespace BudgetLib
{
    internal interface IBudget<T> where T : Account
    {
        public event BudgetStateHandler FindAccountEvent;
        public event BudgetStateHandler AccountInfo;
        public event BudgetStateHandler OpenAccountEvent;
        public event BudgetStateHandler ChangeTypeAccountEvent;
        public string Name { get; }

        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler,
            AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler);

        public void CloseAccount(int id);
        public T FindAccount(int id);
        private void ToHistory(Account account, Account.TypeHistoryEvent type, string message, decimal sum){}
        public void GetAccountInfo(int id);
        public void Put(int id, decimal sum,string item);
        public void Withdraw(int id, decimal sum,string item);
        public void Transfer(int id1, int id2, decimal sum);
    }
}