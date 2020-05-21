using System;
using System.Collections.Generic;
using BudgetLib.Account;

namespace BudgetLib.Budget
{
    public partial class Budget<T> : IBudget<T> where T : Account.Account
    {
        public event BudgetStateHandler FindAccountEvent;
        
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
        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler, AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler,AccountStateHandler changeTypeHandler,AccountStateHandler accountInfo) // open new account
        {
            T newAccount = default(T);

            switch (type)
            {
                case AccountType.Small:
                    newAccount = new SmallAccount(sum) as T;
                    break;
                case AccountType.Middle:
                    newAccount = new MiddleAccount(sum) as T;
                    break;
                case AccountType.Premium:
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
            newAccount.ChangeAccountTypeEvent += changeTypeHandler;
            newAccount.AccountInfo += accountInfo;
            
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

        private static void ToHistory(Account.Account account,Account.Account.TypeHistoryEvent type,string message,Item item) // push to history of account
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
            account.GetAccountInfo();
        }

        public void ChangeTypeAccount(int id, AccountType type) // change type of account
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException("Not find account with id {id}");
            }
            
            account.ChangeTypeAccount(type);
            ToHistory(account, Account.Account.TypeHistoryEvent.GetMoney,
                $"<Changed account type {DateTime.Now}>", new Item("opening",0));
        }
    }
}