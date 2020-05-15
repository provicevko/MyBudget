namespace BudgetLib
{
    public class PremiumAccount : Account
    {
        public PremiumAccount(decimal sum) : base(sum, 1000000)
        {
            Type = AccountType.Premium;
        }
    }
}