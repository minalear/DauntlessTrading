using System;
using OpenTK.Graphics;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Controls.Custom
{
    public class ShipLayout : Control
    {
        private Ship ship;

        public ShipLayout(Control parent, int width, int height) : base(parent)
        {
            FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);
            this.Size = new System.Drawing.Point(width, height);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColor(Color4.Black, FillColor);
            GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            base.DrawStep();
        }
        public void SetShip(Ship ship)
        {
            this.ship = ship;
            this.Children.Clear();

            int layoutWidth = 0, layoutHeight = 0;
            foreach (ShipNode node in ship.Nodes)
            {
                layoutWidth = (node.X * 3 > layoutWidth) ? node.X * 3 : layoutWidth;
                layoutHeight = (node.Y * 2 > layoutHeight) ? node.Y * 2 : layoutHeight;
            }

            foreach (ShipNode node in ship.Nodes)
            {
                int x = (node.X * 3 - 1) + this.Size.X / 2 - layoutWidth / 2;
                int y = (node.Y * 2 - 1) + this.Size.Y / 2 - layoutHeight / 2;

                string buttonText = (node.Empty) ? " " : "X";
                Button nodeButton = new Button(this, " ", x, y, 3, 2);
                nodeButton.FillColor = new Color4(0.3f, 0.3f, 0.3f, 1f);

                nodeButton.Click += (sender, e) => nodeClick(node);
            }

            InterfaceManager.DrawStep();
        }

        private void nodeClick(ShipNode node)
        {
            this.NodeSelect?.Invoke(this, new NodeSelectEventArgs(node));
        }

        public delegate void NodeSelectDelegate(object sender, NodeSelectEventArgs e);
        public NodeSelectDelegate NodeSelect;

        public Color4 FillColor { get; set; }
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
