using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Game;
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
            Title companyName = new Title(null, "Comp Name: ", 1, 6, Title.TextAlignModes.Left);
            Title shipType = new Title(null, "Ship Type: ", 1, 8, Title.TextAlignModes.Left);
            Title shipDescription = new Title(null, "Description: ", 24, 8, Title.TextAlignModes.Left);

            playerNameInput = new InputBox(null, 12, 2, 20);
            playerNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            shipNameInput = new InputBox(null, 12, 4, 20);
            shipNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            companyNameInput = new InputBox(null, 12, 6, 20);
            companyNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            shipSelectionList = new ScrollingList(null, 1, 9, 22, 19);
            shipSelectionList.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            shipDescriptionBox = new TextBox(null, 24, 9, 35, 19);
            shipDescriptionBox.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            startGameButton = new Button(null, "Start", GraphicConsole.BufferWidth - 8, GraphicConsole.BufferHeight - 4);
            startGameButton.Click += (sender, e) =>
            {
                //if (!isValidInputs()) return;

                Ship ship = new Ship(shipNameInput.Text.Trim(), /*shipSelectionList.GetSelection()*/ "Hello", 500, 2000);
                GameManager.SetupGame(playerNameInput.Text.Trim(), companyNameInput.Text.Trim(), ship);

                InterfaceManager.ChangeInterface("Travel");
            };

            backButton = new Button(null, "Back", 1, GraphicConsole.BufferHeight - 4);
            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Start");
            };

            RegisterControl(characterCreation);
            RegisterControl(playerName);
            RegisterControl(shipName);
            RegisterControl(companyName);
            RegisterControl(shipType);
            RegisterControl(shipDescriptionBox);
            RegisterControl(playerNameInput);
            RegisterControl(shipNameInput);
            RegisterControl(companyNameInput);
            RegisterControl(shipSelectionList);
            RegisterControl(shipDescription);
            RegisterControl(startGameButton);
            RegisterControl(backButton);
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
            GraphicConsole.Draw.Line(1, 7, GraphicConsole.BufferWidth - 2, 7, '-');
            GraphicConsole.Draw.Line(1, 28, GraphicConsole.BufferWidth - 2, 28, '-');
        }
        private bool isValidInputs()
        {
            if (playerNameInput.Text.Trim().Length <= 0) return false;
            if (companyNameInput.Text.Trim().Length <= 0) return false;
            if (shipNameInput.Text.Trim().Length <= 0) return false;
            if (!shipSelectionList.HasSelection) return false;

            return true;
        }
        
        private InputBox playerNameInput, companyNameInput, shipNameInput;
        private ScrollingList shipSelectionList;
        private TextBox shipDescriptionBox;
        private Button startGameButton, backButton;
    }
}
