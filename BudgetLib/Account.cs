using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public abstract class Account : IAccount
    {
        protected internal event AccountStateHandler OpenEvent;
        protected internal event AccountStateHandler CloseEvent;
        protected internal event AccountStateHandler PutEvent;
        protected internal event AccountStateHandler WithdrawEvent;
        protected internal event AccountStateHandler TransferEvent;
        protected internal event AccountStateHandler LimitOverflowEvent;
        public decimal Sum { get; private set; }
        public decimal Limit { get; protected set; }
        public int Id { get; }
        public string Type { get; protected set; }

        private static int _idCounter = 0;
        
        public Account(decimal sum, decimal limit)
        {
            Sum = sum;
            Limit = limit;
            Id = ++_idCounter;
        }

        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
            {
                handler?.Invoke(this, e);
            }
        }

        protected virtual void OnOpened(AccountEventArgs e) => CallEvent(e, OpenEvent);
        protected virtual void OnClosed(AccountEventArgs e) => CallEvent(e, CloseEvent);
        protected virtual void OnPut(AccountEventArgs e) => CallEvent(e, PutEvent);
        protected virtual void OnWithdrawed(AccountEventArgs e) => CallEvent(e, WithdrawEvent);
        protected virtual void OnTransfer(AccountEventArgs e) => CallEvent(e, TransferEvent);
        protected virtual void OnLimitOverflow(AccountEventArgs e) => CallEvent(e, LimitOverflowEvent);

        protected internal virtual void Opened() =>
            OnOpened(new AccountEventArgs($"Відкритий новий рахунок. Ідентифікатор рахунку: {Id}", Sum));

        protected internal virtual void Closed() =>
            OnClosed(new AccountEventArgs($"Рахунок закритий. Ідентифікатор рахунку: {Id}", Sum));

        public virtual void Put(decimal sum)
        {
            if (sum > 0)
            {
                if (Sum + sum > Limit)
                {
                    string message =
                        $"Не можливо покласти на рахунок: сума перевищує ліміт рахунку ({Limit}).\nЗменшіть суму, або змініть тип рахунку.";
                    OnLimitOverflow(new AccountEventArgs(message, Limit));
                    return;
                }

                Sum += sum;
                OnPut(new AccountEventArgs($"Рахунок успішно поповнений на {sum} грн.", sum));
            }
            else
            {
                OnPut(new AccountEventArgs($"Неможливо поповнити на {sum} грн.", sum));
                // throw new ArgumentException("sum must be more than 0");
            }
        }

        public virtual decimal Withdraw(decimal sum)
        {
            if (sum > Sum)
            {
                OnWithdrawed(new AccountEventArgs("Недостатньо коштів на рахунку. Поточний баланс: {Sum} грн.", 0));
                return 0;
            }

            Sum -= sum;
            OnWithdrawed(new AccountEventArgs($"З рахунку знято {sum} грн.", sum));
            return sum;
        }

        public virtual void Transfer(Account account, decimal sum)
        {
            if (Sum > sum)
            {
                if (account.Sum + sum < account.Limit)
                {
                    Sum -= sum;
                    account.Sum += sum;
                    OnTransfer(new AccountEventArgs($"Переведено на рахунок з id {account.Id} - {sum} грн.", sum));
                }
                else
                {
                    OnTransfer(new AccountEventArgs($"Неможливо перевести на рахунок з id {account.Id}. Сумма перевищує ліміт рахунку", 0));
                }
            }
            else
            {
                OnTransfer(new AccountEventArgs("Недостатньо коштів на даному рахунку для переводу.", 0));
            }
        }
    }
}
        
