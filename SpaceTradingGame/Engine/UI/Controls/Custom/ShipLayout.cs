using System;
using OpenTK.Graphics;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Controls.Custom
{
    public class ShipLayout : Control
    {
        private Ship ship;

        public ShipLayout(Control parent) : base(parent)
        {
            
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            this.Children.Clear();

            foreach (ShipNode node in ship.Nodes)
            {
                Button nodeButton = new Button(this, " ", node.X * 3, node.Y * 2, 3, 2);
                nodeButton.FillColor = Color4.Red;
                this.Children.Add(nodeButton);
            }

            InterfaceManager.DrawStep();
        }
    }
}
