﻿using System;

namespace SpaceTradingGame.Game.Factories
{
    public static class ShipFactory
    {
        public static void Init()
        {
            Ship MaverickMkI = new Ship("Blueprint", "Maverick Mk I", 15, 10, 100, 500);
            MaverickMkI.Nodes.Add(new ShipNode(1, 0));
            MaverickMkI.Nodes.Add(new ShipNode(0, 2));
            MaverickMkI.Nodes.Add(new ShipNode(2, 2));

            Ship MaverickMkII = new Ship("Blueprint", "Maverick Mk II", 20, 15, 135, 850);
            MaverickMkII.Nodes.Add(new ShipNode(1, 0));
            MaverickMkII.Nodes.Add(new ShipNode(1, 2));
            MaverickMkII.Nodes.Add(new ShipNode(0, 4));
            MaverickMkII.Nodes.Add(new ShipNode(2, 4));

            Ship DelpheneI = new Ship("Blueprint", "Delphene I", 8, 12, 140, 550);
            Ship DelpheneII = new Ship("Blueprint", "Delphene II", 12, 20, 220, 850);
            Ship Dauntless = new Ship("Blueprint", "Dauntless Class", 25, 50, 800, 1350);
            Ship Exodia = new Ship("Blueprint", "Exodia Class", 80, 100, 2400, 2500);

            ShipBlueprints = new Ship[] { MaverickMkI, MaverickMkII, DelpheneI, DelpheneII, Dauntless, Exodia };
        }

        public static Ship MaverickMkI;
        public static Ship MaverickMkII;
        public static Ship DelpheneI;
        public static Ship DelpheneII;
        public static Ship Dauntless;
        public static Ship Exodia;

        public static Ship[] ShipBlueprints;
    }
}