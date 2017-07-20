using System;
using System.Drawing;

namespace SpaceTradingGame.Game
{
    public class ShipNode : ICloneable
    {
        public Point Location { get { return location; } set { location = value; } }
        public ShipMod Modification { get; set; }
        public ShipMod.ShipModTypes ModType { get; set; }
        public int X { get { return location.X; } set { location.X = value; } }
        public int Y { get { return Location.Y; } set { location.Y = value; } }
        public bool Empty { get; set; }

        public ShipNode(int x, int y, ShipMod.ShipModTypes type)
        {
            Empty = true;
            ModType = type;
            Location = new Point(x, y);
        }
        public ShipNode(int x, int y, ShipMod mod)
            : this(x, y, mod.ModType)
        {
            Empty = false;
            Modification = mod;
        }

        public object Clone()
        {
            if (Empty) return new ShipNode(X, Y, ModType);
            return new ShipNode(X, Y, Modification);
        }
        public override string ToString()
        {
            if (Empty) return "Empty";
            return string.Format("{0} - {1}", ModType, Modification.Name);
        }

        private Point location;
    }
}
