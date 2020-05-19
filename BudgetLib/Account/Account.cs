using System;
using System.Collections.Generic;
using BudgetLib.Budget;

namespace BudgetLib.Account
{
    public abstract class Account : IAccount 
    {
        protected internal event AccountStateHandler OpenEvent;
        protected internal event AccountStateHandler CloseEvent;
        protected internal event AccountStateHandler PutEvent;
        protected internal event AccountStateHandler WithdrawEvent;
        protected internal event AccountStateHandler TransferEvent;
        public decimal Sum { get; private set; } // sum of money
        public decimal Limit { get; protected internal set; } // limit money
        public int Id { get; } // accounts' id
        public AccountType Type { get; protected internal set; } // type account
        public DateTime RegData { get;} // register time

        private static int _idCounter = 0;

        protected internal Account(decimal sum, decimal limit)
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
            public Item Item;
        }
        protected internal HistoryAccount _historyAccount;

        public class HistoryAccount // history operations of account
        {
            public List<HistoryStruct> HistoryList { get; protected internal set;}
            protected internal HistoryAccount()
            {
                HistoryList = new List<HistoryStruct>();
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

        protected internal virtual void Opened() => // open account
            OnOpened(new AccountEventArgs($"A new account of type {Type} has been opened. Account ID: {Id}."));

        protected internal virtual void Closed() // close account
        {
            if (Sum > 0)
            {
                OnClosed(new AccountEventArgs($"The account (id {Id}) can't be closed. Sum of money must be 0."));
                throw new ArgumentOutOfRangeException("Sum");
            }
            OnClosed(new AccountEventArgs($"The account of type {Type} is closed. Account ID: {Id}."));
        }
            

        public virtual void Put(decimal sum) // put money to the account
        {
            if (sum > 0)
            {
                if (Sum + sum > Limit)
                {
                    string message =
                        $"It is not possible to put on the account (the sum of money exceeds the account limit ({Limit} UAH)).\nReduce the sum of money, or change the account type.";
                    OnPut(new AccountEventArgs(message));
                    throw new ArgumentException("Result sum of money more then limit of account");
                }

                Sum += sum;
                OnPut(new AccountEventArgs($"The account was successfully replenished with {sum} UAH."));
            }
            else
            {
                OnPut(new AccountEventArgs($"Unable to replenish account for {sum} UAH."));
                throw new ArgumentException("Unreal to replenish account. Sum of money on this account <= 0");
            }
        }

        public virtual void Withdraw(decimal sum) // withdraw money from account
        {
            if (sum <= 0)
            {
                OnWithdrawed(new AccountEventArgs("It is impossible to withdraw less than 1 UAH."));
                throw new ArgumentException("Parametr 'sum' must be more than 0");
            }
            
            if (sum > Sum)
            {
                OnWithdrawed(new AccountEventArgs($"Insufficient UAH in the account. Current balance: {Sum} UAH."));
                throw new ArgumentException("Not enough money in this account");
            }

            Sum -= sum;
            OnWithdrawed(new AccountEventArgs($"{sum} UAH was withdrawn from the account."));
        }

        public virtual void Transfer(Account account, decimal sum) // transfer money to other account
        {
            if (sum <= 0)
            {
                OnWithdrawed(new AccountEventArgs("It is impossible to transfer less than 1 UAH."));
                throw new ArgumentException("Parametr 'sum' must be more than 0");
            }
            
            if (Sum >= sum)
            {
                if (account.Sum + sum < account.Limit)
                {
                    Sum -= sum;
                    account.Sum += sum;
                    OnTransfer(new AccountEventArgs($"Transferred to another account (id <{Id}> -> id <{account.Id}>) with {sum} UAH."));
                }
                else
                {
                    OnTransfer(new AccountEventArgs($"Unable to transfer to account with id {account.Id}. The sum of money exceeds the account limit."));
                    throw new ArgumentException("Sum more than limit of account");
                }
            }
            else
            {
                OnTransfer(new AccountEventArgs("There are not enough money in this account to transfer."));
                throw new ArgumentException($"Not enough money for transfer operation. Sum: {sum}");
            }
        }
    }
}
        
