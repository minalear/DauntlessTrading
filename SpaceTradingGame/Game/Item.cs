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
