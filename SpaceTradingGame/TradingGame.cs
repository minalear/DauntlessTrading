using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine;
using SpaceTradingGame.Engine.UI;
using SpaceTradingGame.Engine.Console;
using OpenTK.Input;

namespace SpaceTradingGame
{
    public class TradingGame : GameWindow
    {
        private GameTime gameTime;

        private ContentManager content;
        private InterfaceManager interfaceManager;

        public TradingGame() : base(799, 443, GraphicsMode.Default, "Dauntless Trading Company")
        {
            gameTime = new GameTime();

            content = new ContentManager(this);
            interfaceManager = new InterfaceManager(this);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            interfaceManager.DrawFrame(gameTime);

            this.SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            gameTime.ElapsedTime = TimeSpan.FromSeconds(e.Time);
            gameTime.TotalTime.Add(gameTime.ElapsedTime);

            interfaceManager.UpdateFrame(gameTime);
        }

        public ContentManager Content { get { return content; } }
    }
}
