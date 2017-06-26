using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class ShipMod : Item
    {
        public int FirePowerMod { get; set; }
        public int CargoMod { get; set; }
        public int DefenseMod { get; set; }
        public double JumpMod { get; set; }

        public ShipMod()
        {
            this.ItemType = ItemTypes.ShipMod;
        }
    }
}
