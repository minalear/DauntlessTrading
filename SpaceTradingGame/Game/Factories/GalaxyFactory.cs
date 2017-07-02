using System;
using System.Collections.Generic;
using System.Text;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game.Factories
{
    public static class GalaxyFactory
    {
        public static List<StarSystem> GenerateGalaxy(int min, int max)
        {
            int numSystems = RNG.Next(min, max);
            List<StarSystem> systemList = new List<StarSystem>(numSystems);

            for (int i = 0; i < numSystems; i++)
            {
                systemList.Add(GenerateRandomStarSystem());
            }

            return systemList;
        }

        public static StarSystem GenerateRandomStarSystem()
        {
            StarSystem system = new StarSystem(GetRandomSystemName());
            system.Coordinates = new OpenTK.Vector2(
                    RNG.NextFloat(-10000.0f, 10000.0f),
                    RNG.NextFloat(-10000.0f, 10000.0f)
                );

            int numPlanets = RNG.Next(0, 8);
            for (int i = 0; i < numPlanets; i++)
            {
                system.Planetoids.Add(GenerateRandomPlanet(system));
            }

            return system;
        }
        public static Planetoid GenerateRandomPlanet(StarSystem parentSystem)
        {
            Planetoid planet = new Planetoid(parentSystem, GetRandomPlanetName(6));

            int numDeposits = RNG.Next(1, 12);
            for (int i = 0; i < numDeposits; i++)
            {
                planet.AddMaterialDeposit(GenerateRandomDeposit());
            }

            int numMoons = RNG.Next(0, 6);
            for (int i = 1; i <= numMoons; i++)
            {
                GenerateRandomMoon(planet, string.Format("{0}-{1}", planet.Name, i));
            }

            return planet;
        }
        public static Planetoid GenerateRandomMoon(Planetoid parent, string name)
        {
            return new Planetoid(parent.System, name, parent);
        }

        public static MaterialDeposit GenerateRandomDeposit()
        {
            Item material = GetRandomMaterial();
            double density = RNG.NextDouble(0.8, 1.2) * (material.Rarity / 100.0);

            return new MaterialDeposit() { Material = material, Density = density };
        }
        public static Item GetRandomMaterial()
        {
            //Adjust for item rarity
            return Item.MaterialsList[RNG.Next(0, Item.MaterialsList.Length)];
        }

        public static string GetRandomPlanetName(int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                builder.Append(letters[RNG.Next(0, letters.Length)]);
            }

            return builder.ToString();
        }
        public static string GetRandomSystemName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string name = chars[RNG.Next(0, chars.Length)].ToString() + chars[RNG.Next(0, chars.Length)].ToString() + "-";
            name += RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString();

            return name;
        }
    }
}
