using System;
using System.Drawing;
using System.Collections.Generic;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TestInterface : Interface
    {
        public TestInterface(InterfaceManager manager)
            : base(manager)
        {
            generateStars();

            for (int i = 0; i < stars.Count; i++)
            {
                Button starButton = new Button(null, "0", stars[i].X, stars[i].Y, 1, 1);
                starButton.Click += (sender, e) =>
                {
                    drawLines = true;
                    Control button = (Control)sender;
                    selectedStar = new Point(button.Position.X, button.Position.Y);

                    InterfaceManager.DrawStep();
                };
                RegisterControl(starButton);
            }
        }

        public override void DrawStep()
        {
            if (drawLines)
            {
                GraphicConsole.SetColor(Color.Red, Color.Black);
                for (int i = 0; i < stars.Count; i++)
                {
                    if (distance(stars[i], selectedStar) <= 8.0)
                    {
                        GraphicConsole.Draw.Line(stars[i].X, stars[i].Y, selectedStar.X, selectedStar.Y, '.');
                    }
                }
                GraphicConsole.ClearColor();
            }

            for (int i = 0; i < stars.Count; i++)
            {
                GraphicConsole.Put('0', stars[i].X, stars[i].Y);
            }

            base.DrawStep();
        }
        private void generateStars()
        {
            int numStars = RNG.Next(80, 100);
            stars = new List<Point>();

            for (int i = 0; i < numStars; i++)
            {
                stars.Add(new Point(
                    RNG.Next(0, GraphicConsole.BufferWidth),
                    RNG.Next(0, GraphicConsole.BufferHeight)));
            }
        }
        private double distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private bool drawLines = false;
        private Point selectedStar;
        private List<Point> stars;
    }
}
