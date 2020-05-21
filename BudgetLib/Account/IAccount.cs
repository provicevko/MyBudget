using BudgetLib.Budget;

namespace BudgetLib.Account
{
    internal interface IAccount
    {
        public void Put(Item item);
        public void Withdraw(Item item);
        public void Transfer(Account account, Item item);
        public int Id { get; }
        public decimal Sum { get; }
        
        private static int _idCounter = 0;
    }
}