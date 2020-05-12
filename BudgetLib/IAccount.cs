namespace BudgetLib
{
    internal interface IAccount
    {
        public void Put(decimal sum);
        public void Withdraw(decimal sum);
        public void Transfer(Account account, decimal sum);
    }
}