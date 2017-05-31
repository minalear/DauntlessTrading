using System;
using System.Collections.Generic;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TravelScreen : Interface
    {
        public TravelScreen(InterfaceManager manager)
            : base(manager)
        {
            mapBounds = new System.Drawing.Rectangle(1, 2, 74, 33);

            screenTitle = new Title(null, "Travel", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            RegisterControl(screenTitle);
        }

        public override void DrawStep()
        {
            drawBorder();

            base.DrawStep();
        }
        private void drawBorder()
        {
            GraphicConsole.Draw.Line(mapBounds.Left, mapBounds.Top, mapBounds.Left, mapBounds.Bottom, '│');
            GraphicConsole.Draw.Line(mapBounds.Right, mapBounds.Top, mapBounds.Right, mapBounds.Bottom, '│');
            GraphicConsole.Draw.Line(mapBounds.Left, mapBounds.Top, mapBounds.Right, mapBounds.Top, '─');
            GraphicConsole.Draw.Line(mapBounds.Left, mapBounds.Bottom, mapBounds.Right, mapBounds.Bottom, '─');

            GraphicConsole.Put('┌', mapBounds.Left, mapBounds.Top);
            GraphicConsole.Put('┐', mapBounds.Right, mapBounds.Top);
            GraphicConsole.Put('└', mapBounds.Left, mapBounds.Bottom);
            GraphicConsole.Put('┘', mapBounds.Right, mapBounds.Bottom);
        }

        private Title screenTitle;
        private System.Drawing.Rectangle mapBounds;
    }
}
