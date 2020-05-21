using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected internal event AccountStateHandler ChangeAccountTypeEvent;
        public decimal Sum { get; private set; } // sum of money
        public decimal Limit { get; private set; } // limit money
        public int Id { get; } // accounts' id
        public AccountType Type { get; protected set; } // type account
        public DateTime RegData { get;} // register time

        protected List<HistoryStruct> _HistoryList; // history of account

        public ReadOnlyCollection<HistoryStruct> HistoryList => _HistoryList.AsReadOnly();

        private static int _idCounter = 0;

        protected internal Account(decimal sum, decimal limit)
        {
            if (sum < 0 || sum > limit)
            { 
                throw new ArgumentException($"In order to open account 'sum = {sum}' must be >= 0 and <= {limit}");
            }
            Sum = sum;
            Limit = limit;
            Id = ++_idCounter;
            RegData = DateTime.Now;
            _HistoryList = new List<HistoryStruct>();
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
            public DateTime Time;
            public Item Item;
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
        protected virtual void OnChangeType(AccountEventArgs e) => CallEvent(e, ChangeAccountTypeEvent);


        protected internal virtual void Opened(Item item) // open account
        {
            DateTime time = DateTime.Now;
            ToHistory(this, item, "Received (on opening)", TypeHistoryEvent.GetMoney, time);
            OnOpened(new AccountEventArgs($"A new account of type {Type} has been opened. Account ID: {Id}.",time, item.Sum));
        }

        protected internal virtual void Closed() // close account
        {
            if (Sum > 0)
            {
                OnClosed(new AccountEventArgs($"The account (id {Id}) can't be closed. Sum of money must be 0.",DateTime.Now));
                throw new ArgumentOutOfRangeException("Sum");
            }
            OnClosed(new AccountEventArgs($"The account of type {Type} is closed. Account ID: {Id}.",DateTime.Now));
        }
            

        public virtual void Put(Item item) // put money to the account
        {
            if (item.Sum > 0)
            {
                if (Sum + item.Sum > Limit)
                {
                    string message =
                        $"It is not possible to put on the account (the sum of money exceeds the account limit ({Limit} UAH)).\nReduce the sum of money, or change the account type.";
                    OnPut(new AccountEventArgs(message,DateTime.Now));
                    throw new ArgumentException("Result sum of money more then limit of account");
                }

                Sum += item.Sum;
                
                DateTime time = DateTime.Now;
                ToHistory(this,item,"Received ",TypeHistoryEvent.GetMoney,time);
                OnPut(new AccountEventArgs($"The account was successfully replenished with {item.Sum} UAH.",time,item.Sum));
            }
            else
            {
                OnPut(new AccountEventArgs($"Unable to replenish account for {item.Sum} UAH.",DateTime.Now));
                throw new ArgumentException("Unreal to replenish account. Sum of money on this account <= 0");
            }
        }

        public virtual void Withdraw(Item item) // withdraw money from account
        {
            if (item.Sum <= 0)
            {
                OnWithdrawed(new AccountEventArgs("It is impossible to withdraw less than 1 UAH.",DateTime.Now));
                throw new ArgumentException("Parametr 'sum' must be more than 0");
            }
            
            if (item.Sum > Sum)
            {
                OnWithdrawed(new AccountEventArgs($"Insufficient UAH in the account. Current balance: {Sum} UAH.",DateTime.Now));
                throw new ArgumentException("Not enough money in this account");
            }

            Sum -= item.Sum;
            
            DateTime time = DateTime.Now;
            ToHistory(this,item,"Withdrawn ",TypeHistoryEvent.GivenMoney,time);
            OnWithdrawed(new AccountEventArgs($"{item.Sum} UAH was withdrawn from the account.",time,item.Sum));
        }

        public virtual void Transfer(Account account, Item item) // transfer money to other account
        {
            if (item.Sum <= 0)
            {
                OnTransfer(new AccountEventArgs("It is impossible to transfer less than 1 UAH.",DateTime.Now));
                throw new ArgumentException("Parametr 'sum' must be more than 0");
            }
            
            if (Sum >= item.Sum)
            {
                if (account.Sum + item.Sum < account.Limit)
                {
                    Sum -= item.Sum;
                    account.Sum += item.Sum;
                    
                    DateTime time = DateTime.Now;
                    ToHistory(this,item,$"Transferred to account (id {account.Id}) ",TypeHistoryEvent.GivenMoney,time);
                    ToHistory(account,item,$"Received by transfer from account (id {Id}) ",TypeHistoryEvent.GetMoney,time);
                    OnTransfer(new AccountEventArgs($"Transferred to another account (id <{Id}> -> id <{account.Id}>) with {item.Sum} UAH.",time,item.Sum));
                }
                else
                {
                    OnTransfer(new AccountEventArgs($"Unable to transfer to account with id {account.Id}. The sum of money exceeds the account limit.",DateTime.Now));
                    throw new ArgumentException("Sum more than limit of account");
                }
            }
            else
            {
                OnTransfer(new AccountEventArgs("There are not enough money in this account to transfer.",DateTime.Now));
                throw new ArgumentException($"Not enough money for transfer operation. Sum: {item.Sum}");
            }
        }

        private void ToHistory(Account account,Item item, string message, TypeHistoryEvent type, DateTime time)
        {
            HistoryStruct hst;
            hst.Item = item;
            hst.Message = message;
            hst.Type = type;
            hst.Time = time;
            account._HistoryList.Add(hst);
        }
        public void ChangeTypeAccount(AccountType type)
        {
            if (Sum > (decimal) type)
            {
                OnChangeType(new AccountEventArgs("Sum of money more than new accounts'LIMIT.",DateTime.Now));
                throw new ArgumentException("Sum of money more than new accounts'LIMIT");
            }

            if (Type == type)
            {
                OnChangeType(new AccountEventArgs("The new type is equal to the current",DateTime.Now));
                throw new ArgumentException($"The new type is equal to the current (id {Id})");
            }
            Type = type;
            Limit = (decimal) type;
            OnChangeType(new AccountEventArgs($"Account type (id {Id}) changed to {type.ToString (). ToUpper ()}",DateTime.Now));
        }

        public Tuple<string,int,AccountType,decimal,decimal,DateTime> GetAccountInfo() // get info about account
        {
            return Tuple.Create($"Information for an account with id {Id}",Id,Type,Limit,Sum,RegData);
        }
    }
}
        
