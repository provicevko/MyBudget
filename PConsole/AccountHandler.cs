using System;
using BudgetLib;

namespace PConsole
{
    public static class AccountHandler
    {
        internal static void OpenHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void CloseHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void PuHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void WithdrawHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void TransferHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
    }
}