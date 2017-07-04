using System;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Station
    {
        public Planetoid Parent { get; private set; }
        public int Level { get; private set; }
        public double DrillRate { get; private set; }
        public Faction Owner { get; private set; }

        public Station(Planetoid parent, Faction owner, int level)
        {
            Parent = parent;
            DrillRate = 1.0;

            for (int i = 1; i < level; i++)
                LevelUp();

            SetOwner(owner);
        }

        public void UpdateSpaceStation()
        {
            if (!Parent.System.HasMarket) return;

            foreach (MaterialDeposit deposit in Parent.MaterialDeposits)
            {
                double var = RNG.NextDouble(0.9, 1.1);

                double amount = (int)(deposit.Density * var * DrillRate * 100.0);

                //Limit production based on existing resources at the market
                double softCap = (int)(deposit.Material.Rarity * 100.0);
                double stockMod = -(1.0 / softCap) * 
                    (Parent.System.SystemMarket.MarketInventory.GetQuantity(deposit.Material) + amount) + 1.0;
                amount = amount * stockMod;

                Parent.System.SystemMarket.MarketInventory.AddItem(deposit.Material, (int)amount);
            }
        }
        public void LevelUp()
        {
            Level++;
            DrillRate *= 1.1;
        }
        public int LevelUpCost()
        {
            return (Level + 1) * (Level + 1) * 5000;
        }
        public void SetOwner(Faction faction)
        {
            Owner = faction;
            Owner.OwnedStations.Add(this);
        }
    }
}
