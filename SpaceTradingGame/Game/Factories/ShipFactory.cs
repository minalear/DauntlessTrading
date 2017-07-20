using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SpaceTradingGame.Engine;
using Newtonsoft.Json;

namespace SpaceTradingGame.Game.Factories
{
    public static class ShipFactory
    {
        public static void Init()
        {
            ShipBlueprints = new List<Ship>();
            BasicShips = new List<Ship>();

            List<ShipJson> ships = JsonConvert.DeserializeObject<List<ShipJson>>(File.ReadAllText("Content/ships.json"));
            foreach (ShipJson blueprint in ships)
            {
                Ship ship = new Ship("Blueprint", blueprint.Model);
                ship.Description = blueprint.Description;

                foreach (NodeJson nodeBlueprint in blueprint.Nodes)
                {
                    ShipNode node = new ShipNode(nodeBlueprint.X, nodeBlueprint.Y,
                        (ShipMod.ShipModTypes)Enum.Parse(typeof(ShipMod.ShipModTypes), nodeBlueprint.Type));
                    ship.Nodes.Add(node);
                }

                //Equip basic modules #0 - 2
                ship.EquipModule(ModFactory.ModList[0], false);
                ship.EquipModule(ModFactory.ModList[1], false);
                ship.EquipModule(ModFactory.ModList[2], false);

                ship.UpdateShipStats();

                ShipBlueprints.Add(ship);
                if (blueprint.Basic)
                    BasicShips.Add(ship);
            }
        }
        public static Ship ConstructNewShip(string model)
        {
            foreach (Ship ship in ShipBlueprints)
            {
                if (ship.Model == model)
                {
                    return (Ship)ship.Clone();
                }
            }

            throw new ArgumentException(string.Format("Invalid model type: {0}.", model));
        }
        public static Ship ConstructRandomShip()
        {
            string randomModel = ShipBlueprints[RNG.Next(0, ShipBlueprints.Count)].Model;
            Ship ship = ConstructNewShip(randomModel);

            //Equip basic modules for each node
            foreach (ShipNode node in ship.Nodes)
            {
                if (!node.Empty) continue;

                if (node.ModType == ShipMod.ShipModTypes.Any)
                {
                    //Equip random module woo!
                    node.Empty = false;
                    node.Module = ModFactory.ModList[RNG.Next(0, ModFactory.ModList.Count)];
                }
                else
                {
                    foreach (ShipMod mod in ModFactory.ModList)
                    {
                        if (mod.ModType == node.ModType)
                        {
                            node.Empty = false;
                            node.Module = mod;
                        }
                    }
                }
            }

            ship.UpdateShipStats();
            return ship;
        }
        public static string GenerateRandomShipName()
        {
            StringBuilder name = new StringBuilder();
            if (RNG.Next(0, 100) < 90) name.Append("The ");

            if (RNG.Next(0, 100) < 75)
            {
                name.Append(Adjectives[RNG.Next(0, Adjectives.Length)]);
                name.Append(" ");
            }
            name.Append(Nouns[RNG.Next(0, Nouns.Length)]);

            return name.ToString();
        }

        private static string[] Adjectives = new string[] {
            "Angry", "Marvelous", "Miserly", "Watchman's", "Official", "Fearful", "Ravenous", "Dystopian",
            "Mad", "Enchanted", "Dishonorable", "Corporeal", "Incorporeal", "Enchanted", "Traveling",
            "Philosophical", "Harmonios", "Boisterous", "Grim", "Boastful", "Impossible", "Possible"
        };
        private static string[] Nouns = new string[] {
            "Disease", "Federation", "Enterprise", "Summoner", "Forest", "Wench", "Imposter", "Sojourner",
            "Beastmaster", "Ghost", "Panda", "Paladin", "Archmage", "Saint", "Valkyrie", "Knight",
            "Seraphim", "Raider", "Blacksmith", "Horizon", "Murderer", "Cleric", "Hoard", "Boat"
        };

        public static List<Ship> BasicShips, ShipBlueprints;

        private class ShipJson
        {
            public string Model;
            public NodeJson[] Nodes;
            public string Description;
            public bool Basic = false;
        }
        private class NodeJson
        {
            public int X;
            public int Y;
            public string Type;
        }
    }
}
