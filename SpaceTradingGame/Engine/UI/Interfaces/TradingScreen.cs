using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Game;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TradingScreen : Interface
    {
        public TradingScreen(InterfaceManager manager)
            : base(manager)
        {
            screenTitle = new Title(null, "Trading Hub", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);

            inventoryList = new ScrollingList(null, 1, 2, 25, GraphicConsole.BufferHeight - 3);
            inventoryList.FillColor = Color4.Gray;
            availableItemsList = new ScrollingList(null, GraphicConsole.BufferWidth - 26, 2, 25, GraphicConsole.BufferHeight - 3);
            availableItemsList.FillColor = Color4.SlateBlue;

            offeredList = new ScrollingList(null, 38, 2, 25, 9);
            offeredList.FillColor = Color4.Snow;
            interestedList = new ScrollingList(null, 38, GraphicConsole.BufferHeight - 10, 25, 9);
            interestedList.FillColor = Color4.DarkSeaGreen;

            playerRemoveOne = new Button(null, "<1", 26, 2, 6, 3);
            playerRemoveOne.FillColor = Color4.DarkRed;
            playerRemoveTen = new Button(null, "<10", 26, 5, 6, 3);
            playerRemoveTen.FillColor = Color4.DarkGreen;
            playerRemoveHundred = new Button(null, "<100", 26, 8, 6, 3);
            playerRemoveHundred.FillColor = Color4.DarkBlue;

            playerAddOne = new Button(null, "1>", 32, 2, 6, 3);
            playerAddOne.FillColor = Color4.Red;
            playerAddTen = new Button(null, "10>", 32, 5, 6, 3);
            playerAddTen.FillColor = Color4.Green;
            playerAddHundred = new Button(null, "100>", 32, 8, 6, 3);
            playerAddHundred.FillColor = Color4.Blue;
            
            RegisterControl(screenTitle);
            RegisterControl(inventoryList);
            RegisterControl(availableItemsList);
            RegisterControl(offeredList);
            RegisterControl(interestedList);
            RegisterControl(playerAddOne);
            RegisterControl(playerAddTen);
            RegisterControl(playerAddHundred);
            RegisterControl(playerRemoveOne);
            RegisterControl(playerRemoveTen);
            RegisterControl(playerRemoveHundred);
        }

        private Title screenTitle;
        private ScrollingList inventoryList, availableItemsList;
        private ScrollingList offeredList, interestedList;
        private Button playerRemoveOne, playerRemoveTen, playerRemoveHundred;
        private Button playerAddOne, playerAddTen, playerAddHundred;
    }
}
