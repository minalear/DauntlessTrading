using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Market
    {
        public Inventory MarketInventory { get; set; }

        public Market(StarSystem system)
        {
            MarketInventory = new Inventory();
            starSystem = system;

            MarketInventory.Credits = 10000000;
        }

        public double Buy(Item item, int amount)
        {
            if (amount > 0 && amount <= MarketInventory.GetQuantity(item))
            {
                double total = item.BaseValue * amount;
                MarketInventory.RemoveItem(item, amount);

                return total;
            }

            return 0.0;
        }
        public double Sell(Item item, int amount)
        {
            double total = item.BaseValue * amount;
            MarketInventory.AddItem(item, amount);

            return total;
        }
        public void UpdateMarket()
        {
            foreach (Planetoid planet in starSystem.Planetoids)
            {
                if (planet.PrimaryExport == null) continue;
                int amount = RNG.Next(50, 500);
                MarketInventory.AddItem(planet.PrimaryExport, amount);
            }
        }

        private StarSystem starSystem;
    }
}
