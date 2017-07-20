namespace SpaceTradingGame.Game
{
    public class Blueprint : Product
    {
        public ShipMod Module { get; private set; }

        public Blueprint(ShipMod mod)
            : base(mod.Name, mod.Description)
        {
            Module = mod;
            AddRequirement(Item.Silver, 100);
            AddRequirement(Item.Platinum, 10);
            AddRequirement(Item.Gold, 50);
            AddRequirement(Item.Plutonium, 10);
        }

        public override Item Produces()
        {
            return Module;
        }
    }
}
