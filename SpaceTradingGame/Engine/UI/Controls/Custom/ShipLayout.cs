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

                nodeButton.Click += (sender, e) => nodeClick(node);

                this.Children.Add(nodeButton);
            }

            InterfaceManager.DrawStep();
        }

        private void nodeClick(ShipNode node)
        {
            this.NodeSelect?.Invoke(this, new NodeSelectEventArgs(node));
        }

        public delegate void NodeSelectDelegate(object sender, NodeSelectEventArgs e);
        public NodeSelectDelegate NodeSelect;
    }

    public class NodeSelectEventArgs : EventArgs
    {
        public ShipNode SelectedShipNode { get; private set; }

        public NodeSelectEventArgs(ShipNode node)
        {
            SelectedShipNode = node;
        }
    }
}
