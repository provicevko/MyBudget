using System;

namespace BudgetLib
{
    public class Item
    {
        public string Comment { get;}
        public decimal Sum { get; }
        public Item(string comment, decimal sum)
        {
            if (comment.Length > 18 || comment.Replace(" ", "").Length == 0)
            {
                throw new ArgumentException("Keyword must be > 0 and <= 18");
            }
            Comment = comment;
            Sum = sum;
        }
    }
}