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
        }

        public static ShipMod MaverickCockpitI;
    }
}
