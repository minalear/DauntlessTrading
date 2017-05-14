using System;
using OpenTK.Input;
using SpaceTradingGame.Engine.Console;

namespace SpaceTradingGame.Engine.UI
{
    public class InterfaceManager
    {
        private TradingGame game;
        private GraphicConsole console;

        private Interface activeInterface;

        public GraphicConsole Console { get { return console; } }

        public InterfaceManager(TradingGame game)
        {
            this.game = game;
            this.console = new GraphicConsole(game, 100, 37);

            this.game.MouseDown += Game_MouseDown;
            this.game.MouseUp += Game_MouseUp;
            this.game.MouseEnter += Game_MouseEnter;
            this.game.MouseLeave += Game_MouseLeave;
            this.game.MouseMove += Game_MouseMove;
            this.game.MouseWheel += Game_MouseWheel;
        }

        public void DrawFrame(GameTime gameTime)
        {
            activeInterface.DrawFrame(gameTime);
        }
        public void UpdateFrame(GameTime gameTime)
        {
            activeInterface.UpdateFrame(gameTime);
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
            activeInterface.Game_MouseMove(sender, e);
        }
        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            activeInterface.Game_MouseWheel(sender, e);
        }
    }
}
