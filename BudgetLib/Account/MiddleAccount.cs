namespace BudgetLib
{
    public class MiddleAccount : Account
    {
        public MiddleAccount(decimal sum) : base(sum, 20000)
        {
            Type = AccountType.Middle;
        }
    }
}