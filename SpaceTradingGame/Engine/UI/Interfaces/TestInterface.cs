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
                if (point.X >= 0 && point.Y >= 0 && point.X < GraphicConsole.BufferWidth && point.Y < GraphicConsole.BufferHeight)
                {
                    Button starButton = new Button(null, "☼", point.X, point.Y, 1, 1);

                    int index = i; //For lambda function

                    starButton.Click += (sender, e) =>
                    {
                        drawLines = true;
                        Control button = (Control)sender;
                        selectedStar = stars[index];

                        title.Text = selectedStar.Name;

                        InterfaceManager.DrawStep();
                    };
                    RegisterControl(starButton);
                }
                else
                {
                    stars.RemoveAt(i);
                    i--;
                }
            }

            title = new Title(null, "System", GraphicConsole.BufferWidth / 2, 1);
            RegisterControl(title);
        }

        public override void DrawStep()
        {
            if (drawLines)
            {
                GraphicConsole.SetColor(Color.Gray, Color.Black);

                Point pointA = getMapPosition(selectedStar.Coordinate);

                double travelRadius = 1000.0;
                int r = (int)(travelRadius / GraphicConsole.BufferWidth);
                GraphicConsole.Draw.Circle(pointA.X, pointA.Y, r, '.');

                GraphicConsole.SetColor(Color.Red, Color.Black);
                for (int i = 0; i < stars.Count; i++)
                {
                    double dist = distance(stars[i].Coordinate, selectedStar.Coordinate);
                    if (dist <= travelRadius)
                    {
                        Point pointB = getMapPosition(stars[i].Coordinate);
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
            int numStars = RNG.Next(50, 100);
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
            //Due to the width/height of each cell being different, we need to ensure that the Y is calculated off the BufferWidth
            int x = (int)(coordinate.X / GraphicConsole.BufferWidth);
            int y = (int)(coordinate.Y / GraphicConsole.BufferWidth);

            return new Point(x, y);
        }

        private bool drawLines = false;
        private StarSystem selectedStar;
        private List<StarSystem> stars;

        private Title title;

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
                    RNG.NextFloat(0.0f, 10000.0f),
                    RNG.NextFloat(0.0f, 5000.0f));
            }

            private static string[] prefix = { "Car", "Bar", "Gnar", "Var", "Uia", "Mar", "Lua", "Mua" };
            private static string[] suffix = { "ia", "bia", "y", "ly", "io", "ioup", "mup" };
            private static Color4[] colors = { Color4.Red, Color4.Orange, Color4.Yellow, Color4.Cyan, Color4.Blue };
        }
    }
}
