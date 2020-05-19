using System;
using System.Collections.Generic;
using BudgetLib.Account;

namespace BudgetLib.Budget
{
    public partial class Budget<T> : IBudget<T> where T : Account.Account
    {
        public event BudgetStateHandler FindAccountEvent;
        public event BudgetStateHandler AccountInfo;
        public event BudgetStateHandler OpenAccountEvent;
        public event BudgetStateHandler ChangeTypeAccountEvent;
        private List<T> _accounts = new List<T>(); // all accounts
        public string Name { get;} // name of budget
        public Budget(string name)
        {
            Name = name;
        }

        private void CallEvent(BudgetEventArgs e, BudgetStateHandler handler)
        {
            if (e != null)
            {
                handler?.Invoke(this,e);
            }
        }

        private void OnFindAccount(BudgetEventArgs e) => CallEvent(e, FindAccountEvent);
        private void OnAccountInfo(BudgetEventArgs e) => CallEvent(e, AccountInfo);
        private void OnOpenAccount(BudgetEventArgs e) => CallEvent(e, OpenAccountEvent);
        private void OnChangeType(BudgetEventArgs e) => CallEvent(e, ChangeTypeAccountEvent);
        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler, AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler) // open new account
        {
            if (sum < 0)
            {
                OnOpenAccount(new BudgetEventArgs("The sum of money must be greater than or equal to 0."));
                throw new ArgumentException("In order to open account 'sum' must be >= 0");
            }
            T newAccount = default(T);

            switch (type)
            {
                case AccountType.Small:
                    if (sum > 1000)
                    {
                        OnOpenAccount(new BudgetEventArgs("For an account of type 'SMALL', the sum of money must be less than or equal to 1000 UAH."));
                        throw new ArgumentException("Sum on account type 'SMALL' must be less than 1,000");
                    }
                    newAccount = new SmallAccount(sum) as T;
                    break;
                case AccountType.Middle:
                    if (sum > 20000)
                    {
                        OnOpenAccount(new BudgetEventArgs("For an account of type 'MIDDLE', the sum of money must be less than or equal to 20000 UAH."));
                        throw new ArgumentException("Sum on account type 'MIDDLE' must be less than 20,000");
                    }
                    newAccount = new MiddleAccount(sum) as T;
                    break;
                case AccountType.Premium:
                    if (sum > 1000000)
                    {
                        OnOpenAccount(new BudgetEventArgs("For an account of type 'PREMIUM', the sum of money must be less than or equal to 1000000 UAH."));
                        throw new ArgumentException("Sum on account type 'PREMIUM' must be less than 1,000,000");
                    }
                    newAccount = new PremiumAccount(sum) as T;
                    break;
            }

            if (newAccount == null)
            {
                throw new NullReferenceException("Unreal to create an account of chosen type. Account is null object");
            }
            
            _accounts.Add(newAccount);

            newAccount.OpenEvent += openHandler;
            newAccount.CloseEvent += closeHandler;
            newAccount.PutEvent += putHandler;
            newAccount.WithdrawEvent += withdrawHandler;
            newAccount.TransferEvent += transferHandler;
            
            OnOpenAccount(new BudgetEventArgs("Operation was successfully completed."));
            newAccount.Opened();
            newAccount.OpenEvent -= openHandler;
            ToHistory(newAccount, Account.Account.TypeHistoryEvent.GetMoney,
                $"<Received (at the opening) {DateTime.Now}>", new Item("opening",sum));
        }
        
        public void CloseAccount(int id) // close exist account
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            account.Closed();
            _accounts.Remove(account);
        }

        public T FindAccount(int id) // find account with id
        {
            foreach (var instance in _accounts)
            {
                if (instance.Id == id)
                {
                    return instance;
                }
            }
            OnFindAccount(new BudgetEventArgs($"Account with id {id} not found."));
            return null;
        }

        public List<int> GetListAccountsId // get ids list
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

        private void ToHistory(Account.Account account,Account.Account.TypeHistoryEvent type,string message,Item item) // push to history of account
        {
            Account.Account.HistoryStruct historyStruct;
            historyStruct.Type = type;
            historyStruct.Message = message;
            historyStruct.Item = item;
            account._historyAccount.HistoryList.Add(historyStruct);
        }

        public Account.Account.HistoryAccount HistoryInfo(int id) // get accounts' history info 
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            
            return account._historyAccount;
        }
        public void GetAccountInfo(int id) // get info about account
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            BudgetEventArgs info = new BudgetEventArgs($"Information for an account with id {id}:",account.Sum){Id = id,Type = account.Type,Limit = account.Limit,DataTime = account.RegData};
            OnAccountInfo(info);
        }

        public void ChangeTypeAccount(int id, AccountType type) // change type of account
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException("Not find account with id {id}");
            }

            if (account.Sum > (decimal) type)
            {
                OnChangeType(new BudgetEventArgs("Sum of money more than new accounts'LIMIT."));
                throw new ArgumentException("Sum of money more than new accounts'LIMIT");
            }
            account.Type = type;
            account.Limit = (decimal) type;
            OnChangeType(new BudgetEventArgs($"Account type (id {id}) changed to {type.ToString (). ToUpper ()}"));
            ToHistory(account, Account.Account.TypeHistoryEvent.GetMoney,
                $"<Changed account type {DateTime.Now}>", new Item("opening",0));
        }
    }
}