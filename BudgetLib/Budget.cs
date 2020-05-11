using System;
using System.Collections.Generic;

namespace BudgetLib
{
    public partial class Budget<T> where T : Account
    {
        public event BudgetStateHandler FindAccountEvent;
        public event BudgetStateHandler AccountInfo;
        public event BudgetStateHandler ErrorAlert;
        private List<T> _accounts = new List<T>();
        public string Name { get; private set; }
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
        private void OnErrorAlert(BudgetEventArgs e) => CallEvent(e, ErrorAlert);
        public void OpenAccount(AccountType type, decimal sum, AccountStateHandler openHandler, AccountStateHandler closeHandler, AccountStateHandler putHandler,
            AccountStateHandler withdrawHandler, AccountStateHandler transferHandler, AccountStateHandler limitOverflowHandler)
        {
            if (sum < 0)
            {
                OnErrorAlert(new BudgetEventArgs("Сума повина бути більше 0."));
                throw new ArgumentException("Sum must be >= 0");
            }
            T newAccount = default(T);

            switch (type)
            {
                case AccountType.Small:
                    if (sum > 1000)
                    {
                        OnErrorAlert(new BudgetEventArgs("Для рахунку типу 'SMALL' сума повинна бути менша або рівна 1000 грн."));
                        throw new ArgumentException("Sum on account type 'SMALL' must be less than 1000");
                    }
                    newAccount = new SmallAccount(sum) as T;
                    break;
                case AccountType.Middle:
                    if (sum > 10000)
                    {
                        OnErrorAlert(new BudgetEventArgs("Для рахунку типу 'MIDDLE' сума повинна бути менша або рівна 20000 грн."));
                        throw new ArgumentException("Sum on account type 'MIDDLE' must be less than 20000");
                    }
                    newAccount = new MiddleAccount(sum) as T;
                    break;
                case AccountType.Premium:
                    if (sum > 1000000)
                    {
                        OnErrorAlert(new BudgetEventArgs("Для рахунку типу 'PREMIUM' сума повинна бути менша або рівна 1000000 грн."));
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

            newAccount.Opened();
            newAccount.OpenEvent -= openHandler;
        }
        
        public void CloseAccount(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            account.Closed();
            bool flag = _accounts.Remove(account);
            if (!flag)
            {
                throw new ArgumentOutOfRangeException($"Account (id {id})wasn't remove from budget");
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
            OnFindAccount(new BudgetEventArgs($"Рахунок з id {id} не знайдений."));
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

        private void ToHistory(Account account,Account.TypeHistoryEvent type,string message,decimal sum)
        {
            Account.HistoryStruct historyStruct;
            historyStruct.Type = type;
            historyStruct.Message = message;
            historyStruct.Sum = sum;
            account._historyAccount.historyList.Add(historyStruct);
        }

        public Account.HistoryAccount HistoryInfo(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            
            return account._historyAccount;
        }
        public void GetAccountInfo(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException($"Unreal find account with id {id}");
            }
            BudgetEventArgs info = new BudgetEventArgs($"Інформація про рахунок з id {id}:",account.Sum){Id = id,Type = account.Type,Limit = account.Limit,RegData = account.RegData};
            OnAccountInfo(info);
        }

        // public void ChangeTypeAccount(int id, AccountType type)
        // {
        //     T account = FindAccount(id);
        //     if (account == null)
        //     {
        //         OnFindAccount(new BudgetEventArgs("Неможливо знайти рахунок. Такого рахунка не існує."));
        //         throw new NullReferenceException("Not choosen account");
        //     }
        //
        //     account.Type = $"{type}";
        //     OnAccountInfo(new BudgetEventArgs($"Тип рахунку (id {id}) змінений на {type}"));
        // }
    }
}