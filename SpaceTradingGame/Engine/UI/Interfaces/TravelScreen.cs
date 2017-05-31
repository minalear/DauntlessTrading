using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
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

            screenTitle = new Title(null, "Star Map", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);

            up = new Button(null, "▲", 87, 30);
            down = new Button(null, "▼", 87, 34);
            left = new Button(null, "◄", 84, 32);
            right = new Button(null, "►", 90, 32);

            up.Click += (sender, e) =>
            {
                mapOffset.Y += 100f;
                InterfaceManager.DrawStep();
            };
            down.Click += (sender, e) =>
            {
                mapOffset.Y -= 100f;
                InterfaceManager.DrawStep();
            };
            left.Click += (sender, e) =>
            {
                mapOffset.X += 100f;
                InterfaceManager.DrawStep();
            };
            right.Click += (sender, e) =>
            {
                mapOffset.X -= 100f;
                InterfaceManager.DrawStep();
            };

            RegisterControl(screenTitle);
            RegisterControl(up);
            RegisterControl(down);
            RegisterControl(left);
            RegisterControl(right);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetBounds(mapBounds);

            Vector2 origin = Vector2.Zero;
            origin.X += mapOffset.X / GraphicConsole.BufferWidth;
            origin.Y += mapOffset.Y / GraphicConsole.BufferWidth;

            int x = (int)(origin.X + mapBounds.X);
            int y = (int)(origin.Y + mapBounds.Y);

            GraphicConsole.SetColor(Color4.Gray, Color4.Black);
            GraphicConsole.Draw.Line(x, mapBounds.Top + 1, x, mapBounds.Bottom - 1, '.');
            GraphicConsole.Draw.Line(mapBounds.Left + 1, y, mapBounds.Right - 1, y, '.');
            GraphicConsole.SetCursor(x + 1, y - 1);
            GraphicConsole.ClearColor();
            
            drawSystems();

            GraphicConsole.ClearBounds();

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
        private void drawSystems()
        {
            for (int i = 0; i < GameManager.Systems.Count; i++)
            {
                System.Drawing.Point point = getPointFromCoord(GameManager.Systems[i].Coordinates);
                GraphicConsole.Put('*', point.X, point.Y);
            }
        }
        private System.Drawing.Point getPointFromCoord(Vector2 coord)
        {
            int x = (int)(coord.X / GraphicConsole.BufferWidth) + (int)(mapOffset.X / GraphicConsole.BufferWidth);
            int y = (int)(coord.Y / GraphicConsole.BufferWidth) + (int)(mapOffset.Y / GraphicConsole.BufferWidth);

            return new System.Drawing.Point(x + mapBounds.X, y + mapBounds.Y);
        }

        private Title screenTitle;
        private Button up, down, left, right;
        private System.Drawing.Rectangle mapBounds;

        private Vector2 mapOffset = new Vector2(3700, 1650);
    }
}
