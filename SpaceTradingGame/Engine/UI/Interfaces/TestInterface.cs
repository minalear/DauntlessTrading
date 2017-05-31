using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
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
                Point point = getMapPosition(stars[i].Coordinate);
                Button starButton = new Button(null, "☼", point.X, point.Y, 1, 1);
                starButton.Click += (sender, e) =>
                {
                    drawLines = true;
                    Control button = (Control)sender;
                    //selectedStar = new Point(button.Position.X, button.Position.Y);

                    InterfaceManager.DrawStep();
                };
                RegisterControl(starButton);
            }
        }

        public override void DrawStep()
        {
            if (drawLines)
            {
                GraphicConsole.SetColor(Color.Gray, Color.Black);
                for (int i = 0; i < stars.Count; i++)
                {
                    if (distance(stars[i].Coordinate, selectedStar.Coordinate) <= 15.0)
                    {
                        Point pointA = getMapPosition(stars[i].Coordinate);
                        Point pointB = getMapPosition(selectedStar.Coordinate);

                        GraphicConsole.Draw.Line(pointA.X, pointA.Y, pointB.X, pointB.Y, '.');
                    }
                }
                GraphicConsole.ClearColor();
            }

            for (int i = 0; i < stars.Count; i++)
            {
                Point point = getMapPosition(stars[i].Coordinate);
                GraphicConsole.Put('☼', point.X, point.Y);
            }

            base.DrawStep();
        }
        private void generateStars()
        {
            int numStars = RNG.Next(25, 50);
            stars = new List<StarSystem>();

            for (int i = 0; i < numStars; i++)
            {
                stars.Add(new StarSystem());
            }
        }
        private double distance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        private Point getMapPosition(Vector2 coordinate)
        {
            int x = (int)(coordinate.X / GraphicConsole.BufferWidth);
            int y = (int)(coordinate.Y / GraphicConsole.BufferHeight);

            return new Point(x, y);
        }

        private bool drawLines = false;
        private StarSystem selectedStar;
        private List<StarSystem> stars;

        public class StarSystem
        {
            public string Name;
            public Color4 StarColor;
            public Vector2 Coordinate;

            public StarSystem()
            {
                Name = prefix[RNG.Next(0, prefix.Length)] + suffix[RNG.Next(0, suffix.Length)];
                StarColor = colors[RNG.Next(0, colors.Length)];

                Coordinate = new Vector2(
                    RNG.NextFloat(-1000.0f, 1000.0f),
                    RNG.NextFloat(-1000.0f, 1000.0f));
            }

            private static string[] prefix = { "Car", "Bar", "Gnar", "Var", "Uia", "Mar", "Lua", "Mua" };
            private static string[] suffix = { "ia", "bia", "y", "ly", "io", "ioup", "mup" };
            private static Color4[] colors = { Color4.Red, Color4.Orange, Color4.Yellow, Color4.Cyan, Color4.Blue };
        }
    }
}
