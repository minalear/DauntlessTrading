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

        }

        public override void DrawStep()
        {
            GraphicConsole.Draw.Circle(GraphicConsole.BufferWidth / 2, GraphicConsole.BufferHeight / 2, 10, '*');

            base.DrawStep();
        }
    }
}
