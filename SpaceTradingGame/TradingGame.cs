using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpaceTradingGame.Game;

namespace SpaceTradingGame
{
    public class TradingGame : GameWindow
    {
        public TradingGame() : base(800, 450, GraphicsMode.Default, "Dauntless Trading Company")
        {
            Market market = new Game.Market();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            this.SwapBuffers();
        }
    }
}
