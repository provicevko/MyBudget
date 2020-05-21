using System;
using BudgetLib.Budget;

namespace PConsole.EventHandlers
{
    public static class BudgetHandler
    {
        internal static void FindAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
        
    }
}