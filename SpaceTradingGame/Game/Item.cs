﻿using System;
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
        public double BaseValue { get; set; }
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
            BaseValue = 1.0,
            Weight = 1.0,
            Rarity = 100.0
        };
        public static Item Gold = new Item()
        {
            Name = "Gold",
            Description = "Used for backing credits.",
            BaseValue = 140.0,
            Weight = 70.0,
            Rarity = 7.0
        };
        public static Item Copper = new Item()
        {
            Name = "Copper",
            Description = "Used for creating computer components.",
            BaseValue = 13.0,
            Weight = 10.0,
            Rarity = 2
        };
        public static Item Helium = new Item()
        {
            Name = "Helium",
            Description = "Used for ion-based weaponry.",
            BaseValue = 10.0,
            Weight = 6.0,
            Rarity = 1
        };
        public static Item Oxygen = new Item()
        {
            Name = "Oxygen",
            Description = "Used for life support systems and fuel mixtures.",
            BaseValue = 10.0,
            Weight = 5.0,
            Rarity = 2
        };
        public static Item Iron = new Item()
        {
            Name = "Iron",
            Description = "Used for ship construction.",
            BaseValue = 8.0,
            Weight = 12.0,
            Rarity = 2
        };
        public static Item Cesium = new Item()
        {
            Name = "Cesium",
            Description = "Used for warp drive syncing systems.",
            BaseValue = 115.0,
            Weight = 85.0,
            Rarity = 10
        };
        public static Item Silver = new Item()
        {
            Name = "Silver",
            Description = "Used for quantum computing and warp drive systems.",
            BaseValue = 70.0,
            Weight = 45.0,
            Rarity = 5
        };
        public static Item Platinum = new Item()
        {
            Name = "Platinum",
            Description = "Used for warp drive cases.",
            BaseValue = 212.0,
            Weight = 69.0,
            Rarity = 12
        };
        public static Item Plutonium = new Item()
        {
            Name = "Plutonium",
            Description = "Used for warp fuel.",
            BaseValue = 140.0,
            Weight = 500.0,
            Rarity = 15
        };
        #endregion
        #region Refined Goods
        public static Item StarshipFuel = new Item()
        {
            Name = "Fuel",
            Description = "Universal fuel source for all modern starships.",
            BaseValue = 100.0,
            Weight = 10.0,
            Rarity = 1.0
        };
        #endregion
    }
}
