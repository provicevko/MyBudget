using System;
using MoneySpendingItems;

namespace PConsole
{
    public static class MoneyItems
    {
        internal static void AddItems(PutItems putItems)
        {
            putItems.AddItem("Freelance");
            putItems.AddItem("Job");
            putItems.AddItem("Other");
        }
        
        internal static void SpendItems(SpendItems spendItems)
        {
            spendItems.SpendItem("Food");
            spendItems.SpendItem("Technics");
            spendItems.SpendItem("Entertainment");
            spendItems.SpendItem("Other");
        }
    }
}