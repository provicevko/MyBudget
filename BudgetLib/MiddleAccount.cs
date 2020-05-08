namespace BudgetLib
{
    public class MiddleAccount : Account
    {
        public MiddleAccount(decimal sum) : base(sum, 20000)
        {
            Type = "MIDDLE";
        }
        
        protected internal override void Opened()
        {
            base.OnOpened(new AccountEventArgs($"Відкрито новий рахунок типу {Type}. Ідентифікатор рахунку: {Id}",Sum));
        }
    }
}