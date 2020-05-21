namespace BudgetLib.Account
{
    public sealed class SmallAccount : Account
    {
        public SmallAccount(decimal sum) : base(sum, (decimal)AccountType.Small)
        {
            Type = AccountType.Small;
        }
    }
}