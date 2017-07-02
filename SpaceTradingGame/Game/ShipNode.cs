using System;
using System.Drawing;

namespace SpaceTradingGame.Game
{
    public class ShipNode
    {
        public Point Location { get; set; }
        public ShipMod Modification { get; set; }
        public ShipMod.ShipModTypes ModType { get; set; }
        public int X { get { return Location.X; } }
        public int Y { get { return Location.Y; } }
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
    }
}
