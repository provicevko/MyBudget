namespace BudgetLib.Budget
{
    public delegate void BudgetStateHandler(object sender, BudgetEventArgs e);
    public class BudgetEventArgs // info object for events
    {
        public string Message { get; }
        public BudgetEventArgs(string message)
        {
            Message = message;
        }
    }
}