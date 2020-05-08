using System;
using BudgetLib;

namespace PConsole
{
    public class BudgetHandler
    {
        internal static void FindAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
        internal static void ChooseAccountHandler(object sender, BudgetEventArgs e) => Console.WriteLine(e.Message);
    }
}