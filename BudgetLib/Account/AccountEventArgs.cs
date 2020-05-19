namespace BudgetLib.Account
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs // info object for events
    {
        public string Message { get; }

        public AccountEventArgs(string message)
        {
            Message = message;
        }
    }
}