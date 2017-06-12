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
            controlGroup = new Control();
            controlGroup.Position = System.Drawing.Point.Empty;
            controlGroup.Size = new System.Drawing.Point(GraphicConsole.BufferWidth, GraphicConsole.BufferHeight);

            backButton = new Button(null, "Back", 0, 0);
            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Travel");
            };

            marketButton = new Button(null, "Market", 0, GraphicConsole.BufferHeight - 3);
            marketButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Trading");
            };

            RegisterControl(controlGroup);
            RegisterControl(backButton);
            RegisterControl(marketButton);
        }

        public override void OnEnable()
        {
            controlGroup.Children.Clear();
            for (int i = 0; i < GameManager.CurrentSystem.Planetoids.Count; i++)
            {
                int x = i % 4;
                int y = i / 4;

                Planetoid planet = GameManager.CurrentSystem.Planetoids[i];

                Title title = new Title(controlGroup, planet, x * 20 + 25, y * 15 + 10, Title.TextAlignModes.Center);
                controlGroup.Children.Add(title);
            }

            base.OnEnable();
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

                int planetRadius = 5;

                GraphicConsole.SetColor(Color4.White, Color4.Black);
                GraphicConsole.Draw.Circle(x * 20 + 25, y * 15 + 10, planetRadius, '.');

                for (int moon = 0; moon < planet.Moons.Count; moon++)
                {
                    double angle = OpenTK.MathHelper.DegreesToRadians(moon * 30) - OpenTK.MathHelper.DegreesToRadians(90.0);
                    int mX = (int)(Math.Cos(angle) * (planetRadius + 3.1)) + (x * 20 + 25);
                    int mY = (int)(Math.Sin(angle) * (planetRadius + 1)) + (y * 15 + 10);

                    GraphicConsole.SetColor(Color4.White, Color4.Black);
                    GraphicConsole.Put('*', mX, mY);
                }
            }

            GraphicConsole.ClearColor();
            base.DrawStep();
        }

        //For registering and clearing controls between interface loads
        private Control controlGroup;
        private Button backButton, marketButton;
    }
}
