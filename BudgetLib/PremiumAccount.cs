namespace BudgetLib
{
    public class PremiumAccount : Account
    {
        public PremiumAccount(decimal sum) : base(sum, 1000000)
        {
            Type = "PREMIUM";
        }

        protected internal override void Opened()
        {
            base.OnOpened(new AccountEventArgs($"Відкрито новий рахунок типу {Type}. Ідентифікатор рахунку: {Id}"));
        }
        
    }
}