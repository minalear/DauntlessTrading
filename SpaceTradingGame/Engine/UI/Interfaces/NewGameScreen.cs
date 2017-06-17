using System;
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
            Title characterCreation = new Title(null, "Ship Registration", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            Title playerName = new Title(null, "Name: ", 6, 2, Title.TextAlignModes.Left);
            Title shipName = new Title(null, "Ship Name: ", 1, 4, Title.TextAlignModes.Left);
            Title shipType = new Title(null, "Ship Type: ", 1, 6, Title.TextAlignModes.Left);

            playerNameInput = new InputBox(null, 12, 2, 20);
            playerNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            shipNameInput = new InputBox(null, 12, 4, 20);
            shipNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            RegisterControl(characterCreation);
            RegisterControl(playerName);
            RegisterControl(shipName);
            RegisterControl(shipType);
            RegisterControl(playerNameInput);
            RegisterControl(shipNameInput);
        }

        public override void DrawStep()
        {
            this.drawBorders();

            base.DrawStep();
        }

        private void drawBorders()
        {
            GraphicConsole.Draw.Line(0, 0, 0, GraphicConsole.BufferHeight - 1, '║');
            GraphicConsole.Draw.Line(GraphicConsole.BufferWidth - 1, 0, GraphicConsole.BufferWidth - 1, GraphicConsole.BufferHeight - 1, '║');
            GraphicConsole.Draw.Line(0, 0, GraphicConsole.BufferWidth - 1, 0, '═');
            GraphicConsole.Draw.Line(0, GraphicConsole.BufferHeight - 1, GraphicConsole.BufferWidth - 1, GraphicConsole.BufferHeight - 1, '═');

            GraphicConsole.Put('╔', 0, 0);
            GraphicConsole.Put('╗', GraphicConsole.BufferWidth - 1, 0);
            GraphicConsole.Put('╚', 0, GraphicConsole.BufferHeight - 1);
            GraphicConsole.Put('╝', GraphicConsole.BufferWidth - 1, GraphicConsole.BufferHeight - 1);

            GraphicConsole.Draw.Line(1, 3, GraphicConsole.BufferWidth - 2, 3, '-');
            GraphicConsole.Draw.Line(1, 5, GraphicConsole.BufferWidth - 2, 5, '-');
            GraphicConsole.Draw.Line(1, 25, GraphicConsole.BufferWidth - 2, 25, '-');
        }
        
        private InputBox playerNameInput, companyNameInput, shipNameInput;
        private ScrollingList shipSelectionList;
        private TextBox shipDescriptionBox;
    }
}
