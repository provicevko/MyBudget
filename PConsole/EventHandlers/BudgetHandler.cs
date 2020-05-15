using System;
using BudgetLib.Budget;

namespace PConsole.EventHandlers
{
    public static class BudgetHandler
    {
        internal static void FindAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);

        internal static void AccountInfoHandler(object sender, BudgetEventArgs e)
        {
            Console.WriteLine($"{e.Message}\n\tId: {e.Id}\n\tType: {e.Type}\n\tLimit: {e.Limit}\n\tSum: {e.Sum}\n\tRegister data: {e.RegData}");   
        }

        internal static void OpenAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
        internal static void ChangeTypeAccountAccount(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
    }
}