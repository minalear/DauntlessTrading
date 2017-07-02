using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private List<StarSystem> systems;
        private StarSystem currentSystem;

        private DateTime galacticDate;

        private string playerName;
        private string companyName;
        private Ship playerShip;

        public GameManager()
        {
            //Init Factories
            Factories.ModFactory.Init();
            Factories.ShipFactory.Init();

            galacticDate = new DateTime(2347, 1, 1);

            systems = new List<StarSystem>();

            //Sol System
            StarSystem solSystem = new StarSystem("Sol") { Coordinates = OpenTK.Vector2.Zero };
            solSystem.StarColor = OpenTK.Graphics.Color4.Yellow;

            Planetoid planet = Factories.GalaxyFactory.GenerateRandomPlanet(solSystem);

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

            systems.Add(solSystem);
            systems.AddRange(Factories.GalaxyFactory.GenerateGalaxy(250, 500));

            CurrentSystem = solSystem;
        }

        public void SetupGame(string playerName, string companyName, Ship ship)
        {
            this.playerName = playerName;
            this.companyName = companyName;
            this.playerShip = ship;
        }
        public void SimulateGame(double days)
        {
            galacticDate = galacticDate.AddDays(days);

            int _days = (int)OpenTK.MathHelper.Clamp(days, 1.0, days);
            foreach (StarSystem system in Systems)
            {
                for (int i = 0; i < _days; i++)
                    system.UpdateStarSystem();
            }
        }

        public List<StarSystem> Systems { get { return this.systems; } }
        public StarSystem CurrentSystem { get { return this.currentSystem; } set { this.currentSystem = value; } }
        public string PlayerName { get { return this.playerName; } set { this.playerName = value; } }
        public string CompanyName { get { return this.companyName; } set { this.companyName = value; } }
        public Ship PlayerShip { get { return this.playerShip; } set { this.playerShip = value; } }
        public DateTime GalacticDate { get { return galacticDate; } set { galacticDate = value; } }
    }
}
