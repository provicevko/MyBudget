namespace BudgetLib
{
    public class SmallAccount : Account
    {
        public SmallAccount(decimal sum) : base(sum, 1000)
        {
            Type = "SMALL";
        }

        protected internal override void Opened()
        {
            base.OnOpened(new BugetEventArgs($"Відкрито новий рахунок типу {Type}. Ідентифікатор рахунку: {Id}",Sum));
        }
    }
}