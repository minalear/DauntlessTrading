using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradingGame.Game
{
    public class ShipMod : Item
    {
        public int Grade { get; set; }
        public int FirePowerMod { get; set; }
        public int CargoMod { get; set; }
        public int DefenseMod { get; set; }
        public double JumpMod { get; set; }
        public ShipModTypes ModType { get; set; }

        public ShipMod(ShipModTypes type)
        {
            this.ItemType = ItemTypes.ShipMod;
            this.ModType = type;
        }

        public override string GetDescription()
        {
            return string.Format("{0}\n-\nGrade: {1}\nAttack: {2}\nCargo: {3}\nDefense: {4}\nJump: {5}",
                baseDescription, Grade, FirePowerMod, CargoMod, DefenseMod, JumpMod);
        }

        public enum ShipModTypes
        {
            Cockpit, //Determines max grade of any ship modification
            WarpCore, //Improved fuel efficiency 
            Scanner, //For determining specs and cargo of other ships
            EmShield, //For defending against scanners of other ships
            StealthGen, //Generator for making ships invisible to various forms of detection
            ShieldGen, //Generator for shields which resist various forms of attack
            CargoBay, //Increased storage capacity for items
            Weapon, //Used to deal damage in combat
            Hull, //Makes the ship stronger to withstand various forms of attack
            Any //Any kind of modification can fit this slot
        }
    }
}
