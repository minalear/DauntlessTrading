using System;
using System.Text;
using SpaceTradingGame.Engine;
using OpenTK.Graphics;

namespace SpaceTradingGame.Game.Factories
{
    public static class FactionFactory
    {
        //Auto increments to set unique faction colors
        private static int _colorPointer = 0;

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

            return new Faction(name.ToString()) { Capital = RNG.Next(75000, 400000), RegionColor = FactionColors[_colorPointer++] };
        }

        private static string[] Names = {
            "Barry", "Whitmore", "Fisher", "Comey", "Anderson", "Sun", "Kim", "Marchessa",
            "Hixus", "Warden", "Nantucket", "Knipplemeir", "Winchester", "Harden", "Blant",
            "Newport", "Koch", "Comcast", "New Harrier", "New Terran", "Baltic", "Horizon"
        };
        private static string[] Titles = {
            "Industries", "Inc.", "Shipping", "and Sons", "Company", "Trading", "Exports"
        };
        private static Color4[] FactionColors = {
            Color4.Red,
            Color4.Green,
            Color4.Blue, 
            Color4.Yellow,
            Color4.Orange,
            Color4.Purple,
            Color4.Cyan, 
            Color4.Brown,
            Color4.PeachPuff,
            Color4.ForestGreen,
        };
    }
}
