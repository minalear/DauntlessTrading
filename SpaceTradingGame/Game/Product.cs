using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Product : Item
    {
        public List<RequirementInfo> Requirements { get; private set; }
        public int UnitsProducted { get; set; }

        public Product(string name, string description)
        {
            this.ItemType = ItemTypes.Product;
            this.Requirements = new List<RequirementInfo>();

            this.Name = name;
            this.Description = description;
            this.UnitsProducted = 1;
        }

        public void CalculateValue()
        {
            int valueTotal = 0;
            double totalRarity = 0.0;

            foreach (RequirementInfo req in Requirements)
            {
                valueTotal += req.Item.BaseValue * req.Quantity;
                totalRarity += req.Item.Rarity / 100.0;
            }

            Rarity = totalRarity * 100.0;
            BaseValue = (int)(valueTotal * 0.65) / UnitsProducted;
        }
        public void AddRequirement(Item item, int quantity)
        {
            Requirements.Add(new RequirementInfo(item, quantity));
            CalculateValue();
        }
        public bool CanProduce(Inventory inventory, bool checkQuantity)
        {
            foreach (RequirementInfo req in Requirements)
            {
                if (req.Item.GetType() == typeof(Product)) //Advanced Product
                {
                    Product product = (Product)req.Item;

                    //Recursive check to see if provided inventory can produce our product
                    if (!product.CanProduce(inventory, false)) return false;
                }
                else
                {
                    if (!inventory.HasItem(req.Item)) return false;
                    if (checkQuantity && inventory.GetQuantity(req.Item) < req.Quantity) return false;
                }
            }

            return true;
        }

        public struct RequirementInfo
        {
            public Item Item { get; set; }
            public int Quantity { get; set; }

            public RequirementInfo(Item item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }
        }

        //Pulls resources from local market
        //Materials are free if owned by same faction, otherwise spends budget
        //Budget is set and resets monthly
        //Add in UpdateMonth functions to station/market
    }
}
