using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine;
using SpaceTradingGame.Engine.Console;

namespace SpaceTradingGame
{
    public class TradingGame : GameWindow
    {
        private ContentManager content;
        private GraphicConsole console;

        public TradingGame() : base(800, 450, GraphicsMode.Default, "Dauntless Trading Company")
        {
            content = new ContentManager(this);
            console = new GraphicConsole(this);

            console.WriteLine("Hello, world!");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            console.RenderFrame();
            this.SwapBuffers();
        }

        public ContentManager Content { get { return content; } }
        public GraphicConsole Console { get { return console; } }
    }
}
