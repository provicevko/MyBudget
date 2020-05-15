namespace BudgetLib.Account
{
    internal interface IAccount
    {
        public void Put(decimal sum);
        public void Withdraw(decimal sum);
        public void Transfer(Account account, decimal sum);
        public int Id { get; }
        public decimal Sum { get; }
        
        private static int _idCounter = 0;
    }
}