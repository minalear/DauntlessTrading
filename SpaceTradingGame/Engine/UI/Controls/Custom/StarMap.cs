using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using SpaceTradingGame.Game;
using OpenTK.Input;

namespace SpaceTradingGame.Engine.UI.Controls.Custom
{
    public class StarMap : Control
    {
        public StarMap()
        {
            systemList = new List<MapPoint>();
        }

        public override void DrawStep()
        {
            Rectangle mapBounds = new Rectangle(Position, new Size(Size));
            GraphicConsole.SetBounds(mapBounds);

            Vector2 origin = Vector2.Zero;
            origin.X += mapOffset.X / GraphicConsole.BufferWidth;
            origin.Y += mapOffset.Y / GraphicConsole.BufferWidth;

            int x = (int)(origin.X + mapBounds.X);
            int y = (int)(origin.Y + mapBounds.Y);

            GraphicConsole.SetColor(axisColor, Color4.Black);
            GraphicConsole.Draw.Line(x, mapBounds.Top + 1, x, mapBounds.Bottom - 1, '.');
            GraphicConsole.Draw.Line(mapBounds.Left + 1, y, mapBounds.Right - 1, y, '.');
            GraphicConsole.SetCursor(x + 1, y - 1);
            GraphicConsole.ClearColor();

            if (drawSelectedSystem)
                drawPaths();
            drawSystems();
            GraphicConsole.ClearBounds();
            drawBorder(mapBounds);

            base.DrawStep();
        }

        public void SetSystemList(List<StarSystem> list)
        {
            systemList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                systemList.Add(new MapPoint(list[i], GraphicConsole.BufferWidth));
            }
        }
        public void PanMap(float x, float y)
        {
            mapOffset.X += x;
            mapOffset.Y += y;
        }

        public override void MouseUp(MouseButtonEventArgs e)
        {
            Point mousePos = InterfaceManager.CurrentCursorPosition;
            mousePos.X -= Position.X;
            mousePos.Y -= Position.Y;

            Point selectedPoint = new Point(
                mousePos.X - (int)(mapOffset.X / GraphicConsole.BufferWidth),
                mousePos.Y - (int)(mapOffset.Y / GraphicConsole.BufferWidth));

            bool systemFound = false;
            for (int i = 0; i < systemList.Count; i++)
            {
                if (systemList[i].MapCoord.X == selectedPoint.X && 
                    systemList[i].MapCoord.Y == selectedPoint.Y)
                {
                    selectedSystem = systemList[i];
                    systemFound = true;

                    break;
                }
            }

            drawSelectedSystem = systemFound;

            InterfaceManager.DrawStep();
            base.MouseUp(e);
        }

        private void drawBorder(Rectangle mapBounds)
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
            foreach (MapPoint point in systemList)
            {
                Point relativePos = point.MapCoord;
                relativePos.X += (int)(mapOffset.X / GraphicConsole.BufferWidth);
                relativePos.Y += (int)(mapOffset.Y / GraphicConsole.BufferWidth);

                GraphicConsole.SetColor(point.System.StarColor, Color4.Black);
                GraphicConsole.Put('☼', relativePos.X + Position.X, relativePos.Y + Position.Y);
            }

            if (drawSelectedSystem)
            {
                Point relativePos = selectedSystem.MapCoord;
                relativePos.X += (int)(mapOffset.X / GraphicConsole.BufferWidth);
                relativePos.Y += (int)(mapOffset.Y / GraphicConsole.BufferWidth);

                GraphicConsole.SetColor(Color4.Black, selectedSystem.System.StarColor);
                GraphicConsole.Put('☼', relativePos.X + Position.X, relativePos.Y + Position.Y);
            }

            GraphicConsole.ClearColor();
        }
        private void drawPaths()
        {
            double travelRadius = 1250.0;
            int r = (int)(travelRadius / GraphicConsole.BufferWidth);

            Point center = getScreenPosFromCoord(selectedSystem.MapCoord);
            GraphicConsole.SetColor(Color4.Gray, Color4.Black);
            GraphicConsole.Draw.Circle(center.X , center.Y, r, '.');

            GraphicConsole.SetColor(Color4.Red, Color4.Black);
            for (int i = 0; i < systemList.Count; i++)
            {
                if (distance(selectedSystem.System.Coordinates, systemList[i].System.Coordinates) <= travelRadius)
                {
                    Point point = getScreenPosFromCoord(systemList[i].MapCoord);
                    GraphicConsole.Draw.Line(center.X, center.Y, point.X, point.Y, '.');
                }
            }
            GraphicConsole.ClearColor();
        }

        private Point getScreenPosFromCoord(Point coord)
        {
            coord.X += Position.X + (int)(mapOffset.X / GraphicConsole.BufferWidth);
            coord.Y += Position.Y + (int)(mapOffset.Y / GraphicConsole.BufferWidth);

            return coord;
        }
        private double distance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public class MapPoint
        {
            public StarSystem System;
            public Point MapCoord;

            public MapPoint(StarSystem system, int mapWidth)
            {
                this.System = system;

                int x = (int)(system.Coordinates.X / mapWidth);
                int y = (int)(system.Coordinates.Y / mapWidth);

                MapCoord = new Point(x, y);
            }
        }

        private List<MapPoint> systemList;
        private MapPoint selectedSystem;
        private bool drawSelectedSystem = false;
        private Vector2 mapOffset = new Vector2(3700, 1650);

        private Color4 axisColor = Color4.Gray;
    }
}
