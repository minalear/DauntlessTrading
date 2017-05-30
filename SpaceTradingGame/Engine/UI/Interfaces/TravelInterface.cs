using System;
using System.Collections.Generic;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TravelInterface : Interface
    {
        public TravelInterface(InterfaceManager manager)
            : base(manager)
        {
            //Interface Initialization
            Title title = new Title(null, string.Format("-= {0} System =-", GameManager.Systems[0].Name), 50, 0, Title.TextAlignModes.Center);
            ScrollingList destinationList = new ScrollingList(null, 1, 2, 39, 23);
            destinationList.SetList(GameManager.Systems);

            Button travelButton = new Button(null, "Travel", 1, 26, 39, 3);
            travelButton.Click += (sender, e) =>
            {
                ListItem selection = destinationList.GetSelection();

                if (selection.GetType() == typeof(Game.System))
                {
                    destinationList.SetList(GameManager.Systems[destinationList.SelectedIndex].Planetoids);
                }
                else if (selection.GetType() == typeof(Game.Planetoid))
                {
                    destinationList.SetList(GameManager.Systems[0].Planetoids[destinationList.SelectedIndex].Moons);
                }

                title.Text = string.Format("-= {0} System =-", selection.ListText);
                destinationList.ClearSelection();
                InterfaceManager.DrawStep();
            };

            RegisterControl(title);
            RegisterControl(destinationList);
            RegisterControl(travelButton);
        }

        public override void DrawStep()
        {
            GraphicConsole.Draw.Line(0, 1, 0, 25, '│');
            GraphicConsole.Draw.Line(40, 1, 40, 25, '│');
            GraphicConsole.Draw.Line(0, 1, 40, 1, '─');
            GraphicConsole.Draw.Line(0, 25, 40, 25, '─');
            GraphicConsole.Put('┌', 0, 1);
            GraphicConsole.Put('┐', 40, 1);
            GraphicConsole.Put('└', 0, 25);
            GraphicConsole.Put('┘', 40, 25);

            base.DrawStep();
        }
    }
}
