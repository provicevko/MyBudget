namespace BudgetLib.Account
{
    public sealed class PremiumAccount : Account
    {
        public PremiumAccount(decimal sum) : base(sum, (decimal)AccountType.Premium)
        {
            Type = AccountType.Premium;
        }
    }
}