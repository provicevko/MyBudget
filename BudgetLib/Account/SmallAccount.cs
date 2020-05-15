namespace BudgetLib
{
    public class SmallAccount : Account
    {
        public SmallAccount(decimal sum) : base(sum, 1000)
        {
            Type =AccountType.Small;
        }
    }
}