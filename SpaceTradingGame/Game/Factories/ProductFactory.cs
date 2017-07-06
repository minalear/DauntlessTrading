using System;

namespace SpaceTradingGame.Game.Factories
{
    public static class ProductFactory
    {
        public static void Init()
        {
            //Base Products
            Water = new Product("Water", "Used to preserve life and as a cheap coolant.");
            Water.UnitsProducted = 5;
            Water.AddRequirement(Item.Hydrogen, 20);
            Water.AddRequirement(Item.Oxygen, 10);

            CarbonDioxide = new Product("Carbon Dioxide", "Various uses.");
            CarbonDioxide.UnitsProducted = 5;
            CarbonDioxide.AddRequirement(Item.Carbon, 10);
            CarbonDioxide.AddRequirement(Item.Oxygen, 20);

            ProductList = new Product[2] { Water, CarbonDioxide };
        }

        public static Product Water;
        public static Product CarbonDioxide;

        public static Product[] ProductList;
    }
}
