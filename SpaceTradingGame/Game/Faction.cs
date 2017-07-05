using System;
using System.Collections.Generic;
using OpenTK.Graphics;

namespace SpaceTradingGame.Game
{
    public class Faction : SpaceTradingGame.Engine.UI.Controls.ListItem
    {
        public string Name { get; private set; }
        public bool PlayerOwned { get; private set; }

        public int Capital { get; set; }
        public Color4 RegionColor { get; set; }

        public List<Ship> OwnedShips { get; private set; }
        public List<Market> OwnedMarkets { get; private set; }
        public List<Station> OwnedStations { get; private set; }
        public List<Factory> OwnedFactories { get; private set; }

        public Faction(string name)
        {
            Name = name;
            PlayerOwned = false;

            OwnedShips = new List<Ship>();
            OwnedMarkets = new List<Market>();
            OwnedStations = new List<Station>();
            OwnedFactories = new List<Factory>();

            ListText = Name;
        }
        public Faction(string name, bool playerOwned)
        {
            Name = name;
            PlayerOwned = playerOwned;

            OwnedShips = new List<Ship>();
            OwnedMarkets = new List<Market>();
            OwnedStations = new List<Station>();
            OwnedFactories = new List<Factory>();

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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
