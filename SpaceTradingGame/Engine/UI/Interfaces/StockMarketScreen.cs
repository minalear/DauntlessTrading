using System.Text;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class StockMarketScreen : Interface
    {
        public StockMarketScreen(InterfaceManager manager)
            : base(manager)
        {
            //Titles
            Title screenTitle = new Title(null, "Galactic Stock Market", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            Title factionListTitle = new Title(null, "Factions", 10, 2, Title.TextAlignModes.Center);

            //Lists
            factionList = new ScrollingList(null, 1, 3, 21, 23);
            factionList.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            //Textboxes
            descriptionBox = new TextBox(null, 23, 3, 65, 23);
            descriptionBox.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            //Buttons
            Button backButton = new Button(null, "Back", 0, 0, 6, 1);
            Button buildMarketButton = new Button(null, "Build Market", 1, 27);
            Button buildStationButton = new Button(null, "Build Station", 1, 30);
            Button buildFactoryButton = new Button(null, "Build Factory", 1, 33);

            /* UI EVENTS */
            factionList.Selected += (sender, e) =>
            {
                descriptionBox.Text = getFactionDescription((Faction)factionList.GetSelection());
                InterfaceManager.DrawStep();
            };

            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("System");
            };
            buildMarketButton.Click += (sender, e) =>
            {

            };
            buildStationButton.Click += (sender, e) =>
            {

            };
            buildFactoryButton.Click += (sender, e) =>
            {

            };
            /* END UI EVENTS */

            //Control Register
            RegisterControl(screenTitle);
            RegisterControl(factionListTitle);
            RegisterControl(factionList);
            RegisterControl(descriptionBox);
            RegisterControl(backButton);
            RegisterControl(buildMarketButton);
            RegisterControl(buildStationButton);
            RegisterControl(buildFactoryButton);
        }

        public override void OnEnable()
        {
            factionList.SetList(GameManager.Factions);
            descriptionBox.Text = string.Empty;

            base.OnEnable();
        }

        private string getFactionDescription(Faction faction)
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine("Generated faction description goes here.");
            description.Append("\n");

            foreach (Market market in faction.OwnedMarkets)
            {
                description.AppendFormat("Has a market in the {0} system.\n", market.System.Name);
            }

            description.Append("\n");

            foreach (Ship ship in faction.OwnedShips)
            {
                description.AppendFormat("Owns a {0} ship named {1}.\n", ship.Model, ship.Name);
            }

            return description.ToString();
        }

        private ScrollingList factionList;
        private TextBox descriptionBox;
    }
}
