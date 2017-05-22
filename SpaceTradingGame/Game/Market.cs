using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Market
    {
        public Dictionary<Material, MetaInfo> Materials { get; set; }

        public Market()
        {
            Materials = new Dictionary<Material, MetaInfo>();
            Materials.Add(Material.Hydrogen, new MetaInfo(Material.Hydrogen, 100));
            Materials.Add(Material.Copper, new MetaInfo(Material.Copper, 9));
            Materials.Add(Material.Gold, new MetaInfo(Material.Gold, 87));
        }

        public double Buy(Material material, int amount)
        {
            if (amount > 0 && amount <= Materials[material].Amount)
            {
                double total = Materials[material].Price * amount;
                Materials[material].Amount -= amount;
                Materials[material].CalculatePrice();

                return total;
            }

            return 0.0;
        }
        public double Sell(Material material, int amount)
        {
            double total = Materials[material].Price * amount;
            Materials[material].Amount += amount;
            Materials[material].CalculatePrice();

            return total;
        }

        public class MetaInfo
        {
            public Material Material;
            public int Amount;
            public double Price;

            public MetaInfo(Material material, int amount)
            {
                Material = material;
                Amount = amount;

                CalculatePrice();
            }

            public void CalculatePrice()
            {
                double scarcity = ((11 - Material.Rarity) * 10.0) / Amount;
                Price = Math.Round(Material.BaseValue * scarcity, 2);

                if (Price < 0.01) Price = 0.01;
            }
        }
    }
}
