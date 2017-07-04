using System.Text;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class BuildScreen : Interface
    {
        public BuildScreen(InterfaceManager manager)
            : base(manager)
        {
            //Titles
            Title screenTitle = new Title(null, "Building Interface", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            Title planetListTitle = new Title(null, "Planets", 10, 2, Title.TextAlignModes.Center);

            //Lists
            planetList = new ScrollingList(null, 1, 3, 21, 23);
            planetList.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            //Textboxes
            descriptionBox = new TextBox(null, 23, 3, 35, 23);
            descriptionBox.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            //Buttons
            Button backButton = new Button(null, "Back", 0, 0, 6, 1);
            Button buildMarketButton = new Button(null, "Build Market", 1, 27);
            Button buildStationButton = new Button(null, "Build Station", 1, 30);
            Button buildFactoryButton = new Button(null, "Build Factory", 1, 33);

            /* UI EVENTS */
            planetList.Selected += (sender, e) =>
            {
                descriptionBox.Text = getPlanetDescription((Planetoid)planetList.GetSelection());
                InterfaceManager.DrawStep();
            };

            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("System");
            };
            buildMarketButton.Click += (sender, e) =>
            {
                GameManager.CurrentSystem.BuildMarket(GameManager.PlayerFaction);
            };
            buildStationButton.Click += (sender, e) =>
            {
                foreach (Planetoid planet in GameManager.CurrentSystem.Planetoids)
                {
                    planet.BuildStation(GameManager.PlayerFaction);
                }
            };
            buildFactoryButton.Click += (sender, e) =>
            {
                foreach (Planetoid planet in GameManager.CurrentSystem.Planetoids)
                {
                    planet.BuildFactory(GameManager.PlayerFaction, Game.Factories.ProductFactory.CarbonDioxide);
                }
            };
            /* END UI EVENTS */

            //Control Register
            RegisterControl(screenTitle);
            RegisterControl(planetListTitle);
            RegisterControl(planetList);
            RegisterControl(descriptionBox);
            RegisterControl(backButton);
            RegisterControl(buildMarketButton);
            RegisterControl(buildStationButton);
            RegisterControl(buildFactoryButton);
        }

        public override void OnEnable()
        {
            planetList.SetList(GameManager.CurrentSystem.Planetoids);
            descriptionBox.Text = string.Empty;

            base.OnEnable();
        }

        private string getPlanetDescription(Planetoid planet)
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine("Generated planet description goes here.");
            description.Append("\n");

            foreach (MaterialDeposit deposit in planet.MaterialDeposits)
            {
                description.AppendFormat("{0} - {1}\n", deposit.Material.Name, deposit.Density.Truncate(4));
            }

            description.Append("\n");

            foreach (Station station in planet.Stations)
            {
                description.AppendFormat("Has a space station owned by {0}.\n", station.Owner.Name);
            }

            description.Append("\n");

            foreach (Factory factory in planet.Factories)
            {
                description.AppendFormat("Has a factory owned by {0}.  It produces {1}.", 
                    factory.Owner.Name, factory.MainProduction.Name);
            }

            return description.ToString();
        }

        private ScrollingList planetList;
        private TextBox descriptionBox;
    }
}
