using System;
using BudgetLib;

namespace PConsole
{
    public static class BudgetHandler
    {
        internal static void FindAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);

        internal static void AccountInfoHandler(object sender, BudgetEventArgs e)
        {
            Console.WriteLine($"{e.Message}\n\tId: {e.Id}\n\tType: {e.Type}\n\tLimit: {e.Limit}\n\tSum: {e.Sum}");   
        }

        internal static void ErrorHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
    }
}