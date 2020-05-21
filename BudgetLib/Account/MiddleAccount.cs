namespace BudgetLib.Account
{
    public sealed class MiddleAccount : Account
    {
        public MiddleAccount(decimal sum) : base(sum, (decimal)AccountType.Middle)
        {
            Type = AccountType.Middle;
        }
    }
}