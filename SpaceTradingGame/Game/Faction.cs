using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Faction : Engine.UI.Controls.ListItem
    {
        public GameManager GameManager { get; private set; }
        public string Name { get; private set; }
        public bool PlayerOwned { get; private set; }

        public int Capital { get; set; }
        public Color4 RegionColor { get; set; }

        public List<Ship> OwnedShips { get; private set; }
        public List<Market> OwnedMarkets { get; private set; }
        public List<Station> OwnedStations { get; private set; }
        public List<Factory> OwnedFactories { get; private set; }

        public List<int> StockPrices { get; private set; }

        public Faction(GameManager gameManager, string name) : this(gameManager, name, false) { }
        public Faction(GameManager gameManager, string name, bool playerOwned)
        {
            GameManager = gameManager;
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
                    {
                        //Move ship to random market (via random faction => random owned market)
                        Faction randomFaction = GameManager.Factions[RNG.Next(0, GameManager.Factions.Count)];
                        if (randomFaction.OwnedMarkets.Count <= 0)
                        {
                            //In case there are no markets anywhere, ships will park until one opens up
                            continue;
                        }
                        Market randomMarket = randomFaction.OwnedMarkets[RNG.Next(0, randomFaction.OwnedMarkets.Count)];

                        ship.Pilot.MoveTo(randomMarket.System);
                    }
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
            Pilot pilot = (Pilot)sender;
            if (pilot.Ship.CurrentSystem.HasMarket)
            {
                Market market = pilot.Ship.CurrentSystem.Market;

                //Sell randomly held resources
                List<InventorySlot> inventory = pilot.Ship.Inventory.GetInventoryList();
                int numSales = RNG.Next(0, inventory.Count);
                for (int i = 0; i < numSales && inventory.Count > 0; i++)
                {
                    InventorySlot slot = inventory[RNG.Next(0, inventory.Count)];
                    int quantity = RNG.Next(1, slot.Quantity);

                    //Check if the market can afford the purchase
                    if (market.CalculateBuyPrice(slot.Item, quantity) <= market.MarketInventory.Credits)
                    {
                        market.MarketBuy(slot.Item, quantity);
                        pilot.Ship.Inventory.RemoveItem(slot.Item, quantity);

                        inventory = pilot.Ship.Inventory.GetInventoryList(); //List doesn't update automatically
                    }
                }

                //Buy random resources
                List<InventorySlot> marketList = market.MarketInventory.GetInventoryList();
                int numPurchases = RNG.Next(3, 6);
                for (int i = 0; i < numPurchases && marketList.Count > 0; i++)
                {
                    InventorySlot slot = marketList[RNG.Next(0, marketList.Count)];
                    int quantity = RNG.Next(1, slot.Quantity);

                    pilot.Ship.Inventory.AddItem(slot.Item, quantity);
                    market.MarketSell(slot.Item, quantity); //Factions have infinite money for the moment

                    marketList = market.MarketInventory.GetInventoryList(); //List doesn't update automatically
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private double stockTimer = 0.0;
        private double variance = 1.0;
    }
}
