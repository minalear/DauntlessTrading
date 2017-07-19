using Newtonsoft.Json;

namespace SpaceTradingGame.Game
{
    public class Item
    {
        protected string baseDescription;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get { return GetDescription(); } set { baseDescription = value; } }

        [JsonProperty(PropertyName = "value")]
        public int BaseValue { get; set; }

        [JsonProperty(PropertyName = "rarity")]
        public double Rarity { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }

        [JsonProperty(PropertyName = "basetype")]
        public ItemTypes ItemType { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
        public virtual string GetDescription()
        {
            return this.baseDescription;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Item))
                return ((Item)obj).Name == Name;

            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region Raw Materials
        public static Item Hydrogen = new Item()
        {
            Name = "Hydrogen",
            Description = "Used for synthesizing various products.",
            BaseValue = 10,
            Weight = 1,
            Rarity = 100.0
        };
        public static Item Gold = new Item()
        {
            Name = "Gold",
            Description = "Used for backing credits.",
            BaseValue = 1400,
            Weight = 70,
            Rarity = 65
        };
        public static Item Copper = new Item()
        {
            Name = "Copper",
            Description = "Used for creating computer components.",
            BaseValue = 130,
            Weight = 10,
            Rarity = 80
        };
        public static Item Helium = new Item()
        {
            Name = "Helium",
            Description = "Used for ion-based weaponry.",
            BaseValue = 100,
            Weight = 6,
            Rarity = 95
        };
        public static Item Oxygen = new Item()
        {
            Name = "Oxygen",
            Description = "Used for life support systems and fuel mixtures.",
            BaseValue = 100,
            Weight = 5,
            Rarity = 80
        };
        public static Item Iron = new Item()
        {
            Name = "Iron",
            Description = "Used for ship construction.",
            BaseValue = 80,
            Weight = 12,
            Rarity = 70
        };
        public static Item Carbon = new Item()
        {
            Name = "Carbon",
            Description = "HUMANS ARE MADE OUT OF THIS SHIT... and water.",
            BaseValue = 50,
            Weight = 8,
            Rarity = 75
        };
        public static Item Cesium = new Item()
        {
            Name = "Cesium",
            Description = "Used for warp drive syncing systems.",
            BaseValue = 1150,
            Weight = 85,
            Rarity = 40
        };
        public static Item Silver = new Item()
        {
            Name = "Silver",
            Description = "Used for quantum computing and warp drive systems.",
            BaseValue = 700,
            Weight = 45,
            Rarity = 65
        };
        public static Item Platinum = new Item()
        {
            Name = "Platinum",
            Description = "Used for warp drive cases.",
            BaseValue = 2120,
            Weight = 69,
            Rarity = 35
        };
        public static Item Plutonium = new Item()
        {
            Name = "Plutonium",
            Description = "Used for warp fuel.",
            BaseValue = 14000,
            Weight = 500,
            Rarity = 10
        };

        public static Item[] MaterialsList = new Item[] { Hydrogen, Gold, Helium, Copper, Oxygen, Iron, Cesium, Silver, Platinum, Plutonium };
        #endregion
        #region Refined Goods
        public static Item StarshipFuel = new Item()
        {
            Name = "Fuel",
            Description = "Universal fuel source for all modern starships.",
            BaseValue = 1000,
            Weight = 10,
            Rarity = 1.0
        };
        #endregion
    }

    public enum ItemTypes { RawMaterial, ShipMod, Product, Other }
}
