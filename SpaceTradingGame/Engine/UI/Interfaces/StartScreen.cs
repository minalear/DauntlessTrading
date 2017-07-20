using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class StartScreen : Interface
    {
        public StartScreen(InterfaceManager manager)
            : base(manager)
        {
            mainTitle = new Title(null, "Dauntless Trading Company", GraphicConsole.BufferWidth / 2, 3, Title.TextAlignModes.Center);
            mainTitle.TextColor = Color4.White;
            subTitle = new Title(null, "Chapter 1", GraphicConsole.BufferWidth / 2, 4, Title.TextAlignModes.Center);
            subTitle.TextColor = Color4.Gray;

            newGameButton = new Button(null, "New Game", GraphicConsole.BufferWidth / 2 - 10, 10, 20, 3);
            loadGameButton = new Button(null, "Load Game", GraphicConsole.BufferWidth / 2 - 10, 14, 20, 3);
            optionsButton = new Button(null, "Options", GraphicConsole.BufferWidth / 2 - 10, 18, 20, 3);
            exitButton = new Button(null, "Exit", GraphicConsole.BufferWidth / 2 - 10, 22, 20, 3);

            infoTitle = new Title(null, "Game by Trevor Fisher - minalear.com", GraphicConsole.BufferWidth / 2, 
                GraphicConsole.BufferHeight - 3, Title.TextAlignModes.Center);
            infoTitle.TextColor = Color4.Gray;

            newGameButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("NewGame");
            };
            exitButton.Click += (sender, e) => 
            {
                InterfaceManager.Game.Exit();
            };

            generateRandomStars();

            RegisterControl(mainTitle);
            RegisterControl(subTitle);
            RegisterControl(newGameButton);
            RegisterControl(loadGameButton);
            RegisterControl(optionsButton);
            RegisterControl(exitButton);
            RegisterControl(infoTitle);
        }

        private void generateRandomStars()
        {
            stars = new List<StarPoint>();
            
            for (int i = 0; i < 80; i++)
            {
                stars.Add(new StarPoint(
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

        private Title mainTitle, subTitle, infoTitle;
        private Button newGameButton, loadGameButton, optionsButton, exitButton;
        private List<StarPoint> stars;

        private Color4 DARK_GRAY = new Color4(25, 25, 25, 255);
        private Color4 RED = new Color4(75, 50, 50, 255);
        private Color4 BLUE = new Color4(75, 95, 95, 255);
        private Color4 GRAY = new Color4(50, 50, 50, 255);

        public class StarPoint
        {
            public int X;
            public int Y;
            public Color4 Color;

            public StarPoint(int x, int y)
            {
                X = x;
                Y = y;
                Color = Color4.White;
            }
        }
    }
}
