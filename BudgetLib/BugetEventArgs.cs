namespace BudgetLib
{
    public delegate void BudgetStateHandler(object sender, BugetEventArgs e);
    public class BugetEventArgs
    {
        public string Message { get; }
        public decimal Sum { get; }

        public BugetEventArgs(string message, decimal sum)
        {
            Message = message;
            Sum = sum;
        }
    }
}