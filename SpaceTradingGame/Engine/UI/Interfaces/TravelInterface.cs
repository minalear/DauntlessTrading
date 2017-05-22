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

            RegisterControl(title);
        }
    }
}
