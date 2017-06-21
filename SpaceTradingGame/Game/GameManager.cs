using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private List<StarSystem> systems;
        private StarSystem currentSystem;

        private string playerName;
        private string companyName;
        private Ship playerShip;

        public GameManager()
        {
            systems = new List<StarSystem>();

            //Sol System
            StarSystem solSystem = new StarSystem("Sol") { Coordinates = OpenTK.Vector2.Zero };
            solSystem.StarColor = OpenTK.Graphics.Color4.Yellow;

            //Terra
            Planetoid terra = new Planetoid(solSystem, "Terra");
            Planetoid luna = new Planetoid(solSystem, "Luna", terra);

            //Mars
            Planetoid mars = new Planetoid(solSystem, "Mars");
            Planetoid phobos = new Planetoid(solSystem, "Phobos", mars);
            Planetoid deimos = new Planetoid(solSystem, "Deimos", mars);

            //Jupiter
            Planetoid jupiter = new Planetoid(solSystem, "Jupiter");
            Planetoid io = new Planetoid(solSystem, "Io", jupiter);
            Planetoid europa = new Planetoid(solSystem, "Europa", jupiter);
            Planetoid ganymede = new Planetoid(solSystem, "Ganymede", jupiter);
            Planetoid callisto = new Planetoid(solSystem, "Callisto", jupiter);

            solSystem.Planetoids.Add(terra);
            solSystem.Planetoids.Add(mars);
            solSystem.Planetoids.Add(jupiter);

            solSystem.SystemMarket.UpdateMarket();

            systems.Add(solSystem);
            generateRandomSystems();

            CurrentSystem = solSystem;
        }

        public void SetupGame(string playerName, string companyName, Ship ship)
        {
            this.playerName = playerName;
            this.companyName = companyName;
            this.playerShip = ship;
        }

        private void generateRandomSystems()
        {
            int numSystems = RNG.Next(250, 500);
            for (int i = 0; i < numSystems; i++)
            {
                StarSystem system = new StarSystem(randomSystemName());
                system.Coordinates = new OpenTK.Vector2(
                    RNG.NextFloat(-10000.0f, 10000.0f),
                    RNG.NextFloat(-10000.0f, 10000.0f)
                );

                system.Planetoids = generateRandomPlanets(system);
                system.SystemMarket.UpdateMarket();

                systems.Add(system);
            }
        }
        private string randomSystemName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string name = chars[RNG.Next(0, chars.Length)].ToString() + chars[RNG.Next(0, chars.Length)].ToString() + "-";
            name += RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString() + RNG.Next(0, 10).ToString();

            return name;
        }
        private List<Planetoid> generateRandomPlanets(StarSystem system)
        {
            List<Planetoid> planets = new List<Planetoid>();
            int num = RNG.Next(0, 8);

            for (int i = 0; i < num; i++)
            {
                Planetoid planet = new Planetoid(system, string.Format("{0}-{1}", system.Name, i));
                planet.PrimaryExport = Item.Hydrogen;

                int numMoons = RNG.Next(0, 5);
                for (int j = 0; j < numMoons; j++)
                {
                    Planetoid moon = new Planetoid(system, "Moon", planet);
                }

                planets.Add(planet);
            }

            return planets;
        }

        public List<StarSystem> Systems { get { return this.systems; } }
        public StarSystem CurrentSystem { get { return this.currentSystem; } set { this.currentSystem = value; } }
        public string PlayerName { get { return this.playerName; } set { this.playerName = value; } }
        public string CompanyName { get { return this.companyName; } set { this.companyName = value; } }
        public Ship PlayerShip { get { return this.playerShip; } set { this.playerShip = value; } }
    }
}
