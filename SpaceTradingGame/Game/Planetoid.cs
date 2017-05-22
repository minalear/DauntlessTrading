using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Planetoid
    {
        public System System { get; set; }
        public Planetoid Parent { get; set; }
        public string Name { get; set; }
        public List<Planetoid> Moons { get; set; }
        public List<Station> Stations { get; set; }

        public Planetoid(System system, string name)
        {
            System = system;
            Name = name;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();
        }
        public Planetoid(System system, string name, Planetoid parent)
        {
            System = system;
            Name = name;
            Parent = parent;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();

            Parent.Moons.Add(this);
        }
    }
}
