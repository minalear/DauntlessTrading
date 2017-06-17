using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class NewGameScreen : Interface
    {
        public NewGameScreen(InterfaceManager manager)
            : base(manager)
        {
            inputBox = new InputBox(null, 0, 0, 12);

            RegisterControl(inputBox);
        }

        private InputBox inputBox;
    }
}
