using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class Ship : Engine.UI.Controls.ListItem
    {
        private string shipName;
        private string shipModel;
        private int firePower;
        private int defenseRating;
        private int cargoCapacity;
        private double baseJumpRadius;

        private Inventory shipInventory;

        public Ship()
        {
            this.shipName = "Name";
            this.shipModel = "Maverick Class I";
            this.firePower = 10;
            this.cargoCapacity = 100;
            this.baseJumpRadius = 350.0;

            this.shipInventory = new Inventory();

            this.ListText = shipModel;
        }
        public Ship(string name, string model, int firePower, int defenseRating, int capacity, double jumpRadius)
        {
            this.shipName = name;
            this.shipModel = model;
            this.firePower = firePower;
            this.defenseRating = defenseRating;
            this.cargoCapacity = capacity;
            this.baseJumpRadius = jumpRadius;

            this.shipInventory = new Inventory();

            this.ListText = shipModel;
        }

        public override string ToString()
        {
            return string.Format("== {0} ==\nFire: {1}\nDfns: {2}\nCargo: {3}\nJump: {4}", 
                shipModel, firePower, defenseRating, cargoCapacity, baseJumpRadius);
        }

        public string Name { get { return shipName; } set { shipName = value; } }
        public string Model { get { return shipModel; } set { shipModel = value; } }
        public int FirePower { get { return this.firePower; } set { this.firePower = value; } }
        public int DefenseRating { get { return this.defenseRating; } set { this.defenseRating = value; } }
        public int CargoCapacity { get { return cargoCapacity; } set { cargoCapacity = value; } }
        public double BaseJumpRadius { get { return baseJumpRadius; } set { baseJumpRadius = value; } }
        public Inventory Inventory { get { return shipInventory; } set { shipInventory = value; } }

        public static Ship MaverickMkI = new Ship("Blueprint", "Maverick Mk I", 15, 10, 100, 500);
        public static Ship MaverickMkII = new Ship("Blueprint", "Maverick Mk II", 20, 15, 135, 850);
        public static Ship DelpheneI = new Ship("Blueprint", "Delphene I", 8, 12, 140, 550);
        public static Ship DelpheneII = new Ship("Blueprint", "Delphene II", 12, 20, 220, 850);
        public static Ship Dauntless = new Ship("Blueprint", "Dauntless Class", 25, 50, 800, 1350);
        public static Ship Exodia = new Ship("Blueprint", "Exodia Class", 80, 100, 2400, 2500);

        public static Ship[] ShipBlueprints = new Ship[] { MaverickMkI, MaverickMkII, DelpheneI, DelpheneII, Dauntless, Exodia };
    }
}
