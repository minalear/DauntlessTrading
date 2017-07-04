﻿using System;

namespace SpaceTradingGame.Game
{
    public class Factory
    {
        public Planetoid Paret { get; private set; }
        public int Level { get; private set; }
        public double ProductionRate { get; private set; }
        public Faction Owner { get; private set; }
        public Product MainProduction { get; private set; }

        public Factory(Planetoid parent, Faction owner, Product product, int level)
        {
            Paret = parent;
            ProductionRate = 1.0;

            for (int i = 1; i < level; i++)
                LevelUp();

            SetOwner(owner);
            SetProduct(product);
        }

        public void UpdateFactory()
        {
            Market market = Paret.System.SystemMarket;

            if (!Paret.System.HasMarket) return;
            if (!MainProduction.CanProduce(market.MarketInventory, true)) return;

            foreach (Product.RequirementInfo req in MainProduction.Requirements)
            {
                //Subtract materials from market inventory (later bill faction)
                market.MarketInventory.RemoveItem(req.Item, req.Quantity);
            }

            //Build product
            market.MarketInventory.AddItem(MainProduction, MainProduction.UnitsProducted);
        }

        public void SetOwner(Faction owner)
        {
            Owner = owner;
            Owner.OwnedFactories.Add(this);
        }
        public void LevelUp()
        {
            Level++;
            ProductionRate *= 1.05;
        }
        public void SetProduct(Product product)
        {
            MainProduction = product;
        }
    }
}
