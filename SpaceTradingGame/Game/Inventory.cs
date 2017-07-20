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
        public int TotalWeight
        {
            get { return totalWeight; }
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
            else if (amount > 0)
            {
                InventorySlot slot = new InventorySlot()
                {
                    Item = item,
                    Quantity = amount
                };
                inventorySlots.Add(item, slot);
            }

            totalWeight += item.Weight * amount;
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
        public int GetTotalWeight(Item item)
        {
            if (!HasItem(item)) return 0;

            return inventorySlots[item].TotalWeight;
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
                if (slot.Value.Item.ItemType == itemType)
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
                if ((modList[i].Item as ShipMod).ModType != modType)
                    modList.RemoveAt(i--);
            }

            return modList;
        }
        public void AddInventoryList(List<InventorySlot> items)
        {
            foreach (InventorySlot slot in items)
            {
                AddItem(slot.Item, slot.Quantity);
            }
        }

        private int credits;
        private int totalWeight;
        private Dictionary<Item, InventorySlot> inventorySlots;
    }

    public class InventorySlot
    {
        public Item Item
        {
            get { return inventoryItem; }
            set { SetInventoryItem(value); }
        }
        public int Quantity
        {
            get { return quantity; }
            set { SetQuantity(value); }
        }
        public int TotalWeight { get; private set; }

        public void SetInventoryItem(Item item)
        {
            inventoryItem = item;
            CalculateWeight();
        }
        public void SetQuantity(int amount)
        {
            quantity = amount;
            CalculateWeight();
        }

        public void CalculateWeight()
        {
            TotalWeight = inventoryItem.Weight * quantity;
        }

        public override string ToString()
        {
            return string.Format("{0} - #{1}", inventoryItem.Name, quantity);
        }

        private Item inventoryItem;
        private int quantity;
    }
}
