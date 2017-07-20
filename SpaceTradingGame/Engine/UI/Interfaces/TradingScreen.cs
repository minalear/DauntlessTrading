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

            playerValueTitle = new Title(null, "Player Value", 50, 11, Title.TextAlignModes.Center);
            computerValueTitle = new Title(null, "Computer Value", 51, GraphicConsole.BufferHeight - 11, Title.TextAlignModes.Center);
            differenceValueTitle = new Title(null, "Difference Value", GraphicConsole.BufferWidth / 2, GraphicConsole.BufferHeight / 2, Title.TextAlignModes.Center);

            playerCreditsTitle = new Title(null, "Player Credits", 1, 1, Title.TextAlignModes.Left);
            computerCreditsTitle = new Title(null, "Computer Credits", GraphicConsole.BufferWidth - 26, 1, Title.TextAlignModes.Left);

            backButton = new Button(null, "Back", 0, 0, 6, 1);
            backButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("System");
            };

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

            makeOfferButton = new Button(null, "Make Offer", GraphicConsole.BufferWidth / 2 - 6, GraphicConsole.BufferHeight / 2 - 3);
            makeOfferButton.Click += (sender, e) => makeOffer();

            computerInventory = new Inventory();

            #region Control Registration
            RegisterControl(screenTitle);
            RegisterControl(playerValueTitle);
            RegisterControl(computerValueTitle);
            RegisterControl(playerCreditsTitle);
            RegisterControl(computerCreditsTitle);
            RegisterControl(differenceValueTitle);
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
            RegisterControl(makeOfferButton);
            RegisterControl(backButton);
            #endregion
        }

        public override void OnEnable()
        {
            computerMarket = GameManager.CurrentSystem.SystemMarket;
            computerInventory = GameManager.CurrentSystem.SystemMarket.MarketInventory;
            updateScreenInformation();
            updateLists();

            base.OnEnable();
        }
        public override void OnDisable()
        {
            inventoryList.ClearList();
            availableItemsList.ClearList();
            interestedList.ClearList();
            offeredList.ClearList();

            base.OnDisable();
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
                    offeredItem = new TradingListItem(tradingItem.Item, 0);
                    offeredList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                updatePrices();
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();
                updateScreenInformation();

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
                    offeredItem = new TradingListItem(tradingItem.Item, 0);
                    inventoryList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                updatePrices();
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();
                updateScreenInformation();

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
                    offeredItem = new TradingListItem(tradingItem.Item, 0);
                    interestedList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                updatePrices();
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();
                updateScreenInformation();

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
                    offeredItem = new TradingListItem(tradingItem.Item, 0);
                    availableItemsList.AddItem(offeredItem);
                }
                offeredItem.Quantity += number;

                //Update displays
                updatePrices();
                tradingItem.UpdateDisplayInformation();
                offeredItem.UpdateDisplayInformation();
                updateScreenInformation();

                InterfaceManager.DrawStep();
            }
        }

        private void makeOffer()
        {
            int playerValue = 0;
            foreach (TradingListItem item in offeredList.Items)
            {
                playerValue += item.PriceTotal;
            }

            int computerValue = 0;
            foreach (TradingListItem item in interestedList.Items)
            {
                computerValue += item.PriceTotal;
            }

            int diff = playerValue - computerValue;

            if (diff < 0) //Player pays credits
            {
                diff = Math.Abs(diff);
                if (GameManager.PlayerShip.Inventory.Credits >= diff)
                {
                    differenceValueTitle.Text = "Good deal!";
                    makeTrade();
                    GameManager.PlayerShip.Inventory.Credits -= diff;
                    computerInventory.Credits += diff;
                }
                else
                {
                    differenceValueTitle.Text = "Not enough player credits.";
                }
            }
            else //AI pays credit
            {
                diff = Math.Abs(diff);
                if (computerInventory.Credits >= diff)
                {
                    differenceValueTitle.Text = "Good deal!";
                    makeTrade();
                    
                    GameManager.PlayerShip.Inventory.Credits += diff;
                    computerInventory.Credits -= diff;
                }
                else
                {
                    differenceValueTitle.Text = "Not enough computer credits.";
                }
            }

            updateScreenInformation();
            updateLists();
            InterfaceManager.DrawStep();
        }
        private void makeTrade()
        {
            //Loop through each item offered and add it to the AI's inventory
            foreach (TradingListItem item in offeredList.Items)
            {
                GameManager.PlayerShip.Inventory.RemoveItem(item.Item, item.Quantity);
                computerInventory.AddItem(item.Item, item.Quantity);
            }

            //Loop through each interested item and add it to the PC's inventory
            foreach (TradingListItem item in interestedList.Items)
            {
                computerInventory.RemoveItem(item.Item, item.Quantity);
                GameManager.PlayerShip.Inventory.AddItem(item.Item, item.Quantity);
            }

            offeredList.ClearList();
            interestedList.ClearList();
        }

        private bool hasItem(ScrollingList list, TradingListItem item, out TradingListItem offeredItem)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if ((list.Items[i] as TradingListItem).Item == item.Item)
                {
                    offeredItem = (TradingListItem)list.Items[i];
                    return true;
                }
            }

            offeredItem = null;
            return false;
        }
        private void updatePrices()
        {
            foreach (TradingListItem item in interestedList.Items)
            {
                item.PriceTotal = computerMarket.CalculateSellPrice(item.Item, item.Quantity);
            }
            foreach (TradingListItem item in offeredList.Items)
            {
                item.PriceTotal = computerMarket.CalculateBuyPrice(item.Item, item.Quantity);
            }
        }
        private void updateScreenInformation()
        {
            double playerValue = 0.0;
            foreach (TradingListItem item in offeredList.Items)
            {
                playerValue += item.PriceTotal;
            }
            playerValueTitle.Text = playerValue.ToString();

            double computerValue = 0.0;
            foreach (TradingListItem item in interestedList.Items)
            {
                computerValue += item.PriceTotal;
            }
            computerValueTitle.Text = computerValue.ToString();

            double diff = playerValue - computerValue;
            if (diff < 0.0)
            {
                differenceValueTitle.TextColor = Color4.Red;
                differenceValueTitle.Text = "◄ " + Math.Abs(diff).ToString() + "δ";
            }
            else if (diff == 0.0)
            {
                differenceValueTitle.TextColor = Color4.White;
                differenceValueTitle.Text = Math.Abs(diff).ToString() + "δ";
            }
            else
            {
                differenceValueTitle.TextColor = Color4.Green;
                differenceValueTitle.Text = Math.Abs(diff).ToString() + "δ ►";
            }

            playerCreditsTitle.Text = string.Format("Credits: {0}δ", GameManager.PlayerShip.Inventory.Credits);
            computerCreditsTitle.Text = string.Format("Credits: {0}δ", computerInventory.Credits);
        }
        private void updateLists()
        {
            inventoryList.ClearList();
            availableItemsList.ClearList();

            List<InventorySlot> inventory = computerInventory.GetInventoryList();
            foreach (InventorySlot slot in inventory)
            {
                availableItemsList.AddItem(new TradingListItem(slot.InventoryItem, slot.Quantity));
            }

            List<InventorySlot> pcInventory = GameManager.PlayerShip.Inventory.GetInventoryList();
            foreach (InventorySlot slot in pcInventory)
            {
                inventoryList.AddItem(new TradingListItem(slot.InventoryItem, slot.Quantity));
            }
        }

        private Title screenTitle, playerCreditsTitle, computerCreditsTitle;
        private Title playerValueTitle, computerValueTitle, differenceValueTitle;
        private ScrollingList inventoryList, availableItemsList;
        private ScrollingList offeredList, interestedList;
        private Button playerRemoveOne, playerRemoveTen, playerRemoveHundred;
        private Button playerAddOne, playerAddTen, playerAddHundred;
        private Button computerRemoveOne, computerRemoveTen, computerRemoveHundred;
        private Button computerAddOne, computerAddTen, computerAddHundred;
        private Button makeOfferButton;
        private Button backButton;

        private Market computerMarket;
        private Inventory computerInventory; //Temporary

        public class TradingListItem : ListItem
        {
            public static int BufferWidth = 25;
            public static int MaxTitleWidth = 11;

            public Item Item;
            public int Quantity;
            public int PriceTotal;

            public TradingListItem(Item item, int quantity)
            {
                this.Item = item;
                this.Quantity = quantity;

                UpdateDisplayInformation();
            }

            public void UpdateDisplayInformation()
            {
                //MaterialName     Price      xQuantity
                string name = string.Empty;
                if (Item.Name.Length <= MaxTitleWidth)
                {
                    name = Item.Name;
                    for (int i = 0; i < MaxTitleWidth - Item.Name.Length; i++)
                        name += " ";
                }
                else
                {
                    //MaxTitleWidth - 3 for "..."
                    for (int i = 0; i < MaxTitleWidth - 3; i++)
                    {
                        name += Item.Name[i];
                    }
                    name += "...";
                }

                string price = PriceTotal.ToString();
                string quantity = "x" + Quantity.ToString();
                
                int frontSpace = 2;
                int backSpace = BufferWidth - (MaxTitleWidth + frontSpace) - (quantity.Length + 1);

                string formattedText = name + new string(' ', frontSpace) + price + new string(' ', backSpace) + quantity;
                this.ListText = formattedText;
            }
        }
    }
}
