using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class StarSystem : Engine.UI.Controls.ListItem
    {
        private static int _nextValidID = 0;

        public int ID { get; set; }
        public string Name { get; set; }
        public Color4 StarColor { get; set; }
        public List<Planetoid> Planetoids { get; set; }

        public Vector2 Coordinates { get; set; }
        public Point MapCoord { get; set; }

        public Market SystemMarket { get; set; }
        public bool HasMarket { get; set; }

        public StarSystem(string name)
        {
            ID = _nextValidID++;
            Name = name;
            Planetoids = new List<Planetoid>();

            Coordinates = Vector2.Zero;
            StarColor = colors[RNG.Next(0, colors.Length)];

            this.ListText = Name;
        }

        public void UpdateStarSystem()
        {
            foreach (Planetoid planet in Planetoids)
                planet.UpdatePlanetoid();
        }
        public void BuildMarket(Faction owner)
        {
            HasMarket = true;
            SystemMarket = new Market(this, owner);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(StarSystem))
                return ((StarSystem)obj).ID == ID;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static Color4[] colors = { Color4.Red, Color4.Orange, Color4.Yellow, Color4.Cyan, Color4.Blue, Color4.White };
    }
}
