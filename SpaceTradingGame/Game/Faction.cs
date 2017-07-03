using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Faction
    {
        public string Name { get; private set; }
        public bool PlayerOwned { get; private set; }

        public List<Ship> OwnedShips { get; private set; }
        public List<Market> OwnedMarkets { get; private set; }
        public List<Station> OwnedStations { get; private set; }

        public Faction(string name)
        {
            Name = name;
            PlayerOwned = false;
        }
        public Faction(string name, bool playerOwned)
        {
            Name = name;
            PlayerOwned = playerOwned;
        }

        public void MergeFactions(Faction owner, Faction subsidiary)
        {
            foreach (Ship ship in subsidiary.OwnedShips)
                owner.OwnedShips.Add(ship);

            foreach (Market market in subsidiary.OwnedMarkets)
                owner.OwnedMarkets.Add(market);

            foreach (Station station in subsidiary.OwnedStations)
                owner.OwnedStations.Add(station);
        }
    }
}
