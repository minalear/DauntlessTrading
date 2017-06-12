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
            Weight = 10.0,
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
        public static Material Helium = new Material()
        {
            Name = "Helium",
            Description = "Used for ion-based weaponry.",
            BaseValue = 10.0,
            Weight = 6.0,
            Rarity = 1
        };
        public static Material Oxygen = new Material()
        {
            Name = "Oxygen",
            Description = "Used for life support systems and fuel mixtures.",
            BaseValue = 10.0,
            Weight = 5.0,
            Rarity = 2
        };
        public static Material Iron = new Material()
        {
            Name = "Iron",
            Description = "Used for ship construction.",
            BaseValue = 8.0,
            Weight = 12.0,
            Rarity = 2
        };
        public static Material Cesium = new Material()
        {
            Name = "Cesium",
            Description = "Used for warp drive syncing systems.",
            BaseValue = 115.0,
            Weight = 85.0,
            Rarity = 10
        };
        public static Material Silver = new Material()
        {
            Name = "Silver",
            Description = "Used for quantum computing and warp drive systems.",
            BaseValue = 70.0,
            Weight = 45.0,
            Rarity = 5
        };
        public static Material Platinum = new Material()
        {
            Name = "Platinum",
            Description = "Used for warp drive cases.",
            BaseValue = 212.0,
            Weight = 69.0,
            Rarity = 12
        };
        public static Material Plutonium = new Material()
        {
            Name = "Plutonium",
            Description = "Used for warp fuel.",
            BaseValue = 140.0,
            Weight = 500.0,
            Rarity = 15
        };

        public static Material[] MaterialList = new Material[] { Hydrogen, Copper, Gold, Helium, Oxygen, Iron, Cesium, Silver, Platinum, Plutonium };
    }
}
