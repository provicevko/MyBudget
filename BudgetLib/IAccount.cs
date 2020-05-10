namespace BudgetLib
{
    internal interface IAccount
    {
        public bool Put(decimal sum);
        public bool Withdraw(decimal sum);
        public bool Transfer(Account account,decimal sum);
    }
}