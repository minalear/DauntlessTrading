using System;
using OpenTK;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Market
    {
        public Inventory MarketInventory { get; private set; }
        public Faction Owner { get; private set; }

        public Market(StarSystem system, Faction owner)
        {
            MarketInventory = new Inventory();
            starSystem = system;
            Owner = owner;

            MarketInventory.Credits = 10000000;
        }

        /// <summary>
        /// Market sells to someone.
        /// </summary>
        public double MarketSell(Item item, int amount)
        {
            if (amount > 0 && amount <= MarketInventory.GetQuantity(item))
            {
                double total = CalculateSellPrice(item, amount);
                MarketInventory.RemoveItem(item, amount);

                return total;
            }

            return 0.0;
        }

        /// <summary>
        /// Market buys from someone.
        /// </summary>
        public double MarketBuy(Item item, int amount)
        {
            double total = CalculateBuyPrice(item, amount);
            MarketInventory.AddItem(item, amount);

            return total;
        }
        public int CalculateBuyPrice(Item offeredItem, int amount)
        {
            int price = (int)(CalculateSellPrice(offeredItem, amount) * 0.9);
            return MathHelper.Clamp(price, 1, price);
        }
        public int CalculateSellPrice(Item item, int amount)
        {
            int existingQuantity = MarketInventory.GetQuantity(item);
            double existingMod = MathHelper.Clamp(1.0 - existingQuantity / (item.Rarity * 100.0), 0.05, 1.0);

            int price = (int)((item.BaseValue * amount) * existingMod);
            return MathHelper.Clamp(price, 1, price);
        }

        private StarSystem starSystem;
    }
}
