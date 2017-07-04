using System;

namespace SpaceTradingGame.Game
{
    public class Blueprint : Product
    {
        public ShipMod Modification { get; private set; }

        public Blueprint(ShipMod mod)
            : base(mod.Name, mod.Description)
        {
            Modification = mod;
            AddRequirement(Item.Silver, 100);
            AddRequirement(Item.Platinum, 10);
            AddRequirement(Item.Gold, 50);
            AddRequirement(Item.Plutonium, 10);
        }

        public override Item Produces()
        {
            return Modification;
        }
    }
}
