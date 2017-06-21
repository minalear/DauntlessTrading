using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class Inventory
    {
        public int Credits
        {
            get { return credits; }
            set
            {
                credits = (value < 0) ? 0 : value;
            }
        }

        public Inventory()
        {
            inventorySlots = new Dictionary<Item, InventorySlot>();
        }

        public bool HasItem(Item item)
        {
            return inventorySlots.ContainsKey(item);
        }
        public void AddItem(Item item, int amount)
        {
            if (HasItem(item))
            {
                inventorySlots[item].Quantity += amount;
            }
            else
            {
                InventorySlot slot = new InventorySlot()
                {
                    InventoryItem = item,
                    Quantity = amount
                };
                inventorySlots.Add(item, slot);
            }
        }
        public void RemoveItem(Item item, int amount)
        {
            if (!HasItem(item)) return;

            int quantity = inventorySlots[item].Quantity;
            inventorySlots[item].Quantity = (quantity > amount) ? quantity - amount : 0;
        }
        public void ClearItem(Item item)
        {
            if (!HasItem(item)) return;
            inventorySlots.Remove(item);
        }
        public void ClearInventory()
        {
            inventorySlots.Clear();
        }
        public int GetQuantity(Item item)
        {
            if (!HasItem(item)) return 0;

            return inventorySlots[item].Quantity;
        }
        public List<InventorySlot> GetInventoryList()
        {
            return inventorySlots.Values.ToList();
        }

        private int credits;
        private Dictionary<Item, InventorySlot> inventorySlots;
    }

    public class InventorySlot
    {
        public Item InventoryItem { get; set; }
        public int Quantity { get; set; }
    }
}
