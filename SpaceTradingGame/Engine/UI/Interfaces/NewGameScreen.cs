using OpenTK.Graphics;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class NewGameScreen : Interface
    {
        public NewGameScreen(InterfaceManager manager)
            : base(manager)
        {
            Title characterCreation = new Title(null, "Ship Registration", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            Title playerName = new Title(null, "Name: ", 7, 2, Title.TextAlignModes.Left);
            Title shipName = new Title(null, "Ship Name: ", 2, 4, Title.TextAlignModes.Left);
            Title companyName = new Title(null, "Comp Name: ", 2, 6, Title.TextAlignModes.Left);
            Title shipType = new Title(null, "Ship Type: ", 2, 8, Title.TextAlignModes.Left);
            Title shipDescription = new Title(null, "Description: ", 25, 8, Title.TextAlignModes.Left);
            Title shipLayoutTitle = new Title(null, "Layout: ", 64, 8, Title.TextAlignModes.Left);

            playerNameInput = new InputBox(null, 13, 2, 20);
            playerNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);
            playerNameInput.Text = "James Comey";

            shipNameInput = new InputBox(null, 13, 4, 20);
            shipNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);
            shipNameInput.Text = "Heart of the Horizon";

            companyNameInput = new InputBox(null, 13, 6, 20);
            companyNameInput.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);
            companyNameInput.Text = "Comey Shipping Inc";

            shipSelectionList = new ScrollingList(null, 2, 9, 22, 19);
            shipSelectionList.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);
            shipSelectionList.SetList(Game.Factories.ShipFactory.BasicShips);
            shipSelectionList.Selected += (sender, e) =>
            {
                Ship selectedShip = (Ship)shipSelectionList.GetSelection();
                
                shipDescriptionBox.Text = string.Format("== {0} ==\n{1}\n-\nFire: {2}\nDfns: {3}\nCargo: {4}\nJump: {5}",
                    selectedShip.Model, selectedShip.Description, selectedShip.FirePower, selectedShip.DefenseRating,
                    selectedShip.CargoCapacity, selectedShip.JumpRadius);
                shipLayout.SetShip((Ship)shipSelectionList.GetSelection());

                InterfaceManager.DrawStep();
            };

            shipDescriptionBox = new TextBox(null, 25, 9, 38, 19);
            shipDescriptionBox.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            startGameButton = new Button(null, "Start", GraphicConsole.BufferWidth - 8, GraphicConsole.BufferHeight - 4);
            startGameButton.Click += (sender, e) =>
            {
                if (!isValidInputs()) return;
                GameManager.SetupGame(playerNameInput.Text.Trim(), companyNameInput.Text.Trim(), shipNameInput.Text.Trim(), (Ship)shipSelectionList.GetSelection());

                InterfaceManager.ChangeInterface("Travel");
            };

            backButton = new Button(null, "Back", 1, GraphicConsole.BufferHeight - 4);
            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Start");
            };

            shipLayout = new ShipLayout(null, 34, 19);
            shipLayout.Position = new System.Drawing.Point(64, 9);

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
            RegisterControl(shipLayoutTitle);
            RegisterControl(startGameButton);
            RegisterControl(backButton);
            RegisterControl(shipLayout);
        }

        public override void OnEnable()
        {
            shipSelectionList.SetSelection(0);

            base.OnEnable();
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
        private ShipLayout shipLayout;
    }
}
