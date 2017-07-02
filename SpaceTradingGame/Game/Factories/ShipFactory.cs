using System;

namespace SpaceTradingGame.Game.Factories
{
    public static class ShipFactory
    {
        public static void Init()
        {
            Ship MaverickMkI = new Ship("Blueprint", "Maverick Mk I", 15, 10, 100, 500);
            MaverickMkI.Nodes.Add(new ShipNode(1, 0, ShipMod.ShipModTypes.Cockpit));
            MaverickMkI.Nodes.Add(new ShipNode(0, 2, ShipMod.ShipModTypes.CargoBay));
            MaverickMkI.Nodes.Add(new ShipNode(2, 2, ShipMod.ShipModTypes.WarpCore));

            Ship MaverickMkII = new Ship("Blueprint", "Maverick Mk II", 20, 15, 135, 850);
            MaverickMkII.Nodes.Add(new ShipNode(1, 0, ShipMod.ShipModTypes.Cockpit));
            MaverickMkII.Nodes.Add(new ShipNode(1, 2, ShipMod.ShipModTypes.Any));
            MaverickMkII.Nodes.Add(new ShipNode(0, 4, ShipMod.ShipModTypes.CargoBay));
            MaverickMkII.Nodes.Add(new ShipNode(2, 4, ShipMod.ShipModTypes.WarpCore));

            Ship DelpheneI = new Ship("Blueprint", "Delphene I", 8, 12, 140, 550);
            Ship DelpheneII = new Ship("Blueprint", "Delphene II", 12, 20, 220, 850);
            Ship Dauntless = new Ship("Blueprint", "Dauntless Class", 25, 50, 800, 1350);
            Dauntless.Nodes.Add(new ShipNode(2, 0, ShipMod.ShipModTypes.Cockpit));
            Dauntless.Nodes.Add(new ShipNode(0, 2, ShipMod.ShipModTypes.Weapon));
            Dauntless.Nodes.Add(new ShipNode(2, 2, ShipMod.ShipModTypes.Any));
            Dauntless.Nodes.Add(new ShipNode(4, 2, ShipMod.ShipModTypes.Weapon));
            Dauntless.Nodes.Add(new ShipNode(2, 4, ShipMod.ShipModTypes.CargoBay));
            Dauntless.Nodes.Add(new ShipNode(0, 6, ShipMod.ShipModTypes.Any));
            Dauntless.Nodes.Add(new ShipNode(2, 6, ShipMod.ShipModTypes.WarpCore));
            Dauntless.Nodes.Add(new ShipNode(4, 6, ShipMod.ShipModTypes.Any));
            Dauntless.Nodes.Add(new ShipNode(0, 8, ShipMod.ShipModTypes.Any));
            Dauntless.Nodes.Add(new ShipNode(4, 8, ShipMod.ShipModTypes.Any));

            

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
