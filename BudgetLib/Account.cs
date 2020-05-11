using System;
using System.Collections;
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
        public decimal Sum { get; private set; }
        public decimal Limit { get; protected set; }
        public int Id { get; }
        public string Type { get; protected internal set; }
        public DateTime RegData { get;}

        private static int _idCounter = 0;
        
        public Account(decimal sum, decimal limit)
        {
            Sum = sum;
            Limit = limit;
            Id = ++_idCounter;
            RegData = DateTime.Now;
            _historyAccount = new HistoryAccount();
        }

        public enum TypeHistoryEvent
        {
            GetMoney,
            GivenMoney
        }

        public struct HistoryStruct
        {
            public string Message;
            public TypeHistoryEvent Type;
            public decimal Sum;
        }
        protected internal HistoryAccount _historyAccount;

        public class HistoryAccount
        {
            public List<HistoryStruct> historyList { get; protected internal set;}
            protected internal HistoryAccount()
            {
                historyList = new List<HistoryStruct>();
            }
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

        protected internal virtual void Opened() =>
            OnOpened(new AccountEventArgs($"Відкритий новий рахунок. Ідентифікатор рахунку: {Id}", Sum));

        protected internal virtual void Closed() =>
            OnClosed(new AccountEventArgs($"Рахунок закритий. Ідентифікатор рахунку: {Id}", Sum));

        public virtual bool Put(decimal sum)
        {
            if (sum > 0)
            {
                if (Sum + sum > Limit)
                {
                    string message =
                        $"Не можливо покласти на рахунок: сума перевищує ліміт рахунку ({Limit}).\nЗменшіть суму, або змініть тип рахунку.";
                    OnPut(new AccountEventArgs(message, Limit));
                    return false;
                }

                Sum += sum;
                OnPut(new AccountEventArgs($"Рахунок успішно поповнений на {sum} грн.", sum));
                return true;
            }
            else
            {
                OnPut(new AccountEventArgs($"Неможливо поповнити на {sum} грн.", sum));
                return false;
            }
        }

        public virtual bool Withdraw(decimal sum)
        {
            if (sum == 0)
            {
                OnWithdrawed(new AccountEventArgs("Неможливо зняти 0 грн.", 0));
                throw new ArgumentException("Parametr 'sum' must be more than 0.");
            }
            
            if (sum > Sum)
            {
                OnWithdrawed(new AccountEventArgs($"Недостатньо коштів на рахунку. Поточний баланс: {Sum} грн.", 0));
                throw new ArgumentException("not enough money in this account");
            }

            Sum -= sum;
            OnWithdrawed(new AccountEventArgs($"З рахунку знято {sum} грн.", sum));
            return true;
        }

        public virtual bool Transfer(Account account, decimal sum)
        {
            if (Sum >= sum)
            {
                if (account.Sum + sum < account.Limit)
                {
                    Sum -= sum;
                    account.Sum += sum;
                    OnTransfer(new AccountEventArgs($"Переведено на рахунок з id {account.Id} - {sum} грн.", sum));
                    return true;
                }
                else
                {
                    OnTransfer(new AccountEventArgs($"Неможливо перевести на рахунок з id {account.Id}. Сумма перевищує ліміт рахунку", 0));
                    return false;
                }
            }
            else
            {
                OnTransfer(new AccountEventArgs("Недостатньо коштів на даному рахунку для переводу.", 0));
                return false;
            }
        }
    }
}
        
