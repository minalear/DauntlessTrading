using System;

namespace SpaceTradingGame.Game.Factories
{
    public static class ShipFactory
    {
        public static void Init()
        {
            //Basic ship setup
            Ship MaverickMkI = new Ship("Blueprint", "Maverick Mk I");
            MaverickMkI.Nodes.Add(new ShipNode(1, 0, ShipMod.ShipModTypes.Cockpit));
            MaverickMkI.Nodes.Add(new ShipNode(0, 2, ShipMod.ShipModTypes.CargoBay));
            MaverickMkI.Nodes.Add(new ShipNode(2, 2, ShipMod.ShipModTypes.WarpCore));

            Ship MaverickMkII = new Ship("Blueprint", "Maverick Mk II");
            MaverickMkII.Nodes.Add(new ShipNode(1, 0, ShipMod.ShipModTypes.Cockpit));
            MaverickMkII.Nodes.Add(new ShipNode(1, 2, ShipMod.ShipModTypes.Any));
            MaverickMkII.Nodes.Add(new ShipNode(0, 4, ShipMod.ShipModTypes.CargoBay));
            MaverickMkII.Nodes.Add(new ShipNode(2, 4, ShipMod.ShipModTypes.WarpCore));

            Ship DelpheneI = new Ship("Blueprint", "Delphene I");
            Ship DelpheneII = new Ship("Blueprint", "Delphene II");
            Ship Dauntless = new Ship("Blueprint", "Dauntless Class");
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

            Ship Exodia = new Ship("Blueprint", "Exodia Class");

            //Equip Modifications
            MaverickMkI.EquipModification(ModFactory.BasicCockpit, false);
            MaverickMkI.EquipModification(ModFactory.BasicCargoBay, false);
            MaverickMkI.EquipModification(ModFactory.BasicWarpCore, false);

            MaverickMkII.EquipModification(ModFactory.BasicCockpit, false);
            MaverickMkII.EquipModification(ModFactory.BasicCargoBay, false);
            MaverickMkII.EquipModification(ModFactory.BasicWarpCore, false);

            Dauntless.EquipModification(ModFactory.BasicCockpit, false);
            Dauntless.EquipModification(ModFactory.BasicCargoBay, false);
            Dauntless.EquipModification(ModFactory.BasicWarpCore, false);

            ShipBlueprints = new Ship[] { MaverickMkI, MaverickMkII, DelpheneI, DelpheneII, Dauntless, Exodia };
            for (int i = 0; i < ShipBlueprints.Length; i++)
                ShipBlueprints[i].UpdateShipStats();
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
