namespace BudgetLib
{
    public interface IAccount
    {
        void Put(decimal sum);
        decimal Withdraw(decimal sum);
        void Transfer(Account account,decimal sum);
    }
}