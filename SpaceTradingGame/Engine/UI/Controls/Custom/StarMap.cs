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
            path = new List<StarSystem>();

            HasSystemSelected = false;
            DrawPlayerPosition = false;
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

            double travelRadius = Interface.GameManager.PlayerShip.JumpRadius;
            int r = (int)(travelRadius / GraphicConsole.BufferWidth);

            Point playerPos = getScreenPosFromCoord(getCoordFromWorldPos(Interface.GameManager.PlayerShip.WorldPosition));
            GraphicConsole.SetColor(Color4.Gray, Color4.Black);
            GraphicConsole.Draw.Circle(playerPos.X, playerPos.Y, r, '·');

            if (drawSelectedSystem)
                drawPaths();
            drawShips();
            drawSystems();
            drawFactions();

            GraphicConsole.ClearBounds();
            GraphicConsole.ClearColor();

            drawBorder(mapBounds);

            if (DrawPlayerPosition)
            {
                GraphicConsole.SetColor(Color4.Red, Color4.Black);
                GraphicConsole.Put('@', playerPos);
            }

            GraphicConsole.ClearColor();
            base.DrawStep();
        }

        public void SetCurrentSystem(StarSystem system)
        {
            currentSystem = system;
            drawSelectedSystem = false;
            HasSystemSelected = false;

            InterfaceManager.DrawStep();
        }
        public void UpdateSystemList(List<StarSystem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].MapCoord = getCoordFromWorldPos(list[i].Coordinates);
            }
        }
        public void PanMap(float x, float y)
        {
            mapOffset.X += x;
            mapOffset.Y += y;
        }
        public void SetPath(List<StarSystem> path)
        {
            this.path = path;
        }
        public void ClearPath()
        {
            path.Clear();
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
            foreach (StarSystem system in Interface.GameManager.Systems)
            {
                if (system.MapCoord.X == selectedPoint.X &&
                    system.MapCoord.Y == selectedPoint.Y)
                {
                    selectedSystem = system;
                    systemFound = true;

                    break;
                }
            }

            drawSelectedSystem = systemFound;
            HasSystemSelected = systemFound;
            if (drawSelectedSystem)
            {
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
        private void drawFactions()
        {
            GraphicConsole.Draw.PaintMode = Console.PaintModes.Add;
            foreach (Faction faction in Interface.GameManager.Factions)
            {
                foreach (Market market in faction.OwnedMarkets)
                {
                    Point relativePos = getScreenPosFromCoord(market.System.MapCoord);
                    GraphicConsole.SetColor(Color4.Transparent, faction.RegionColor);
                    GraphicConsole.Draw.FillCircle(relativePos.X, relativePos.Y, 8, ' ');
                }
            }
            GraphicConsole.Draw.PaintMode = Console.PaintModes.Default;
        }
        private void drawSystems()
        {
            //Draw every system
            foreach (StarSystem point in Interface.GameManager.Systems)
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
            GraphicConsole.SetColor(Color4.Red, Color4.Black);
            for (int i = 0; i < path.Count - 1; i++)
            {
                Point a = getScreenPosFromCoord(path[i].MapCoord);
                Point b = getScreenPosFromCoord(path[i + 1].MapCoord);

                GraphicConsole.Draw.Line(a.X, a.Y, b.X, b.Y, '∙');
            }
            GraphicConsole.ClearColor();
        }
        private void drawShips()
        {
            List<Ship> shipsInRange = Interface.GameManager.GetShipsInJumpRadius(Interface.GameManager.PlayerShip);
            foreach (Ship ship in shipsInRange)
            {
                Point point = getScreenPosFromCoord(getCoordFromWorldPos(ship.WorldPosition));

                GraphicConsole.SetColor(ship.Faction.RegionColor, Color4.Black);
                GraphicConsole.Put('.', point);
            }
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
        
        private List<StarSystem> path;
        private StarSystem selectedSystem, currentSystem;
        private bool drawSelectedSystem = false;
        private Vector2 mapOffset = new Vector2(3700, 1650);

        private Color4 axisColor = Color4.Gray;

        public StarSystem CurrentSystem { get { return Interface.GameManager.CurrentSystem; } }
        public StarSystem SelectedSystem { get { return selectedSystem; } }

        public bool HasSystemSelected { get; set; }
        public bool DrawPlayerPosition { get; set; }

        public delegate void SystemSelected(object sender, StarSystem system);
        public SystemSelected Selected;
    }
}
