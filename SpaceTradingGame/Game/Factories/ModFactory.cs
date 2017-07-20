using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SpaceTradingGame.Game.Factories
{
    public static class ModFactory
    {
        public static void Init()
        {
            ModList = JsonConvert.DeserializeObject<List<ShipMod>>(File.ReadAllText("Content/modules.json"));
            CalculateModuleValues();
        }
        public static void CalculateModuleValues()
        {
            foreach (ShipMod mod in ModList)
            {
                mod.BaseValue = mod.Grade * 1000;
                mod.Weight = DEFAULT_MODULE_WEIGHT;
            }
        }
        
        public static List<ShipMod> ModList;
        public const int DEFAULT_MODULE_WEIGHT = 250;
    }
}
