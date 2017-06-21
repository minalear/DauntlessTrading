using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Planetoid : Engine.UI.Controls.ListItem
    {
        public StarSystem System { get; set; }
        public Planetoid Parent { get; set; }
        public string Name { get; set; }
        public List<Planetoid> Moons { get; set; }
        public List<Station> Stations { get; set; }
        public Item PrimaryExport { get; set; }

        public Planetoid(StarSystem system, string name)
        {
            System = system;
            Name = name;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();

            ListText = Name;
        }
        public Planetoid(StarSystem system, string name, Planetoid parent)
        {
            System = system;
            Name = name;
            Parent = parent;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();

            Parent.Moons.Add(this);

            ListText = Name;
        }
    }
}
