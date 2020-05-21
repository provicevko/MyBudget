using System;

namespace BudgetLib.Account
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs // info object for events
    {
        public string Message { get; }
        public int Id { get; internal set; }
        public AccountType Type { get; internal set; }
        public DateTime DataTime { get; internal set; }
        public decimal Limit { get; internal set; }
        public decimal Sum { get;}

        public AccountEventArgs(string message,decimal sum = 0)
        {
            Message = message;
            Sum = sum;
        }
    }
}