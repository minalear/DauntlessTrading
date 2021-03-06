﻿using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class StarSystem : Engine.UI.Controls.ListItem, IComparable
    {
        public int ID { get; set; }
        public double WeightedValue { get; set; }
        public string Name { get; set; }
        public Color4 StarColor { get; set; }
        public List<Planetoid> Planetoids { get; set; }
        public List<Ship> VisitingShips { get; private set; }

        public Vector2 Coordinates { get; set; }
        public Point MapCoord { get; set; }

        public Market Market { get; set; }
        public bool HasMarket { get; set; }

        public StarSystem(string name)
        {
            ID = _nextValidID++;
            Name = name;
            Planetoids = new List<Planetoid>();
            VisitingShips = new List<Ship>();

            Coordinates = Vector2.Zero;
            StarColor = colors[RNG.Next(0, colors.Length)];

            this.ListText = Name;
        }

        public void UpdateStarSystem(double days)
        {
            foreach (Planetoid planet in Planetoids)
                planet.UpdatePlanetoid(days);
        }
        public void BuildMarket(Faction owner)
        {
            HasMarket = true;
            Market = new Market(this, owner);
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

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(StarSystem)) return 0;
            return this.WeightedValue.CompareTo(((StarSystem)obj).WeightedValue);
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, WeightedValue);
        }

        private static int _nextValidID = 0;
        private static Color4[] colors = { Color4.Red, Color4.Orange, Color4.Yellow, Color4.Cyan, Color4.Blue, Color4.White };

        public static void ResetIDCounter()
        {
            _nextValidID = 0;
        }
    }
}
