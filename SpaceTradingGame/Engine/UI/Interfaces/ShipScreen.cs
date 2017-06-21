using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class ShipScreen : Interface
    {
        public ShipScreen(InterfaceManager manager)
            : base(manager)
        {
            shipDesignationTitle = new Title(null, "USS Ravioli", 1, 1, Title.TextAlignModes.Left);
            shipDesignationTitle.TextColor = Color4.White;
            shipModelTitle = new Title(null, "Disney Gummi mk1*", 1, 2, Title.TextAlignModes.Left);
            shipModelTitle.TextColor = Color4.Gray;

            cockpit = new Button(null, " ", 9, 6, 3, 2);
            cockpit.FillColor = new Color4(50, 50, 50, 255);
            leftWing = new Button(null, " ", 4, 9, 3, 2);
            leftWing.FillColor = new Color4(50, 50, 50, 255);
            cargoBay = new Button(null, " ", 9, 9, 3, 2);
            cargoBay.FillColor = new Color4(50, 50, 50, 255);
            rightWing = new Button(null, " ", 14, 9, 3, 2);
            rightWing.FillColor = new Color4(50, 50, 50, 255);
            drive = new Button(null, " ", 9, 12, 3, 2);
            drive.FillColor = new Color4(50, 50, 50, 255);
            leftEngine = new Button(null, " ", 5, 14, 3, 2);
            leftEngine.FillColor = new Color4(50, 50, 50, 255);
            rightEngine = new Button(null, " ", 13, 14, 3, 2);
            rightEngine.FillColor = new Color4(50, 50, 50, 255);

            scrollingList = new ScrollingList(null, 30, 2, GraphicConsole.BufferWidth - 31, 21);
            scrollingList.FillColor = new Color4(50, 50, 50, 255);
            descriptionBox = new TextBox(null, 30, 24, GraphicConsole.BufferWidth - 31, 12);
            descriptionBox.FillColor = new Color4(50, 50, 50, 255);

            Title inventoryTitle = new Title(null, "== Inventory ==", 30 + (GraphicConsole.BufferWidth - 31) / 2, 1, Title.TextAlignModes.Center);
            RegisterControl(inventoryTitle);

            //Control Events
            scrollingList.Selected += (sender, index) =>
            {
                descriptionBox.Text = ((InventoryListItem)scrollingList.GetSelection()).InventorySlot.InventoryItem.Description;
                InterfaceManager.DrawStep();
            };
            scrollingList.Deselected += (sender) =>
            {
                descriptionBox.Text = string.Empty;
                InterfaceManager.DrawStep();
            };

            //Titles
            RegisterControl(shipDesignationTitle);
            RegisterControl(shipModelTitle);

            //Modules
            RegisterControl(cockpit);
            RegisterControl(leftWing);
            RegisterControl(cargoBay);
            RegisterControl(rightWing);
            RegisterControl(drive);
            RegisterControl(leftEngine);
            RegisterControl(rightEngine);

            //Other
            RegisterControl(scrollingList);
            RegisterControl(descriptionBox);
        }

        public override void OnEnable()
        {
            shipDesignationTitle.Text = GameManager.PlayerShip.Name;
            shipModelTitle.Text = GameManager.PlayerShip.Model;

            scrollingList.ClearList();
            List<Game.InventorySlot> inventory = GameManager.PlayerShip.Inventory.GetInventoryList();
            List<InventoryListItem> listItems = new List<InventoryListItem>();

            foreach (Game.InventorySlot slot in inventory)
            {
                listItems.Add(new InventoryListItem(slot));
            }

            scrollingList.SetList(listItems);

            base.OnEnable();
        }
        public override void DrawStep()
        {
            GraphicConsole.SetCursor(5, 6);
            GraphicConsole.Write("   ╔   ╗");
            GraphicConsole.SetCursor(5, 7);
            GraphicConsole.Write("   ║   ║");
            GraphicConsole.SetCursor(5, 8);
            GraphicConsole.Write("╔══╝   ╚══╗");
            GraphicConsole.SetCursor(5, 11);
            GraphicConsole.Write("╚═╗     ╔═╝");
            GraphicConsole.SetCursor(5, 12);
            GraphicConsole.Write("  ║     ║");
            GraphicConsole.SetCursor(5, 13);
            GraphicConsole.Write("╔═╝     ╚═╗");

            base.DrawStep();
        }

        private Title shipDesignationTitle, shipModelTitle;
        private Button cockpit, leftWing, rightWing, cargoBay, drive, leftEngine, rightEngine;
        private ScrollingList scrollingList;
        private TextBox descriptionBox;

        public class InventoryListItem : ListItem
        {
            public Game.InventorySlot InventorySlot { get; set; }

            public InventoryListItem(Game.InventorySlot slot)
            {
                this.InventorySlot = slot;
                this.ListText = string.Format("{0} - {1}", slot.InventoryItem.Name, slot.Quantity);
            }
        }
    }
}
