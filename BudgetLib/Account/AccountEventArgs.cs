using System;

namespace BudgetLib.Account
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs // info object for events
    {
        public string Message { get; }
        public DateTime DataTime { get; }
        public decimal Sum { get;}

        public AccountEventArgs(string message, DateTime time, decimal sum = 0)
        {
            Message = message;
            Sum = sum;
            DataTime = time;
        }
    }
}