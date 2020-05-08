using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public class Budget<T> where T : Account
    {
        private List<T> _accounts = new List<T>();
        public string Name { get; private set; }
        public T CurrentObj { get; set; }

        public Budget(string name)
        {
            if (name != null)
            {
                Name = name;
            }
            else
            {
                Name = "My budget";
            }

            CurrentObj = null;
        }
        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler, AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler, AccountStateHandler limitOverflowHandler)
        {
            T newAccount = null;

            switch (type)
            {
                case AccountType.Small:
                    if (sum > 1000)
                    {
                        throw new ArgumentException("Sum on account type 'SMALL' must be less than 100");
                    }
                    newAccount = new SmallAccount(sum) as T;
                    break;
                case AccountType.Middle:
                    if (sum > 10000)
                    {
                        throw new ArgumentException("Sum on account type 'SMALL' must be less than 20000");
                    }
                    newAccount = new MiddleAccount(sum) as T;
                    break;
                case AccountType.Premium:
                    if (sum > 1000000)
                    {
                        throw new ArgumentException("Sum on account type 'SMALL' must be less than 1000000");
                    }
                    newAccount = new PremiumAccount(sum) as T;
                    break;
            }

            if (newAccount == null)
            {
                throw new NullReferenceException("Unreal to create an account of choosen type.");
            }
            
            _accounts.Add(newAccount);

            newAccount.OpenEvent += openHandler;
            newAccount.CloseEvent += closeHandler;
            newAccount.PutEvent += putHandler;
            newAccount.WithdrawEvent += withdrawHandler;
            newAccount.TransferEvent += transferHandler;
            newAccount.LimitOverflowEvent += limitOverflowHandler;
            
            newAccount.Opened();
            newAccount.OpenEvent -= openHandler;
        }

        public T FindAccount(int id)
        {
            foreach (var instance in _accounts)
            {
                if (instance.Id == id)
                {
                    return instance;
                }
            }

            return null;
        }
        public void Put(decimal sum)
        {
            
        }
    }
}