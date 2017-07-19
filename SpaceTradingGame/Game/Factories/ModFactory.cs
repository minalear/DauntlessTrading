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
        }
        
        public static List<ShipMod> ModList;
    }
}
