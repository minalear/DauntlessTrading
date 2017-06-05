using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class SystemScreen : Interface
    {
        private int systemOffset = 0;

        public SystemScreen(InterfaceManager manager)
            : base(manager)
        {
            Slider slider = new Slider(null, 0, GraphicConsole.BufferHeight - 2, GraphicConsole.BufferWidth - 1);

            slider.ValueChanged += (sender, e) =>
            {
                systemOffset = -(int)(e * 100);
            };

            RegisterControl(slider);

            genPlanets();
        }

        public override void DrawStep()
        {
            for (int i = 0; i < 15; i++)
                GraphicConsole.Draw.Circle(systemOffset, GraphicConsole.BufferHeight / 2, i, '.');
            GraphicConsole.Draw.Circle(systemOffset, GraphicConsole.BufferHeight / 2, 15, '*');

            int r = 15;
            int offset = 0;
            for (int i = 0; i < planets.Count; i++)
            {
                offset += r + 6;
                r = planets[i].R;
                offset += r;

                GraphicConsole.SetColor(planets[i].Color, Color4.Black);
                GraphicConsole.Draw.Circle(systemOffset + offset, GraphicConsole.BufferHeight / 2, r, '·');
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
                planets.Add(new Planet() { R = RNG.Next(2, 10), Color = colors[RNG.Next(0, colors.Length)] });
            }
        }

        struct Planet
        {
            public int R;
            public Color4 Color;
        }
        private List<Planet> planets;
    }
}
