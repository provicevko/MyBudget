namespace BudgetLib.Account
{
    public class MiddleAccount : Account
    {
        public MiddleAccount(decimal sum) : base(sum, (decimal)AccountType.Middle)
        {
            Type = AccountType.Middle;
        }
    }
}