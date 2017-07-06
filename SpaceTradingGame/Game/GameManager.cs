using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private List<StarSystem> systems;
        private List<Faction> factions;

        private StarSystem currentSystem;

        private DateTime galacticDate;

        private string playerName;
        private Ship playerShip;

        public GameManager()
        {
            //Init Factories
            Factories.ProductFactory.Init();
            Factories.ModFactory.Init();
            Factories.ShipFactory.Init();

            galacticDate = new DateTime(2347, 1, 1);
            Pathfinder = new Pathfinder(this);

            systems = new List<StarSystem>();
            factions = new List<Faction>();
            Ships = new List<Ship>();

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

            systems.Add(solSystem);
            systems.AddRange(Factories.GalaxyFactory.GenerateGalaxy(250, 500));

            CurrentSystem = solSystem;

            //Generate Factions
            int numFactions = RNG.Next(5, 10);
            for (int i = 0; i < numFactions; i++)
            {
                Faction faction = Factories.FactionFactory.GenerateRandomFaction();

                //Build markets, stations, and factories.  Add ships.
                int numSystems = RNG.Next(3, 6);
                for (int k = 0; k < numSystems; k++)
                {
                    StarSystem system = systems[RNG.Next(0, systems.Count)];

                    //Ensure planets exist in the system
                    while (system.Planetoids.Count == 0 || system.HasMarket)
                        system = systems[RNG.Next(0, systems.Count)];

                    system.BuildMarket(faction);

                    foreach (Planetoid planet in system.Planetoids)
                    {
                        planet.BuildStation(faction);

                        //Try to build a factory
                        if (RNG.Next(0, 100) < 50)
                        {
                            Product product = Factories.ProductFactory.ProductList[RNG.Next(0, Factories.ProductFactory.ProductList.Length)];
                            planet.BuildFactory(faction, product);
                        }
                        else
                        {
                            Blueprint blueprint = new Blueprint(Factories.ModFactory.ModList[RNG.Next(0, Factories.ModFactory.ModList.Length)]);
                            planet.BuildFactory(faction, blueprint);
                        }
                    }
                }

                //Add a number of ships
                int numShips = RNG.Next(4, 8);
                for (int k = 0; k < numShips; k++)
                {
                    Ship ship = Factories.ShipFactory.ConstructNewShip("Maverick Mk I");
                    ship.Name = Factories.ShipFactory.GenerateRandomShipName();

                    ship.SetPilot(new Pilot(this, "Mark Webber", faction, ship, false));
                    ship.SetCurrentSystem(Systems[0]);

                    faction.OwnedShips.Add(ship); //They all appear at Sol atm
                    Ships.Add(ship);
                }

                factions.Add(faction);
            }
        }

        public void SetupGame(string playerName, string companyName, Ship ship)
        {
            this.playerName = playerName;
            this.playerShip = ship;

            PlayerFaction = new Faction(companyName, true);
            PlayerFaction.RegionColor = new OpenTK.Graphics.Color4(115, 99, 87, 255);

            playerShip.SetPilot(new Pilot(this, playerName, PlayerFaction, playerShip, true));
            PlayerShip.SetCurrentSystem(CurrentSystem);

            PlayerFaction.OwnedShips.Add(playerShip);
            factions.Add(PlayerFaction);

            //SimulateGame(10.0);
        }
        public void SimulateGame(double days)
        {
            galacticDate = galacticDate.AddDays(days);

            int _days = (int)OpenTK.MathHelper.Clamp(days, 1.0, days);
            foreach (StarSystem system in Systems)
            {
                for (int i = 0; i < _days; i++)
                    system.UpdateStarSystem(days);
            }
            foreach (Faction faction in Factions)
            {
                faction.UpdateFaction(days);
            }
        }

        public List<StarSystem> Systems { get { return this.systems; } }
        public List<Faction> Factions { get { return this.factions; } }
        public List<Ship> Ships { get; private set; }
        public StarSystem CurrentSystem { get { return this.currentSystem; } set { this.currentSystem = value; } }
        public string PlayerName { get { return this.playerName; } set { this.playerName = value; } }
        public Ship PlayerShip { get { return this.playerShip; } set { this.playerShip = value; } }
        public DateTime GalacticDate { get { return galacticDate; } set { galacticDate = value; } }
        public Faction PlayerFaction { get; private set; }
        public Pathfinder Pathfinder { get; private set; }
    }
}
