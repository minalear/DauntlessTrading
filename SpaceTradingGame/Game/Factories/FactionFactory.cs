using System;
using System.Text;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game.Factories
{
    public static class FactionFactory
    {
        public static Faction GenerateRandomFaction()
        {
            StringBuilder name = new StringBuilder();
            name.Append(Names[RNG.Next(0, Names.Length)]);

            if (RNG.Next(0, 100) < 50)
            {
                name.Append(" and ");
                name.Append(Names[RNG.Next(0, Names.Length)]);
            }
            else
            {
                name.Append(" ");
                name.Append(Titles[RNG.Next(0, Titles.Length)]);
            }

            return new Faction(name.ToString()) { Capital = RNG.Next(75000, 400000) };
        }

        private static string[] Names = {
            "Barry", "Whitmore", "Fisher", "Comey", "Anderson", "Sun", "Kim", "Marchessa",
            "Hixus", "Warden", "Nantucket", "Knipplemeir", "Winchester", "Harden", "Blant",
            "Newport", "Koch", "Comcast", "New Harrier", "New Terran", "Baltic", "Horizon"
        };
        private static string[] Titles = {
            "Industries", "Inc.", "Shipping", "and Sons", "Company", "Trading", "Exports"
        };
    }
}
