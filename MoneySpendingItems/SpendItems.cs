using System.Collections.Generic;

namespace MoneySpendingItems
{
    public class SpendItems
    {
        private readonly List<ItemsStruct> _items;
        public SpendItems()
        {
            _items = new List<ItemsStruct>();
        }

        public void SpendItem(string name)
        {
            ItemsStruct item = new ItemsStruct(name);
            _items.Add(item);
        }

        public List<ItemsStruct> GetItems => _items;
    }
}