using System;
using System.Collections.Generic;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TradingInterface : Interface
    {
        private Market market;
        private double credits = 10000;

        public TradingInterface(InterfaceManager manager)
            : base(manager)
        {
            //Interface Initialization
            Title title = new Title(null, "==-Trading Interface-==", 50, 1, Title.TextAlignModes.Center);

            RegisterControl(title);

            //Game Mechanics
            market = new Market();
        }

        public override void OnEnable()
        {
            int id = 0;

            Title banner = new Title(null, "NAME          QNTY           PRCE", 1, 2, Title.TextAlignModes.Left);
            RegisterControl(banner);
            foreach (KeyValuePair<Material, Market.MetaInfo> materials in market.Materials)
            {
                int y = id * 2 + 4;

                Title title = new Title(null, materials.Key.Name, 1, y, Title.TextAlignModes.Left);
                Title amount = new Title(null, materials.Value.Amount.ToString(), 15, y, Title.TextAlignModes.Left);
                Title price = new Title(null, materials.Value.Price.ToString(), 30, y, Title.TextAlignModes.Left);

                Button buyButton = new Button(null, "Buy", 45, y, 5, 1);
                buyButton.Click += (sender, e) =>
                {
                    market.Buy(materials.Key, 10);
                    amount.Text = materials.Value.Amount.ToString();
                    price.Text = materials.Value.Price.ToString();

                    InterfaceManager.DrawStep();
                };
                Button sellButton = new Button(null, "Sell", 52, y, 6, 1);
                sellButton.Click += (sender, e) =>
                {
                    market.Sell(materials.Key, 10);
                    amount.Text = materials.Value.Amount.ToString();
                    price.Text = materials.Value.Price.ToString();

                    InterfaceManager.DrawStep();
                };

                RegisterControl(title);
                RegisterControl(amount);
                RegisterControl(price);
                RegisterControl(buyButton);
                RegisterControl(sellButton);

                id++;
            }

            base.OnEnable();
        }
    }
}
