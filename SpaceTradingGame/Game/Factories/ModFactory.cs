using System.Text;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SpaceTradingGame.Game.Factories
{
    public static class ModFactory
    {
        public static void Init()
        {
            List<ShipMod> mods = JsonConvert.DeserializeObject<List<ShipMod>>(File.ReadAllText("Content/Modules/basic_cockpit.json"));

            MaverickCockpitI = new ShipMod(ShipMod.ShipModTypes.Cockpit);
            MaverickCockpitI.Name = "Maverick Cockpit";
            MaverickCockpitI.Grade = 1;
            MaverickCockpitI.BaseValue = 100;
            MaverickCockpitI.FirePowerMod = 5;
            MaverickCockpitI.DefenseMod = 5;
            MaverickCockpitI.Description = "Cockpit modification developed by the Maverick Corporation.";

            EuripidesWarpCore = new ShipMod(ShipMod.ShipModTypes.WarpCore);
            EuripidesWarpCore.Name = "Euripides Warp Engine";
            EuripidesWarpCore.Grade = 3;
            EuripidesWarpCore.BaseValue = 10000;
            EuripidesWarpCore.Description = "Advanced Warp Core produced by Dauntless Inc.";
            EuripidesWarpCore.JumpMod = 1000;

            BasicCockpit = new ShipMod(ShipMod.ShipModTypes.Cockpit);
            BasicCockpit.Name = "Terra Dynamics Cockpit";
            BasicCockpit.Description = "Cockpit designed by Terra Dynamics.  Designed to be mass produced and cheap, it serves as the basis for all entry level starships and is the defacto standard.";
            BasicCockpit.DefenseMod = 10;

            BasicCargoBay = new ShipMod(ShipMod.ShipModTypes.CargoBay);
            BasicCargoBay.Name = "Terra Dyanmics Cargo Bay";
            BasicCargoBay.Description = "Cargo Bay designed by Terra Dynamics.  Designed to be mass produced and cheap, it serves as the basis for all entry level starships and is the defacto standard.";
            BasicCargoBay.CargoMod = 100;

            BasicWarpCore = new ShipMod(ShipMod.ShipModTypes.WarpCore);
            BasicWarpCore.Name = "Earl's Warp Core";
            BasicWarpCore.Description = "Scavenged from Earl's backyard, this Warp Core design is the very basic warp design that provides very minimal jump ranges and poor fuel efficiency.";
            BasicWarpCore.JumpMod = 650;

            ModList = new ShipMod[] {
                MaverickCockpitI, EuripidesWarpCore, BasicCockpit, BasicCargoBay, BasicWarpCore
            };
        }

        public static ShipMod MaverickCockpitI;
        public static ShipMod EuripidesWarpCore;

        public static ShipMod BasicCockpit, BasicCargoBay, BasicWarpCore;

        public static ShipMod[] ModList;
    }
}
