using System;
using BudgetLib.Account;

namespace PConsole.EventHandlers
{
    public static class AccountHandler
    {
        internal static void OpenHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);
        internal static void CloseHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);
        internal static void PuHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);
        internal static void WithdrawHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);
        internal static void TransferHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);
        internal static void ChangeTypeAccount(object sender, AccountEventArgs e) => Console.WriteLine(e.Message + "\t" + e.DataTime);

    }
}