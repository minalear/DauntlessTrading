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

            TradingListItem.BufferWidth = 25; //Set to the width of the inventory screens

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

            playerInventory = new List<TradingListItem>()
            {
                new TradingListItem(Material.Gold, 100),
                new TradingListItem(Material.Copper, 58),
                new TradingListItem(Material.Hydrogen, 1287)
            };
            computerInventory = new List<TradingListItem>()
            {
                new TradingListItem(Material.Gold, 1200),
                new TradingListItem(Material.Copper, 15),
                new TradingListItem(Material.Hydrogen, 8712)
            };

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
                TradingListItem tradingItem = (TradingListItem)item;

                //Check if the requested number exceeds the amount in the inventory
                number = (tradingItem.Quantity >= number) ? number : tradingItem.Quantity;
                tradingItem.Quantity -= number;

                //If the inventory has run out of items, remove it from the list
                if (tradingItem.Quantity <= 0) inventoryList.RemoveItem(item);

                //Check if the offered items list has the item, if it does update, otherwise add it
                TradingListItem offeredItem;
                if (!hasItem(offeredList, tradingItem, out offeredItem))
                {
                    offeredItem = new TradingListItem(tradingItem.Material, 0);
                    offeredList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();

                InterfaceManager.DrawStep();
            }
        }
        public void RemovePlayerItem(ListItem item, int number)
        {
            if (offeredList.HasSelection)
            {
                TradingListItem tradingItem = (TradingListItem)item;

                //Check if the requested number exceeds the amount in the inventory
                number = (tradingItem.Quantity >= number) ? number : tradingItem.Quantity;
                tradingItem.Quantity -= number;

                //If the inventory has run out of items, remove it from the list
                if (tradingItem.Quantity <= 0) offeredList.RemoveItem(item);

                //Check if the offered items list has the item, if it does update, otherwise add it
                TradingListItem offeredItem;
                if (!hasItem(inventoryList, tradingItem, out offeredItem))
                {
                    offeredItem = new TradingListItem(tradingItem.Material, 0);
                    inventoryList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();

                InterfaceManager.DrawStep();
            }
        }

        public void AddComputerItem(ListItem item, int number)
        {
            if (availableItemsList.HasSelection)
            {
                TradingListItem tradingItem = (TradingListItem)item;

                //Check if the requested number exceeds the amount in the inventory
                number = (tradingItem.Quantity >= number) ? number : tradingItem.Quantity;
                tradingItem.Quantity -= number;

                //If the inventory has run out of items, remove it from the list
                if (tradingItem.Quantity <= 0) availableItemsList.RemoveItem(item);

                //Check if the offered items list has the item, if it does update, otherwise add it
                TradingListItem offeredItem;
                if (!hasItem(interestedList, tradingItem, out offeredItem))
                {
                    offeredItem = new TradingListItem(tradingItem.Material, 0);
                    interestedList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();

                InterfaceManager.DrawStep();
            }
        }
        public void RemoveComputerItem(ListItem item, int number)
        {
            if (interestedList.HasSelection)
            {
                TradingListItem tradingItem = (TradingListItem)item;

                //Check if the requested number exceeds the amount in the inventory
                number = (tradingItem.Quantity >= number) ? number : tradingItem.Quantity;
                tradingItem.Quantity -= number;

                //If the inventory has run out of items, remove it from the list
                if (tradingItem.Quantity <= 0) interestedList.RemoveItem(item);

                //Check if the offered items list has the item, if it does update, otherwise add it
                TradingListItem offeredItem;
                if (!hasItem(availableItemsList, tradingItem, out offeredItem))
                {
                    offeredItem = new TradingListItem(tradingItem.Material, 0);
                    availableItemsList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();

                InterfaceManager.DrawStep();
            }
        }

        private bool hasItem(ScrollingList list, TradingListItem item, out TradingListItem offeredItem)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if ((list.Items[i] as TradingListItem).Material == item.Material)
                {
                    offeredItem = (TradingListItem)list.Items[i];
                    return true;
                }
            }

            offeredItem = null;
            return false;
        }

        private Title screenTitle;
        private ScrollingList inventoryList, availableItemsList;
        private ScrollingList offeredList, interestedList;
        private Button playerRemoveOne, playerRemoveTen, playerRemoveHundred;
        private Button playerAddOne, playerAddTen, playerAddHundred;
        private Button computerRemoveOne, computerRemoveTen, computerRemoveHundred;
        private Button computerAddOne, computerAddTen, computerAddHundred;

        private List<TradingListItem> playerInventory;
        private List<TradingListItem> computerInventory;

        public class TradingListItem : ListItem
        {
            public static int BufferWidth = 25;
            public Material Material;
            public int Quantity;

            public TradingListItem(Material material, int quantity)
            {
                this.Material = material;
                this.Quantity = quantity;

                UpdateDisplayInformation();
            }

            public void UpdateDisplayInformation()
            {
                //MaterialName     Price      xQuantity
                string name = Material.Name;
                string price = Material.BaseValue.ToString();
                string quantity = "x" + Quantity.ToString();

                int bufferLeft = BufferWidth - (name.Length + price.Length + quantity.Length);
                int frontSpace = (bufferLeft % 2 == 0) ? bufferLeft / 2 : bufferLeft / 2 + 1;
                int backSpace = bufferLeft / 2;

                string formattedText = name + new string(' ', frontSpace) + price + new string(' ', backSpace) + quantity;
                this.ListText = formattedText;
            }
        }
    }
}
