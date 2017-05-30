using System;
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

            RegisterControl(mainTitle);
            RegisterControl(subTitle);
            RegisterControl(newGameButton);
            RegisterControl(loadGameButton);
            RegisterControl(optionsButton);
            RegisterControl(exitButton);
            RegisterControl(infoTitle);
        }

        private Title mainTitle, subTitle, infoTitle;
        private Button newGameButton, loadGameButton, optionsButton, exitButton;
    }
}
