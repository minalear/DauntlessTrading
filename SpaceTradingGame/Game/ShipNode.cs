using System;
using System.Drawing;

namespace SpaceTradingGame.Game
{
    public class ShipNode
    {
        public Point Location { get; set; }
        public ShipMod Modification { get; set; }
        public int X { get { return Location.X; } }
        public int Y { get { return Location.Y; } }
        public bool Empty { get; set; }

        public ShipNode(int x, int y)
        {
            Empty = true;
            Location = new Point(x, y);
        }
        public ShipNode(int x, int y, ShipMod mod)
            : this(x, y)
        {
            Empty = false;
            Modification = mod;
        }
    }
}
