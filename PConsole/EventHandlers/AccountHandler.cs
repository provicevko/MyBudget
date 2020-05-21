using System;
using BudgetLib.Account;

namespace PConsole.EventHandlers
{
    public static class AccountHandler
    {
        internal static void OpenHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void CloseHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void PuHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void WithdrawHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void TransferHandler(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        internal static void ChangeTypeAccount(object sender, AccountEventArgs e) => Console.WriteLine(e.Message);
        
        internal static void AccountInfoHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine($"{e.Message}\n\tId: {e.Id}\n\tType: {e.Type}\n\tLimit: {e.Limit}\n\tSum: {e.Sum}\n\tRegister data: {e.DataTime}");   
        }

    }
}