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
    }
}
