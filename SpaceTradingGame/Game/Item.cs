using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BaseValue { get; set; }
        public double Rarity { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        #region Raw Materials
        public static Item Hydrogen = new Item()
        {
            Name = "Hydrogen",
            Description = "Used for synthesizing various products.",
            BaseValue = 1,
            Weight = 1.0,
            Rarity = 100.0
        };
        public static Item Gold = new Item()
        {
            Name = "Gold",
            Description = "Used for backing credits.",
            BaseValue = 140,
            Weight = 70.0,
            Rarity = 65
        };
        public static Item Copper = new Item()
        {
            Name = "Copper",
            Description = "Used for creating computer components.",
            BaseValue = 13,
            Weight = 10.0,
            Rarity = 80
        };
        public static Item Helium = new Item()
        {
            Name = "Helium",
            Description = "Used for ion-based weaponry.",
            BaseValue = 10,
            Weight = 6.0,
            Rarity = 95
        };
        public static Item Oxygen = new Item()
        {
            Name = "Oxygen",
            Description = "Used for life support systems and fuel mixtures.",
            BaseValue = 10,
            Weight = 5.0,
            Rarity = 80
        };
        public static Item Iron = new Item()
        {
            Name = "Iron",
            Description = "Used for ship construction.",
            BaseValue = 8,
            Weight = 12.0,
            Rarity = 70
        };
        public static Item Cesium = new Item()
        {
            Name = "Cesium",
            Description = "Used for warp drive syncing systems.",
            BaseValue = 115,
            Weight = 85.0,
            Rarity = 40
        };
        public static Item Silver = new Item()
        {
            Name = "Silver",
            Description = "Used for quantum computing and warp drive systems.",
            BaseValue = 70,
            Weight = 45.0,
            Rarity = 65
        };
        public static Item Platinum = new Item()
        {
            Name = "Platinum",
            Description = "Used for warp drive cases.",
            BaseValue = 212,
            Weight = 69.0,
            Rarity = 35
        };
        public static Item Plutonium = new Item()
        {
            Name = "Plutonium",
            Description = "Used for warp fuel.",
            BaseValue = 140,
            Weight = 500.0,
            Rarity = 10
        };

        public static Item[] MaterialsList = new Item[] { Hydrogen, Gold, Helium, Copper, Oxygen, Iron, Cesium, Silver, Platinum, Plutonium };
        #endregion
        #region Refined Goods
        public static Item StarshipFuel = new Item()
        {
            Name = "Fuel",
            Description = "Universal fuel source for all modern starships.",
            BaseValue = 100,
            Weight = 10.0,
            Rarity = 1.0
        };
        #endregion
    }
}
