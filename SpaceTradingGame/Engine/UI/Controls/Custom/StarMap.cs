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
            systemList = new List<StarSystem>();
            path = new List<StarSystem>();

            HasSystemSelected = false;
            DrawPlayerPosition = false;
            PlayerPosition = Point.Empty;
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
            GraphicConsole.Draw.Line(x, mapBounds.Top + 1, x, mapBounds.Bottom - 1, '·');
            GraphicConsole.Draw.Line(mapBounds.Left + 1, y, mapBounds.Right - 1, y, '·');
            GraphicConsole.SetCursor(x + 1, y - 1);
            GraphicConsole.ClearColor();

            if (drawSelectedSystem)
                drawPaths();
            drawSystems();
            GraphicConsole.ClearBounds();
            drawBorder(mapBounds);

            if (DrawPlayerPosition)
            {
                GraphicConsole.SetColor(Color4.Red, Color4.Black);
                GraphicConsole.Put('@', getScreenPosFromCoord(PlayerPosition));
            }

            GraphicConsole.ClearColor();
            base.DrawStep();
        }

        public void SetCurrentSystem(StarSystem system)
        {
            Interface.GameManager.CurrentSystem = system;
            currentSystem = system;
            drawSelectedSystem = false;
            HasSystemSelected = false;

            InterfaceManager.DrawStep();
        }
        public void SetSystemList(List<StarSystem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].MapCoord = getCoordFromWorldPos(list[i].Coordinates);
            }
            
            systemList = list;
        }
        public void PanMap(float x, float y)
        {
            mapOffset.X += x;
            mapOffset.Y += y;
        }
        public StarSystem[] GetTravelPath()
        {
            return path.ToArray();
        }
        public void SetPlayerPosition(Vector2 worldPosition)
        {
            PlayerPosition = getCoordFromWorldPos(worldPosition);
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
            HasSystemSelected = systemFound;
            if (drawSelectedSystem)
            {
                plotPath();
                this.Selected?.Invoke(this, SelectedSystem);
            }

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
            //Draw every system
            foreach (StarSystem point in systemList)
            {
                Point relativePos = point.MapCoord;
                relativePos.X += (int)(mapOffset.X / GraphicConsole.BufferWidth);
                relativePos.Y += (int)(mapOffset.Y / GraphicConsole.BufferWidth);

                GraphicConsole.SetColor(point.StarColor, Color4.Black);
                GraphicConsole.Put('☼', relativePos.X + Position.X, relativePos.Y + Position.Y);
            }

            //Draw current system
            Point systemPos = currentSystem.MapCoord;
            systemPos.X += (int)(mapOffset.X / GraphicConsole.BufferWidth);
            systemPos.Y += (int)(mapOffset.Y / GraphicConsole.BufferWidth);

            GraphicConsole.SetColor(Color4.Black, currentSystem.StarColor);
            GraphicConsole.Put('☼', systemPos.X + Position.X, systemPos.Y + Position.Y);

            if (drawSelectedSystem)
            {
                Point relativePos = selectedSystem.MapCoord;
                relativePos.X += (int)(mapOffset.X / GraphicConsole.BufferWidth);
                relativePos.Y += (int)(mapOffset.Y / GraphicConsole.BufferWidth);

                GraphicConsole.SetColor(Color4.Black, selectedSystem.StarColor);
                GraphicConsole.Put('☼', relativePos.X + Position.X, relativePos.Y + Position.Y);
            }

            GraphicConsole.ClearColor();
        }
        private void drawPaths()
        {
            /*double travelRadius = 1250.0;
            int r = (int)(travelRadius / GraphicConsole.BufferWidth);

            Point center = getScreenPosFromCoord(currentSystem.MapCoord);
            GraphicConsole.SetColor(Color4.Gray, Color4.Black);
            GraphicConsole.Draw.Circle(center.X , center.Y, r, '·');

            GraphicConsole.SetColor(Color4.Red, Color4.Black);
            /*for (int i = 0; i < systemList.Count; i++)
            {
                if (distance(selectedSystem.System.Coordinates, systemList[i].System.Coordinates) <= travelRadius)
                {
                    Point point = getScreenPosFromCoord(systemList[i].MapCoord);
                    GraphicConsole.Draw.Line(center.X, center.Y, point.X, point.Y, '.');
                }
            }*/
            /*Point target = getScreenPosFromCoord(selectedSystem.MapCoord);
            GraphicConsole.Draw.Line(center.X, center.Y, target.X, target.Y, '.');
            GraphicConsole.ClearColor();*/

            GraphicConsole.SetColor(Color4.Red, Color4.Black);
            for (int i = 0; i < path.Count - 1; i++)
            {
                Point a = getScreenPosFromCoord(path[i].MapCoord);
                Point b = getScreenPosFromCoord(path[i + 1].MapCoord);

                GraphicConsole.Draw.Line(a.X, a.Y, b.X, b.Y, '.');
            }
            GraphicConsole.ClearColor();
        }
        private void plotPath()
        {
            if (!drawSelectedSystem)
            {
                path.Clear();
            }
            else
            {
                path.Clear();
                path.Add(currentSystem);

                StarSystem currentNode = currentSystem;
                StarSystem testNode = selectedSystem;
                StarSystem target = selectedSystem;

                List<StarSystem> ignoredSystems = new List<StarSystem>();

                bool final = false;
                while (!final)
                {
                    //Find closest system
                    for (int i = 0; i < systemList.Count; i++)
                    {
                        if (systemList[i].Equals(currentNode)) continue;
                        if (ignoredSystems.Contains(systemList[i])) continue;

                        if (currentNode.Coordinates.Distance(systemList[i].Coordinates) <
                            currentNode.Coordinates.Distance(testNode.Coordinates))
                        {
                            testNode = systemList[i];
                        }
                    }

                    //Check if closest node is target
                    if (testNode.Equals(target))
                        break;

                    //Check if node gets closer to target
                    if (testNode.Coordinates.Distance(target.Coordinates) <
                        currentNode.Coordinates.Distance(target.Coordinates))
                    {
                        path.Add(testNode);
                        currentNode = testNode;

                        ignoredSystems.Clear();
                    }
                    else
                    {
                        ignoredSystems.Add(testNode);
                        testNode = target;
                    }
                }

                path.Add(target);
            }

            //Optimize Path
            List<StarSystem> optimalPath = new List<StarSystem>();

            double jumpRange = 400.0;
            optimalPath.Add(path[0]);

            for (int i = 0; i < path.Count; i++)
            {
                for (int j = path.Count - 1; j >= 0; j--)
                {
                    double dist = path[i].Coordinates.Distance(path[j].Coordinates);
                    if (path[i].Coordinates.Distance(path[j].Coordinates) <= jumpRange)
                    {
                        i = j;
                        optimalPath.Add(path[i]);

                        break;
                    }
                }
            }

            path.Clear();
            path = optimalPath;
        }

        private Point getScreenPosFromCoord(Point coord)
        {
            coord.X += Position.X + (int)(mapOffset.X / GraphicConsole.BufferWidth);
            coord.Y += Position.Y + (int)(mapOffset.Y / GraphicConsole.BufferWidth);

            return coord;
        }
        private Point getCoordFromWorldPos(Vector2 pos)
        {
            return new Point(
                (int)(pos.X / this.Size.X),
                (int)(pos.Y / this.size.X));
        }

        private List<StarSystem> systemList;
        private List<StarSystem> path;
        private StarSystem selectedSystem, currentSystem;
        private bool drawSelectedSystem = false;
        private Vector2 mapOffset = new Vector2(3700, 1650);

        private Color4 axisColor = Color4.Gray;

        public StarSystem CurrentSystem { get { return Interface.GameManager.CurrentSystem; } }
        public StarSystem SelectedSystem { get { return selectedSystem; } }

        public bool HasSystemSelected { get; set; }
        public bool DrawPlayerPosition { get; set; }
        public Point PlayerPosition { get; set; }

        public delegate void SystemSelected(object sender, StarSystem system);
        public SystemSelected Selected;
    }
}
