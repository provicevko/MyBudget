namespace BudgetLib
{
    public interface IAccount
    {
        bool Put(decimal sum);
        bool Withdraw(decimal sum);
        bool Transfer(Account account,decimal sum);
    }
}