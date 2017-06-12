using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Market
    {
        public Dictionary<Material, MetaInfo> Materials { get; set; }

        public Market(StarSystem system)
        {
            Materials = new Dictionary<Material, MetaInfo>();
            starSystem = system;
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
            if (!Materials.ContainsKey(material))
            {
                Materials.Add(material, new MetaInfo(material, 0));
            }

            double total = Materials[material].Price * amount;
            Materials[material].Amount += amount;
            Materials[material].CalculatePrice();

            return total;
        }
        public void UpdateMarket()
        {
            foreach (Planetoid planet in starSystem.Planetoids)
            {
                if (planet.PrimeMaterial == null) continue;
                MetaInfo info = new MetaInfo(planet.PrimeMaterial, RNG.Next(50 - planet.PrimeMaterial.Rarity, 500 - planet.PrimeMaterial.Rarity * 10));
                Sell(planet.PrimeMaterial, info.Amount);
            }
        }

        private StarSystem starSystem;

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
