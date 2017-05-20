using System;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class SecondInterface : Interface
    {
        public SecondInterface(InterfaceManager manager)
            : base(manager)
        {
            Title title = new Title(null, "Second Interface", 50, 2, Title.TextAlignModes.Center);
            Button button = new Button(null, "Swap", 47, 4);

            button.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Test");
            };

            RegisterControl(title);
            RegisterControl(button);
        }
    }
}
