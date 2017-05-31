using System;
using System.Collections.Generic;
using OpenTK;

namespace SpaceTradingGame.Game
{
    public class StarSystem : Engine.UI.Controls.ListItem
    {
        public string Name { get; set; }
        public List<Planetoid> Planetoids { get; set; }

        public Vector2 Coordinates { get; set; }

        public StarSystem(string name)
        {
            Name = name;
            Planetoids = new List<Planetoid>();

            Coordinates = Vector2.Zero;

            this.ListText = Name;
        }
    }
}
