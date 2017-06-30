using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;
using SpaceTradingGame.Game;

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

            backButton = new Button(null, "Back", 0, GraphicConsole.BufferHeight - 3);
            backButton.Click += (sender, e) => InterfaceManager.ChangeInterface("Travel");

            shipLayout = new ShipLayout(null, 25, 20);
            shipLayout.Position = new System.Drawing.Point(1, 4);

            scrollingList = new ScrollingList(null, 30, 2, GraphicConsole.BufferWidth - 31, 21);
            scrollingList.FillColor = new Color4(50, 50, 50, 255);
            descriptionBox = new TextBox(null, 30, 24, GraphicConsole.BufferWidth - 31, 12);
            descriptionBox.FillColor = new Color4(50, 50, 50, 255);

            Title inventoryTitle = new Title(null, "== Inventory ==", 30 + (GraphicConsole.BufferWidth - 31) / 2, 1, Title.TextAlignModes.Center);
            RegisterControl(inventoryTitle);

            filterReset = new Button(null, "Rst", 30, 1, 3, 1);
            materialFilter = new Button(null, "Min", 34, 1, 3, 1);
            modFilter = new Button(null, "Mod", 38, 1, 3, 1);

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
            shipLayout.NodeSelect += (sender, e) =>
            {
                ShipNode node = e.SelectedShipNode;
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList(node.ModType));
                inventoryTitle.Text = string.Format("== Inventory - Filter: {0} ==", node.ModType);
            };
            filterReset.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList());
                inventoryTitle.Text = "== Inventory ==";
            };
            materialFilter.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList(ItemTypes.RawMaterial));
                inventoryTitle.Text = string.Format("== Inventory - Filter: {0} ==", ItemTypes.RawMaterial);
            };
            modFilter.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList(ItemTypes.ShipMod));
                inventoryTitle.Text = string.Format("== Inventory - Filter: {0} ==", ItemTypes.ShipMod);
            };

            //Titles
            RegisterControl(shipDesignationTitle);
            RegisterControl(shipModelTitle);

            //Buttons
            RegisterControl(backButton);
            RegisterControl(filterReset);
            RegisterControl(materialFilter);
            RegisterControl(modFilter);

            //Other
            RegisterControl(scrollingList);
            RegisterControl(descriptionBox);
            RegisterControl(shipLayout);
        }

        public override void OnEnable()
        {
            shipDesignationTitle.Text = GameManager.PlayerShip.Name;
            shipModelTitle.Text = GameManager.PlayerShip.Model;

            scrollingList.ClearList();
            List<InventorySlot> inventory = GameManager.PlayerShip.Inventory.GetInventoryList();
            setItemList(GameManager.PlayerShip.Inventory.GetInventoryList());

            shipLayout.SetShip(GameManager.PlayerShip);

            base.OnEnable();
        }
        private void setItemList(List<InventorySlot> list)
        {
            List<InventoryListItem> listItems = new List<InventoryListItem>();

            foreach (InventorySlot slot in list)
            {
                listItems.Add(new InventoryListItem(slot));
            }

            scrollingList.SetList(listItems);
        }

        private Title shipDesignationTitle, shipModelTitle;
        private Button backButton;
        private Button filterReset, materialFilter, modFilter;
        private ScrollingList scrollingList;
        private TextBox descriptionBox;
        private ShipLayout shipLayout;

        public class InventoryListItem : ListItem
        {
            public InventorySlot InventorySlot { get; set; }

            public InventoryListItem(InventorySlot slot)
            {
                this.InventorySlot = slot;
                this.ListText = string.Format("{0} - {1}", slot.InventoryItem.Name, slot.Quantity);
            }
        }
    }
}
