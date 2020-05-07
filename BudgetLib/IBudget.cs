namespace BudgetLib
{
    public interface IBudget
    {
        void Put(decimal sum);
        decimal Withdraw(decimal sum);
        void Transfer(int id,decimal sum);
    }
}