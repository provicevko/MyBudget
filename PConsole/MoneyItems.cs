using System;
using MoneySpendingItems;

namespace PConsole
{
    public static class MoneyItems
    {
        internal static void AddItems(PutItems putItems)
        {
            putItems.AddItem("фріланс");
            putItems.AddItem("робота");
            putItems.AddItem("інше");
        }
        
        internal static void SpendItems(SpendItems spendItems)
        {
            spendItems.SpendItem("їжа");
            spendItems.SpendItem("техніка");
            spendItems.SpendItem("розваги");
            spendItems.SpendItem("інше");
        }
    }
}