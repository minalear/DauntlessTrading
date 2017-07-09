using System;
using SpaceTradingGame.Game;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls.Custom
{
    public class StockMarketChart : Control
    {
        private Faction faction;

        public Color4 FillColor { get; set; }
        public Color4 StripeColor { get; set; }

        public StockMarketChart(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            setPosition(new System.Drawing.Point(x, y));
            Size = new System.Drawing.Point(width, height);

            FillColor = Color4.Black;
            StripeColor = new Color4(0.125f, 0.125f, 0.125f, 1f);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColor(Color4.Transparent, FillColor);
            GraphicConsole.Draw.Rect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

            GraphicConsole.SetColor(Color4.Transparent, StripeColor);
            for (int x = Position.X + Size.X - 1; x >= Position.X; x -= 5)
            {
                GraphicConsole.Draw.Line(x, Position.Y, x, Position.Y + Size.Y - 1, ' ');
            }

            if (faction == null) return;

            int scaleX = 5;
            int scaleY = max(0, 100) / (Size.Y - 1);

            if (scaleY == 0) return; //Prevent divide by zero exception
            
            int loop = 0;
            for (int i = faction.StockPrices.Count - 1; i > 0 && loop < Size.X / 5; i--)
            {
                int x0 = (Position.X + Size.X - 1) - loop * scaleX;
                int x1 = (Position.X + Size.X - 1) - (loop + 1) * scaleX;

                int y0 = (Position.Y + Size.Y - 1) - faction.StockPrices[i] / scaleY;
                int y1 = (Position.Y + Size.Y - 1) - faction.StockPrices[i - 1] / scaleY;

                GraphicConsole.SetColor(faction.RegionColor, FillColor);
                GraphicConsole.Draw.Line(x0, y0, x1, y1, '.');

                GraphicConsole.SetColor(faction.RegionColor, StripeColor);
                GraphicConsole.Put('*', x0, y0);
                GraphicConsole.Put('*', x1, y1);

                loop++;
            }

            GraphicConsole.ClearColor();

            base.DrawStep();
        }
        public void SetFaction(Faction faction)
        {
            this.faction = faction;
        }

        private int max(int start, int end)
        {
            int max = 0;

            for (int i = start; i <= end && i < faction.StockPrices.Count; i++)
            {
                if (faction.StockPrices[i] > max)
                    max = faction.StockPrices[i];
            }

            return max;
        }
    }
}
