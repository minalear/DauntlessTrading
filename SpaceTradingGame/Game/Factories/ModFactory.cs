using System;

namespace SpaceTradingGame.Game.Factories
{
    public static class ModFactory
    {
        public static void Init()
        {
            MaverickCockpitI = new ShipMod(ShipMod.ShipModTypes.Cockpit);
            MaverickCockpitI.Name = "Maverick Cockpit";
            MaverickCockpitI.Grade = 1;
            MaverickCockpitI.BaseValue = 100;
            MaverickCockpitI.Description = "Cockpit modification developed by the Maverick Corporation.";

            EuripidesWarpCore = new ShipMod(ShipMod.ShipModTypes.WarpCore);
            EuripidesWarpCore.Name = "Euripides Warp Engine";
            EuripidesWarpCore.Grade = 3;
            EuripidesWarpCore.BaseValue = 10000;
            EuripidesWarpCore.Description = "Advanced Warp Core produced by Dauntless Inc.";
        }

        public static ShipMod MaverickCockpitI;
        public static ShipMod EuripidesWarpCore;
    }
}
