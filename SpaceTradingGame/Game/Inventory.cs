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

            //Remove items that have zero quantity
            if (inventorySlots[item].Quantity <= 0)
                ClearItem(item);
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
        public List<InventorySlot> GetInventoryList(ItemTypes itemType)
        {
            List<InventorySlot> filteredList = new List<InventorySlot>();
            foreach (KeyValuePair<Item, InventorySlot> slot in inventorySlots)
            {
                if (slot.Value.InventoryItem.ItemType == itemType)
                    filteredList.Add(slot.Value);
            }

            return filteredList;
        }
        public List<InventorySlot> GetInventoryList(ShipMod.ShipModTypes modType)
        {
            List<InventorySlot> modList = GetInventoryList(ItemTypes.ShipMod);
            if (modType == ShipMod.ShipModTypes.Any) return modList; //Return all mods if Any

            //Remove any mod that doesn't match the type
            for (int i = 0; i < modList.Count; i++)
            {
                if ((modList[i].InventoryItem as ShipMod).ModType != modType)
                    modList.RemoveAt(i--);
            }

            return modList;
        }
        public void AddInventoryList(List<InventorySlot> items)
        {
            foreach (InventorySlot slot in items)
            {
                AddItem(slot.InventoryItem, slot.Quantity);
            }
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
