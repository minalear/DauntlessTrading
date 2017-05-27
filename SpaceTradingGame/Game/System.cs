using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class System : Engine.UI.Controls.ListItem
    {
        public string Name { get; set; }
        public List<Planetoid> Planetoids { get; set; }

        public System(string name)
        {
            Name = name;
            Planetoids = new List<Planetoid>();

            this.ListText = Name;
        }
    }
}
