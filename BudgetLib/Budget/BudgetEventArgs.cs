using System;

namespace BudgetLib
{
    public delegate void BudgetStateHandler(object sender, BudgetEventArgs e);
    public class BudgetEventArgs
    {
        public string Message { get; private set; }
        public int Id { get; internal set; }
        public AccountType Type { get; internal set; }
        public DateTime RegData { get; internal set; }
        public decimal Limit { get; internal set; }
        public decimal Sum { get;}

        public BudgetEventArgs(string message,decimal sum = 0)
        {
            Message = message;
            Sum = sum;
        }
    }
}