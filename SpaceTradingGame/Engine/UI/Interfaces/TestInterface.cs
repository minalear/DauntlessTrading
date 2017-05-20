using System;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TestInterface : Interface
    {
        public TestInterface(InterfaceManager manager)
            : base(manager)
        {
            Title title = new Title(null, "Test Interface", 50, 2, Title.TextAlignModes.Center);
            Button button = new Button(null, "Swap", 47, 4);
            Button button2 = new Button(null, "Popup", 47, 8);

            Popup popup = new Popup(null);

            button.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Second");
            };
            button2.Click += (sender, e) =>
            {
                popup.DisplayMessage("Hello there!");
            };

            RegisterControl(title);
            RegisterControl(button);
            RegisterControl(button2);
            RegisterControl(popup);
        }
    }
}
