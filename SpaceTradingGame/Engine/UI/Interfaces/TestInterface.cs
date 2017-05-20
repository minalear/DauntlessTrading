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

            button.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("Second");
            };

            RegisterControl(title);
            RegisterControl(button);
        }
    }
}
