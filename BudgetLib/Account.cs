using System;

namespace BudgetLib
{
    public abstract class Account : IAccount
    {
        protected internal event BudgetStateHandler OpenEvent;
        protected internal event BudgetStateHandler CloseEvent;
        protected internal event BudgetStateHandler PutEvent;
        protected internal event BudgetStateHandler WithdrawEvent;
        protected internal event BudgetStateHandler TransferEvent;
        protected internal event BudgetStateHandler LimitOverflowEvent;
        public decimal Sum { get; private set; }
        public decimal Limit { get; protected set; }
        public int Id { get;}
        public string Type { get; protected set; }
        private static int _idCounter = 0;
        
        public Account(decimal sum, decimal limit)
        {
            Sum = sum;
            Limit = limit;
            Id=++_idCounter;
        }

        private void CallEvent(BugetEventArgs e,BudgetStateHandler handler)
        {
            if (e != null)
            {
                handler?.Invoke(this,e);
            }
        }

        protected virtual void OnOpened(BugetEventArgs e) => CallEvent(e,OpenEvent);
        protected virtual void OnClosed(BugetEventArgs e) => CallEvent(e,CloseEvent);
        protected virtual void OnPut(BugetEventArgs e) => CallEvent(e,PutEvent);
        protected virtual void OnWithdrawed(BugetEventArgs e) => CallEvent(e,WithdrawEvent);
        protected virtual void OnTransfer(BugetEventArgs e) => CallEvent(e,TransferEvent);
        protected virtual void OnLimitOverflow(BugetEventArgs e) => CallEvent(e,LimitOverflowEvent);

        protected internal virtual void Opened() => OnOpened(new BugetEventArgs($"Відкритий новий рахунок. Ідентифікатор рахунку: {Id}",Sum) );
        protected internal virtual void OnClosed() => OnClosed(new BugetEventArgs($"Рахунок закритий. Ідентифікатор рахунку: {Id}",Sum) );
        
        public virtual void Put(decimal sum)
        {
            if (Sum + sum > Limit)
            {
                string message = $"Не можливо покласти на рахунок: сума перевищує ліміт рахунку ({Limit}).\nЗменшіть суму, або змініть тип рахунку.";
                OnLimitOverflow(new BugetEventArgs(message,Limit));
                return;
            }

            Sum += sum;
            OnPut(new BugetEventArgs($"Рахунок поповнений на {sum} грн.",sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            if (sum > Sum)
            {
                OnWithdrawed(new BugetEventArgs("Недостатньо коштів на рахунку. Поточний баланс: {Sum} грн.",0));
                return 0;
            }

            Sum -= sum;
            OnWithdrawed(new BugetEventArgs($"З рахунку знято {sum} грн.",sum));
            return sum;
        }

        public virtual void Transfer(Account account, decimal sum)
        {
            if (account != null)
            {
                if (Sum > sum)
                {
                    if (account.Sum + sum < account.Limit)
                    {
                        Sum -= sum;
                        account.Sum += sum;
                        OnTransfer(new BugetEventArgs($"Переведено на рахунок з id {account.Id}.",sum));
                    }
                    else
                    {
                        OnTransfer(new BugetEventArgs($"Неможливо перевести на рахунок з id {account.Id}.",0));
                    }
                }
                else
                {
                    OnTransfer(new BugetEventArgs("Недостатньо коштів на даному рахунку для переводу.",0));
                }
            }
            else
            {
                OnTransfer(new BugetEventArgs("Неможливо знайти рахунок для здійснення операції переводу.",0));
            }
        }
        
    }
}