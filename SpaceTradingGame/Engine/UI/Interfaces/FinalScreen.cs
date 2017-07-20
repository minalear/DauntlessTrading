using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class FinalScreen : Interface
    {
        public FinalScreen(InterfaceManager manager)
            : base(manager)
        {
            generateRandomStars();

            Title message = new Title(null, "You were defeated!  Better luck next time.", 
                GraphicConsole.BufferWidth / 2, GraphicConsole.BufferHeight / 2, Title.TextAlignModes.Center);
            Button continueButton = new Button(null, "Continue", 
                GraphicConsole.BufferWidth / 2 - 4, GraphicConsole.BufferHeight / 2 + 2);
            Button exitButton = new Button(null, "Exit", 
                GraphicConsole.BufferWidth / 2 - 2, GraphicConsole.BufferHeight / 2 + 5);

            continueButton.Click += (sender, e) => InterfaceManager.ChangeInterface("Start");
            exitButton.Click += (sender, e) => GameManager.ExitGame();

            RegisterControl(message);
            RegisterControl(continueButton);
            RegisterControl(exitButton);
        }

        private void generateRandomStars()
        {
            stars = new List<StartScreen.StarPoint>();

            for (int i = 0; i < 80; i++)
            {
                stars.Add(new StartScreen.StarPoint(
                    RNG.Next(0, GraphicConsole.BufferWidth),
                    RNG.Next(0, GraphicConsole.BufferHeight)));
            }
            updateStars();
        }
        private void updateStars()
        {
            //For changing star colors
            for (int i = 0; i < stars.Count; i++)
            {
                //Color variance in the stars
                int rng = RNG.Next(0, 100);
                if (rng <= 5)
                    stars[i].Color = BLUE;
                else if (rng <= 15)
                    stars[i].Color = RED;
                else if (rng <= 30)
                    stars[i].Color = GRAY;
                else
                    stars[i].Color = DARK_GRAY;
            }
        }
        public override void DrawStep()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                GraphicConsole.SetColor(stars[i].Color, Color4.Black);
                GraphicConsole.Put('.', stars[i].X, stars[i].Y);
            }
            GraphicConsole.ClearColor();

            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {
            //Make the stars twinkle by forcing the screen to draw
            timer += gameTime.ElapsedTime.TotalSeconds;
            if (timer >= 0.85)
            {
                timer = 0.0;
                updateStars();
                InterfaceManager.DrawStep();
            }

            base.UpdateFrame(gameTime);
        }

        private double timer = 0.0;
        private List<StartScreen.StarPoint> stars;

        private Color4 DARK_GRAY = new Color4(25, 25, 25, 255);
        private Color4 RED = new Color4(75, 50, 50, 255);
        private Color4 BLUE = new Color4(75, 95, 95, 255);
        private Color4 GRAY = new Color4(50, 50, 50, 255);
    }
}
