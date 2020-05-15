namespace BudgetLib
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs
    {
        public string Message { get; }

        public AccountEventArgs(string message)
        {
            Message = message;
        }
    }
}