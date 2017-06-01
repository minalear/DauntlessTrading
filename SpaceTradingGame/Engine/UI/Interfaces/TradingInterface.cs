using System;
using System.Collections.Generic;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TradingInterface : Interface
    {
        public TradingInterface(InterfaceManager manager)
            : base(manager)
        {
            Title title = new Title(null, "Trading Hub", 50, 1, Title.TextAlignModes.Center);

            RegisterControl(title);
        }
    }
}
