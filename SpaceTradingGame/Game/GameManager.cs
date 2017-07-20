using System;
using System.Collections.Generic;
using OpenTK;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private TradingGame game;

        private List<StarSystem> systems;
        private List<Faction> factions;

        private DateTime galacticDate;

        private Ship playerShip;

        public GameManager(TradingGame game)
        {
            this.game = game;

            //Init Factories
            Factories.ProductFactory.Init();
            Factories.ModFactory.Init();
            Factories.ShipFactory.Init();

            systems = new List<StarSystem>();
            factions = new List<Faction>();
            Ships = new List<Ship>();
            
            Pathfinder = new Pathfinder(this);
            CombatSimulator = new CombatSimulator(this);
        }

        public void SetupGame(string playerName, string companyName, string shipName, Ship shipBlueprint)
        {
            //Reset variables between new games
            Factories.FactionFactory.Reset();
            StarSystem.ResetIDCounter();
            Ship.ResetIDCounter();

            GenerateGalaxy();

            Ship ship = Factories.ShipFactory.ConstructNewShip(shipBlueprint.Model);
            ship.Name = shipName;
            ship.Inventory.Credits = 500;

            this.playerShip = ship;

            PlayerFaction = new Faction(this, companyName, true);
            PlayerFaction.RegionColor = new OpenTK.Graphics.Color4(115, 99, 87, 255);

            playerShip.SetPilot(new Pilot(this, playerName, playerShip, true));
            PlayerShip.SetCurrentSystem(Systems[0]); //Set to Sol system

            PlayerFaction.RegisterShip(ship);
            factions.Add(PlayerFaction);

            SimulateGame(10.0);
        }
        public void GenerateGalaxy()
        {
            //Game simulates 10 days, starting the game 1/1/2347
            galacticDate = new DateTime(2346, 12, 22);

            cleanUpGalaxy();

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

            //Generate Factions
            int numFactions = RNG.Next(5, 10);
            for (int i = 0; i < numFactions; i++)
            {
                Faction faction = Factories.FactionFactory.GenerateRandomFaction(this);

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
                            Blueprint blueprint = new Blueprint(Factories.ModFactory.ModList[RNG.Next(0, Factories.ModFactory.ModList.Count)]);
                            planet.BuildFactory(faction, blueprint);
                        }
                    }
                }

                //Add a number of ships
                int numShips = RNG.Next(35, 50);
                for (int k = 0; k < numShips; k++)
                {
                    Ship ship = Factories.ShipFactory.ConstructRandomShip();
                    ship.Name = Factories.ShipFactory.GenerateRandomShipName();

                    ship.SetPilot(new Pilot(this, "Mark Webber", ship, false));
                    ship.SetCurrentSystem(Systems[0]); //Default to Sol system (for now)

                    faction.RegisterShip(ship);
                    Ships.Add(ship);
                }

                factions.Add(faction);
            }
        }

        public void SimulateGame(double days)
        {
            galacticDate = galacticDate.AddDays(days);

            foreach (StarSystem system in Systems)
            {
                system.UpdateStarSystem(days);
            }
            foreach (Faction faction in Factions)
            {
                faction.UpdateFaction(days);
            }
        }
        public void LoseGame()
        {
            game.InterfaceManager.ChangeInterface("Final");
        }
        public void ExitGame()
        {
            game.Exit();
        }

        public void ChangePlayerShip(Ship newShip)
        {
            //Unequip everything from the current ship
            foreach (ShipNode node in playerShip.Nodes)
            {
                playerShip.UnequipModule(node, true);
            }

            //Transfer inventory
            Inventory newShipInventory = new Inventory();
            newShipInventory.AddInventoryList(playerShip.Inventory.GetInventoryList());
            playerShip.Inventory.ClearInventory();
            playerShip.Inventory.AddInventoryList(newShip.Inventory.GetInventoryList());
            newShip.Inventory.AddInventoryList(newShipInventory.GetInventoryList());

            //Swap Pilots
            Pilot newPilot = newShip.Pilot;
            newShip.SetPilot(playerShip.Pilot);
            playerShip.SetPilot(newPilot);

            //Set coordinates
            newShip.SetCurrentSystem(playerShip.CurrentSystem);
            newShip.WorldPosition = playerShip.WorldPosition;

            playerShip = newShip;
        }

        public List<Ship> GetShipsInJumpRadius(Ship ship)
        {
            List<Ship> shipsInRange = new List<Ship>();

            foreach (Ship target in Ships)
            {
                if (target.ID != ship.ID && target.WorldPosition.Distance(ship.WorldPosition) <= ship.JumpRadius)
                    shipsInRange.Add(target);
            }

            return shipsInRange;
        }
        public void Destroy(Ship ship)
        {
            Ships.Remove(ship);
            ship.Faction.OwnedShips.Remove(ship);
        }

        private void cleanUpGalaxy()
        {
            foreach (Ship ship in Ships)
            {
                ship.Inventory.ClearInventory();
                ship.Nodes.Clear();
            }
            Ships.Clear();

            foreach (Faction faction in Factions)
            {
                faction.OwnedFactories.Clear();
                faction.OwnedMarkets.Clear();
                faction.OwnedShips.Clear();
                faction.OwnedStations.Clear();
            }

            factions.Clear();

            foreach (StarSystem system in Systems)
            {
                system.Planetoids.Clear();
            }

            Systems.Clear();
        }

        public List<StarSystem> Systems { get { return this.systems; } }
        public List<Faction> Factions { get { return this.factions; } }
        public List<Ship> Ships { get; private set; }
        public Ship PlayerShip { get { return this.playerShip; } set { this.playerShip = value; } }
        public DateTime GalacticDate { get { return galacticDate; } set { galacticDate = value; } }
        public Faction PlayerFaction { get; private set; }
        public Pathfinder Pathfinder { get; private set; }
        public CombatSimulator CombatSimulator { get; private set; }
        public StarSystem CurrentSystem
        {
            get { return PlayerShip.CurrentSystem; }
        }
    }
}
