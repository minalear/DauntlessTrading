using System;
using OpenTK;
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
        public void UpdateMarket(double days)
        {
            foreach (Planetoid planet in starSystem.Planetoids)
            {
                if (planet.PrimaryExport == null) continue;

                int nDays = (int)MathHelper.Clamp(days, 1.0, days);
                for (int i = 0; i < nDays; i++)
                {
                    double variance = RNG.NextDouble(0.7, 1.2);
                    double stockCap = planet.PrimaryExport.Rarity * 100.0;
                    double stockMod = -(1.0 / stockCap) * MarketInventory.GetQuantity(planet.PrimaryExport) + 1.0;
                    double mod = variance * stockMod;

                    int amount = (int)(100 * mod);
                    MarketInventory.AddItem(planet.PrimaryExport, amount);

                    /*double variance = /*RNG.NextDouble(0.7, 1.2);
                    double rarity = planet.PrimaryExport.Rarity / 100.0;
                    double modifier = (variance * rarity);
                    
                    int currentAmount = MarketInventory.GetQuantity(planet.PrimaryExport);

                    double log = 1.0 - Math.Log10((1.0 / planet.PrimaryExport.Rarity) * currentAmount);
                    modifier += (MathHelper.Clamp(log, 0.0, 10.0));

                    int amount = (int)(100 * modifier);
                    MarketInventory.AddItem(planet.PrimaryExport, amount);*/
                }
            }
        }

        private StarSystem starSystem;
    }
}
