using System;

namespace SpaceTradingGame.Game
{
    /// <summary>
    /// Materials generated and traded for money or other materials.  Trade goods.
    /// </summary>
    public class Material
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double BaseValue { get; set; }
        public double Weight { get; set; }
        public int Rarity { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static Material Hydrogen = new Material()
        {
            Name = "Hydrogen",
            Description = "Used for creating fuel.",
            BaseValue = 1.0,
            Weight = 1.0,
            Rarity = 1
        };
        public static Material Copper = new Material()
        {
            Name = "Copper",
            Description = "Used for creating computer components.",
            BaseValue = 13.0,
            Weight = 7.0,
            Rarity = 2
        };
        public static Material Gold = new Material()
        {
            Name = "Gold",
            Description = "Used for backing credits.",
            BaseValue = 140.0,
            Weight = 70.0,
            Rarity = 7
        };
    }
}
