namespace BudgetLib
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs
    {
        public string Message { get; }
        public decimal Sum { get; }

        public AccountEventArgs(string message, decimal sum)
        {
            Message = message;
            Sum = sum;
        }
    }
}