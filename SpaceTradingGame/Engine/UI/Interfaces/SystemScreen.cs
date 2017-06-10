using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class SystemScreen : Interface
    {
        public SystemScreen(InterfaceManager manager)
            : base(manager)
        {

        }

        public override void DrawStep()
        {
            //Draw the star
            int r = 15;
            for (int i = 0; i < r; i++)
            {
                GraphicConsole.SetColor(GameManager.CurrentSystem.StarColor, Color4.Black);
                GraphicConsole.Draw.Circle(0, GraphicConsole.BufferHeight / 2, i, '.');
            }
            GraphicConsole.Draw.Circle(0, GraphicConsole.BufferHeight / 2, r, '*');

            //Draw the planets
            for (int i = 0; i < GameManager.CurrentSystem.Planetoids.Count; i++)
            {
                int x = i % 4;
                int y = i / 4;

                Planetoid planet = GameManager.CurrentSystem.Planetoids[i];

                /*for (int j = 0; j < planet.R; j++)
                {
                    Color4 variatedColor = variateColor(planet.Color);
                    GraphicConsole.SetColor(variatedColor, Color4.Black);
                    GraphicConsole.Draw.Circle(x * 20 + 25, y * 15 + 10, j, '.');
                }*/

                int planetRadius = 5;

                GraphicConsole.SetColor(Color4.White, Color4.Black);
                GraphicConsole.Draw.Circle(x * 20 + 25, y * 15 + 10, planetRadius, '.');

                int moons = RNG.Next(0, 5);
                for (int j = 0; j < moons; j++)
                {
                    double angle = OpenTK.MathHelper.DegreesToRadians(j * 30) - OpenTK.MathHelper.DegreesToRadians(90.0);
                    int mX = (int)(Math.Cos(angle) * (planetRadius + 2.5)) + (x * 20 + 25);
                    int mY = (int)(Math.Sin(angle) * (planetRadius + 2.5)) + (y * 15 + 10);

                    GraphicConsole.SetColor(Color4.White, Color4.Black);

                    char token = (RNG.Next(0, 101) <= 50) ? '&' : '*';
                    GraphicConsole.Put(token, mX, mY);
                }
            }

            GraphicConsole.ClearColor();

            GraphicConsole.SetCursor(1, GraphicConsole.BufferHeight - 2);
            GraphicConsole.WriteLine("-Not to scale-");

            base.DrawStep();
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
    }
}
