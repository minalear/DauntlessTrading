using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using SpaceTradingGame.Engine.Console;
using SpaceTradingGame.Engine.UI.Interfaces;

namespace SpaceTradingGame.Engine.UI
{
    public class InterfaceManager
    {
        private TradingGame game;
        private GraphicConsole console;

        private Interface activeInterface;
        private Dictionary<string, Interface> interfaces;

        public TradingGame Game { get { return game; } }
        public GraphicConsole Console { get { return console; } }

        public Point CurrentCursorPosition { get; set; }
        public Point PreviousCursorPosition { get; set; }

        public InterfaceManager(TradingGame game)
        {
            this.game = game;
            this.console = new GraphicConsole(game, 100, 37);

            this.interfaces = new Dictionary<string, Interface>();

            CurrentCursorPosition = Point.Empty;
            PreviousCursorPosition = Point.Empty;

            this.game.MouseDown += Game_MouseDown;
            this.game.MouseUp += Game_MouseUp;
            this.game.MouseEnter += Game_MouseEnter;
            this.game.MouseLeave += Game_MouseLeave;
            this.game.MouseMove += Game_MouseMove;
            this.game.MouseWheel += Game_MouseWheel;
            this.game.KeyPress += Game_KeyPress;
            this.game.KeyUp += Game_KeyUp;
            this.game.KeyDown += Game_KeyDown;

            this.interfaces.Add("Start", new StartScreen(this));
            this.interfaces.Add("NewGame", new NewGameScreen(this));
            this.interfaces.Add("Ship", new ShipScreen(this));
            this.interfaces.Add("Travel", new TravelScreen(this));
            this.interfaces.Add("System", new SystemScreen(this));
            this.interfaces.Add("Trading", new TradingScreen(this));
            this.interfaces.Add("Build", new BuildScreen(this));
            this.interfaces.Add("Stock", new StockMarketScreen(this));
            
            ChangeInterface("NewGame");
        }

        public void ChangeInterface(string name)
        {
            if (activeInterface != null)
                activeInterface.OnDisable();

            activeInterface = this.interfaces[name];
            activeInterface.OnEnable();
        }

        public void DrawFrame(GameTime gameTime)
        {
            activeInterface.DrawFrame(gameTime);
            console.RenderFrame();
        }
        public void UpdateFrame(GameTime gameTime)
        {
            activeInterface.UpdateFrame(gameTime);
            console.UpdateFrame(gameTime);
        }
        public void DrawStep()
        {
            Console.Clear();
            activeInterface.DrawStep();
        }
        public void UpdateStep()
        {
            activeInterface.UpdateStep();
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            activeInterface.Game_MouseDown(sender, e);
        }
        private void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {
            activeInterface.Game_MouseUp(sender, e);
        }
        private void Game_MouseEnter(object sender, EventArgs e)
        {
            activeInterface.Game_MouseEnter(sender, e);
        }
        private void Game_MouseLeave(object sender, EventArgs e)
        {
            activeInterface.Game_MouseLeave(sender, e);
        }
        private void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            PreviousCursorPosition = CurrentCursorPosition;
            CurrentCursorPosition = console.GetTilePosition(e.Position);
            activeInterface.Game_MouseMove(sender, e);
        }
        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            activeInterface.Game_MouseWheel(sender, e);
        }
        private void Game_KeyPress(object sender, OpenTK.KeyPressEventArgs e)
        {
            activeInterface.Game_KeyPress(sender, e);
        }
        private void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            activeInterface.Game_KeyUp(sender, e);
        }
        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            activeInterface.Game_KeyDown(sender, e);
        }
    }
}
