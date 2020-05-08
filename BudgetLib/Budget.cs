using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public class Budget<T> where T : Account
    {
        public event BudgetStateHandler FindAccountEvent;
        public event BudgetStateHandler ChooseAccountEvent;
        private List<T> _accounts = new List<T>();
        public string Name { get; private set; }
        public T CurrentObj { get; private set; }
    
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

        private void CallEvent(BudgetEventArgs e, BudgetStateHandler handler)
        {
            if (e != null)
            {
                handler?.Invoke(this,e);
            }
        }

        private void OnFindAccount(BudgetEventArgs e) => CallEvent(e, FindAccountEvent);
        private void OnChooseAccount(BudgetEventArgs e) => CallEvent(e, ChooseAccountEvent);
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
                        throw new ArgumentException("Sum on account type 'MIDDLE' must be less than 20000");
                    }
                    newAccount = new MiddleAccount(sum) as T;
                    break;
                case AccountType.Premium:
                    if (sum > 1000000)
                    {
                        throw new ArgumentException("Sum on account type 'PREMIUM' must be less than 1000000");
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

        public void CloseAccount()
        {
            if (CurrentObj == null)
            {
                OnChooseAccount(new BudgetEventArgs("Неможливо виконати операцію. Рахунок не вибраний"));
                throw new NullReferenceException("Not choosen account");
            }
            CurrentObj.Closed();
            bool flag = _accounts.Remove(CurrentObj);
            if (!flag)
            {
                throw new ArgumentOutOfRangeException("CurrentObj");
            }
        }
        public void CloseAccount(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                OnChooseAccount(new BudgetEventArgs("Неможливо виконати операцію. Рахунок не вибраний"));
                throw new NullReferenceException("Not choosen account");
            }
            account.Closed();
            bool flag = _accounts.Remove(account);
            if (!flag)
            {
                throw new ArgumentOutOfRangeException("CurrentObj");
            }
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

        public List<int> GetListAccountsId
        {
            get
            {
                List<int> accountsIds = new List<int>();
                foreach (var account in _accounts)
                {
                    accountsIds.Add(account.Id);
                }

                return accountsIds;
            }
        }

        public void ChooseCurrentAccount(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                OnChooseAccount(new BudgetEventArgs("Неможливо змінити рахунок. Такого рахунка не існує."));
                throw new NullReferenceException("Not choosen account");
            }

            CurrentObj = account;
            OnChooseAccount(new BudgetEventArgs($"Рахунок змінений на рахунок з id {account.Id}"));
        }
        public void Put(decimal sum)
        {
            if (CurrentObj == null)
            {
                OnFindAccount(new BudgetEventArgs("Поточний рахунок для здійснення операції не вибраний."));
                throw new NullReferenceException("Not choosen account");
            }

            CurrentObj.Put(sum);
        }

        public void Withdraw(decimal sum)
        {
            if (CurrentObj == null)
            {
                OnFindAccount(new BudgetEventArgs("Поточний рахунок для здійснення операції не вибраний."));
                throw new NullReferenceException("Not choosen account");
            }

            CurrentObj.Withdraw(sum);
        }
    }
}