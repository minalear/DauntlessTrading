using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class System
    {
        public string Name { get; set; }
        public List<Planetoid> Planetoids { get; set; }

        public System(string name)
        {
            Name = name;
            Planetoids = new List<Planetoid>();
        }
    }
}
