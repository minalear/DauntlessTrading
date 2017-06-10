using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class SystemScreen : Interface
    {
        public SystemScreen(InterfaceManager manager)
            : base(manager)
        {
            genPlanets();
        }

        public override void DrawStep()
        {
            Color4[] starColors = new Color4[] {
                new Color4(255, 255, 0, 255),
                new Color4(255, 255, 153, 255),
                new Color4(252, 232, 131, 255),
                new Color4(255, 215, 0, 255),
                new Color4(255, 128, 0, 255),
                new Color4(255, 255, 153, 255),
                new Color4(252, 232, 131, 255),
                new Color4(255, 215, 0, 255),
                new Color4(220, 20, 60, 255),
                new Color4(255, 255, 153, 255),
                new Color4(252, 232, 131, 255),
                new Color4(255, 215, 0, 255),
                new Color4(255, 255, 0, 255),
                new Color4(255, 255, 153, 255),
                new Color4(252, 232, 131, 255)
            };

            //Draw the star
            int r = 15;
            for (int i = 0; i < r; i++)
            {
                GraphicConsole.SetColor(starColors[i], Color4.Black);
                GraphicConsole.Draw.Circle(0, GraphicConsole.BufferHeight / 2, i, '.');
            }
            GraphicConsole.Draw.Circle(0, GraphicConsole.BufferHeight / 2, r, '*');

            //Draw the planets
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    int i = (y * 4) + x;
                    Planet planet = planets[i];


                    for (int j = 0; j < planet.R; j++)
                    {
                        Color4 variatedColor = variateColor(planet.Color);
                        GraphicConsole.SetColor(variatedColor, Color4.Black);
                        GraphicConsole.Draw.Circle(x * 20 + 25, y * 15 + 10, j, '.');
                    }

                    GraphicConsole.SetColor(planet.Color, Color4.Black);
                    GraphicConsole.Draw.Circle(x * 20 + 25, y * 15 + 10, planet.R, '.');
                }
            }

            GraphicConsole.ClearColor();

            base.DrawStep();
        }

        private void genPlanets()
        {
            Color4[] colors = new Color4[] { Color4.Blue, Color4.Red, Color4.Yellow, Color4.Cyan };
            int num = 8;

            planets = new List<Planet>();
            for (int i = 0; i < num; i++)
            {
                planets.Add(new Planet() { R = RNG.Next(5, 5), Color = colors[RNG.Next(0, colors.Length)] });
            }
        }
        private Color4 variateColor(Color4 color)
        {
            int varR = (RNG.Next(0, 101) <= 50) ? 1 : -1;
            int varG = (RNG.Next(0, 101) <= 50) ? 1 : -1;
            int varB = (RNG.Next(0, 101) <= 50) ? 1 : -1;

            return new Color4(
                color.R + (color.R * 0.4f) * varR,
                color.G + (color.G * 0.4f) * varG,
                color.B + (color.B * 0.4f) * varB,
                1f);
        }

        struct Planet
        {
            public int R;
            public Color4 Color;
        }
        private List<Planet> planets;
    }
}
