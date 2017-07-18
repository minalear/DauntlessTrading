using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Faction : Engine.UI.Controls.ListItem
    {
        public string Name { get; private set; }
        public bool PlayerOwned { get; private set; }

        public int Capital { get; set; }
        public Color4 RegionColor { get; set; }

        public List<Ship> OwnedShips { get; private set; }
        public List<Market> OwnedMarkets { get; private set; }
        public List<Station> OwnedStations { get; private set; }
        public List<Factory> OwnedFactories { get; private set; }

        public List<int> StockPrices { get; private set; }

        public Faction(string name) : this(name, false) { }
        public Faction(string name, bool playerOwned)
        {
            Name = name;
            PlayerOwned = playerOwned;

            OwnedShips = new List<Ship>();
            OwnedMarkets = new List<Market>();
            OwnedStations = new List<Station>();
            OwnedFactories = new List<Factory>();
            StockPrices = new List<int>();

            ListText = Name;
        }

        public void MergeFactions(Faction owner, Faction subsidiary)
        {
            foreach (Ship ship in subsidiary.OwnedShips)
                owner.OwnedShips.Add(ship);

            foreach (Market market in subsidiary.OwnedMarkets)
                owner.OwnedMarkets.Add(market);

            foreach (Station station in subsidiary.OwnedStations)
                owner.OwnedStations.Add(station);

            foreach (Factory factory in subsidiary.OwnedFactories)
                owner.OwnedFactories.Add(factory);

            subsidiary.OwnedShips.Clear();
            subsidiary.OwnedMarkets.Clear();
            subsidiary.OwnedShips.Clear();
            subsidiary.OwnedFactories.Clear();
        }
        public void UpdateFaction(double days)
        {
            foreach (Ship ship in OwnedShips)
            {
                if (!ship.Pilot.IsTraveling)
                {
                    if (!ship.Pilot.IsPlayer)
                        ship.Pilot.MoveTo(ship.Pilot.GameManager.Systems[RNG.Next(0, ship.Pilot.GameManager.Systems.Count)]);
                }

                ship.Pilot.Update(days);
            }

            stockTimer += days;
            while (stockTimer >= 1.0)
            {
                stockTimer -= 1.0;
                CalculateStockPrice();
            }
        }
        public void RegisterShip(Ship ship)
        {
            ship.Faction = this;
            ship.Pilot.Finished += PilotFinishedJourney;
            OwnedShips.Add(ship);
        }
        public void CalculateStockPrice()
        {
            variance *= RNG.NextDouble(0.9, 1.1);

            int price =
                OwnedShips.Count +
                OwnedMarkets.Count * 10 +
                OwnedStations.Count * 5 +
                OwnedFactories.Count * 5;

            price *= (int)(price * variance) + RNG.Next(-5000, 5000);
            price = OpenTK.MathHelper.Clamp(price, 1, price);
            StockPrices.Add(price);
        }

        private void PilotFinishedJourney(object sender, EventArgs e)
        {

        }

        public override string ToString()
        {
            return this.Name;
        }

        private double stockTimer = 0.0;
        private double variance = 1.0;
    }
}
