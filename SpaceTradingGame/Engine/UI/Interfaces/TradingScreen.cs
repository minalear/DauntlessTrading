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

            Color4 controlFillColor = new Color4(0.15f, 0.15f, 0.15f, 1f);
            Color4 darkerColor = new Color4(0.1f, 0.1f, 0.1f, 1f);
            Color4 lighterColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            inventoryList = new ScrollingList(null, 1, 2, 25, GraphicConsole.BufferHeight - 3);
            inventoryList.FillColor = controlFillColor;
            availableItemsList = new ScrollingList(null, GraphicConsole.BufferWidth - 26, 2, 25, GraphicConsole.BufferHeight - 3);
            availableItemsList.FillColor = controlFillColor;

            offeredList = new ScrollingList(null, 38, 2, 25, 9);
            offeredList.FillColor = controlFillColor;
            interestedList = new ScrollingList(null, 37, GraphicConsole.BufferHeight - 10, 25, 9);
            interestedList.FillColor = controlFillColor;

            playerRemoveOne = new Button(null, "<1", 26, 2, 6, 3);
            playerRemoveOne.FillColor = darkerColor;
            playerRemoveTen = new Button(null, "<10", 26, 5, 6, 3);
            playerRemoveTen.FillColor = darkerColor;
            playerRemoveHundred = new Button(null, "<100", 26, 8, 6, 3);
            playerRemoveHundred.FillColor = darkerColor;

            playerAddOne = new Button(null, "1>", 32, 2, 6, 3);
            playerAddOne.FillColor = lighterColor;
            playerAddTen = new Button(null, "10>", 32, 5, 6, 3);
            playerAddTen.FillColor = lighterColor;
            playerAddHundred = new Button(null, "100>", 32, 8, 6, 3);
            playerAddHundred.FillColor = lighterColor;

            playerRemoveOne.Click += (sender, e) => RemovePlayerItem(offeredList.GetSelection(), 1);
            playerRemoveTen.Click += (sender, e) => RemovePlayerItem(offeredList.GetSelection(), 10);
            playerRemoveHundred.Click += (sender, e) => RemovePlayerItem(offeredList.GetSelection(), 100);

            playerAddOne.Click += (sender, e) => AddPlayerItem(inventoryList.GetSelection(), 1);
            playerAddTen.Click += (sender, e) => AddPlayerItem(inventoryList.GetSelection(), 10);
            playerAddHundred.Click += (sender, e) => AddPlayerItem(inventoryList.GetSelection(), 100);

            computerAddOne = new Button(null, "<1", 62, GraphicConsole.BufferHeight - 4, 6, 3);
            computerAddOne.FillColor = lighterColor;
            computerAddTen = new Button(null, "<10", 62, GraphicConsole.BufferHeight - 7, 6, 3);
            computerAddTen.FillColor = lighterColor;
            computerAddHundred = new Button(null, "<100", 62, GraphicConsole.BufferHeight - 10, 6, 3);
            computerAddHundred.FillColor = lighterColor;

            computerRemoveOne = new Button(null, "1>", 68, GraphicConsole.BufferHeight - 4, 6, 3);
            computerRemoveOne.FillColor = darkerColor;
            computerRemoveTen = new Button(null, "10>", 68, GraphicConsole.BufferHeight - 7, 6, 3);
            computerRemoveTen.FillColor = darkerColor;
            computerRemoveHundred = new Button(null, "100>", 68, GraphicConsole.BufferHeight - 10, 6, 3);
            computerRemoveHundred.FillColor = darkerColor;

            computerRemoveOne.Click += (sender, e) => RemoveComputerItem(interestedList.GetSelection(), 1);
            computerRemoveTen.Click += (sender, e) => RemoveComputerItem(interestedList.GetSelection(), 10);
            computerRemoveHundred.Click += (sender, e) => RemoveComputerItem(interestedList.GetSelection(), 100);

            computerAddOne.Click += (sender, e) => AddComputerItem(availableItemsList.GetSelection(), 1);
            computerAddTen.Click += (sender, e) => AddComputerItem(availableItemsList.GetSelection(), 10);
            computerAddHundred.Click += (sender, e) => AddComputerItem(availableItemsList.GetSelection(), 100);

            playerInventory = new List<ListItem>() { "Gold", "Ivory", "Copper" };
            computerInventory = new List<ListItem>() { "Rose Gold", "Rose Ivory", "Rose Copper" };

            inventoryList.SetList(playerInventory);
            availableItemsList.SetList(computerInventory);

            #region Control Registration
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
            RegisterControl(computerAddOne);
            RegisterControl(computerAddTen);
            RegisterControl(computerAddHundred);
            RegisterControl(computerRemoveOne);
            RegisterControl(computerRemoveTen);
            RegisterControl(computerRemoveHundred);
            #endregion
        }

        public void AddPlayerItem(ListItem item, int number)
        {
            if (inventoryList.HasSelection)
            {
                inventoryList.RemoveItem(item);
                offeredList.AddItem(item);

                InterfaceManager.DrawStep();
            }
        }
        public void RemovePlayerItem(ListItem item, int number)
        {
            if (offeredList.HasSelection)
            {
                offeredList.RemoveItem(item);
                inventoryList.AddItem(item);

                InterfaceManager.DrawStep();
            }
        }

        public void AddComputerItem(ListItem item, int number)
        {
            if (availableItemsList.HasSelection)
            {
                availableItemsList.RemoveItem(item);
                interestedList.AddItem(item);

                InterfaceManager.DrawStep();
            }
        }
        public void RemoveComputerItem(ListItem item, int number)
        {
            if (interestedList.HasSelection)
            {
                interestedList.RemoveItem(item);
                availableItemsList.AddItem(item);

                InterfaceManager.DrawStep();
            }
        }

        private Title screenTitle;
        private ScrollingList inventoryList, availableItemsList;
        private ScrollingList offeredList, interestedList;
        private Button playerRemoveOne, playerRemoveTen, playerRemoveHundred;
        private Button playerAddOne, playerAddTen, playerAddHundred;
        private Button computerRemoveOne, computerRemoveTen, computerRemoveHundred;
        private Button computerAddOne, computerAddTen, computerAddHundred;

        private List<ListItem> playerInventory;
        private List<ListItem> computerInventory;
    }
}
