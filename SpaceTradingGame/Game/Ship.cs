using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class Ship
    {
        private string shipName;
        private string shipModel;
        private int cargoCapacity;
        private double baseJumpRadius;

        public Ship()
        {
            this.shipName = "Name";
            this.shipModel = "Maverick Class I";
            this.cargoCapacity = 100;
            this.baseJumpRadius = 350.0;
        }
        public Ship(string name, string model, int capacity, double jumpRadius)
        {
            this.shipName = name;
            this.shipModel = model;
            this.cargoCapacity = capacity;
            this.baseJumpRadius = jumpRadius;
        }

        public string Name { get { return shipName; } set { shipName = value; } }
        public string Model { get { return shipModel; } set { shipModel = value; } }
        public int CargoCapacity { get { return cargoCapacity; } set { cargoCapacity = value; } }
        public double BaseJumpRadius { get { return baseJumpRadius; } set { baseJumpRadius = value; } }
    }
}
