namespace BudgetLib
{
    public delegate void BudgetStateHandler(object sender, BudgetEventArgs e);
    public class BudgetEventArgs
    {
        public string Message { get; private set; }

        public BudgetEventArgs(string message)
        {
            Message = message;
        }
    }
}