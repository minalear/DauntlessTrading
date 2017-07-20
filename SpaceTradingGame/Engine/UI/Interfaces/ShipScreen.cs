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

            equipButton = new Button(null, "Equip", 14, GraphicConsole.BufferHeight - 3);
            unequipButton = new Button(null, "Unequip", 21, GraphicConsole.BufferHeight - 3);

            shipLayout = new ShipLayout(null, 28, 19);
            shipLayout.Position = new System.Drawing.Point(1, GraphicConsole.BufferHeight - shipLayout.Size.Y - 3);

            previousShip = new Button(null, "◄", 1, shipLayout.Position.Y - 3);
            nextShip = new Button(null, "►", shipLayout.Size.X - 2, shipLayout.Position.Y - 3);

            scrollingList = new ScrollingList(null, 30, 2, GraphicConsole.BufferWidth - 31, 21);
            scrollingList.FillColor = new Color4(50, 50, 50, 255);
            descriptionBox = new TextBox(null, 30, 24, GraphicConsole.BufferWidth - 31, 12);
            descriptionBox.FillColor = new Color4(50, 50, 50, 255);

            Title inventoryTitle = new Title(null, "== Inventory ==", 30 + (GraphicConsole.BufferWidth - 31) / 2, 1, Title.TextAlignModes.Center);
            RegisterControl(inventoryTitle);

            shipAttackTitle =  new Title(null, " Attack: 0", 1, 4, Title.TextAlignModes.Left);
            shipDefenseTitle = new Title(null, "Defense: 0", 1, 5, Title.TextAlignModes.Left);
            shipCargoTitle =   new Title(null, "  Cargo: 0", 1, 6, Title.TextAlignModes.Left);
            shipJumpTitle =    new Title(null, "   Jump: 0", 1, 7, Title.TextAlignModes.Left);

            filterReset = new Button(null, "Rst", 30, 1, 3, 1);
            materialFilter = new Button(null, "Min", 34, 1, 3, 1);
            modFilter = new Button(null, "Mod", 38, 1, 3, 1);
            shipFilter = new Button(null, "Ship", 42, 1, 4, 1);

            #region Control Events
            scrollingList.Selected += (sender, index) =>
            {
                //Selection can be an inventory item or a ship, check
                Type typeOfSelection = scrollingList.GetSelection().GetType();
                if (typeOfSelection == typeof(Ship))
                {
                    Ship ship = (Ship)scrollingList.GetSelection();
                    descriptionBox.Text = ship.Description;
                    shipLayout.SetShip(ship);

                    updateDisplayInfo();
                }
                else
                {
                    InventoryListItem selection = (InventoryListItem)scrollingList.GetSelection();
                    Item item = selection.InventorySlot.Item;

                    descriptionBox.Text = string.Format("-{0}-\nWeight: {1}/{2}\n\n{3}",
                        item.Name,
                        item.Weight,
                        selection.InventorySlot.TotalWeight,
                        item.Description);
                }
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

                //Display equipped Mod's description
                if (!node.Empty)
                {
                    descriptionBox.Text = string.Format("-{0}-\n{1}", node.Module.Name, node.Module.Description);
                }
                else
                {
                    descriptionBox.Text = string.Empty;
                }
            };
            equipButton.Click += (sender, e) =>
            {
                if (!scrollingList.HasSelection) return;

                ListItem selectedItem = scrollingList.GetSelection();

                if (selectedItem.GetType() == typeof(Ship))
                {
                    Ship ship = (Ship)selectedItem;
                    GameManager.ChangePlayerShip(ship);

                    filterReset.Press(); //Trigger inventory reset
                }
                else
                {
                    InventorySlot slot = ((InventoryListItem)selectedItem).InventorySlot;
                    if (slot.Item.ItemType != ItemTypes.ShipMod) return;

                    if (shipLayout.HasNodeSelected)
                        GameManager.PlayerShip.EquipModule(shipLayout.SelectedNode, (ShipMod)slot.Item, true);
                    else
                        GameManager.PlayerShip.EquipModule((ShipMod)slot.Item, true);

                    setItemList(GameManager.PlayerShip.Inventory.GetInventoryList());
                    inventoryTitle.Text = "== Inventory ==";
                }

                updateDisplayInfo();
                InterfaceManager.DrawStep();
            };
            unequipButton.Click += (sender, e) =>
            {
                if (!shipLayout.HasNodeSelected) return;
                GameManager.PlayerShip.UnequipModule(shipLayout.SelectedNode, true);

                updateDisplayInfo();
                InterfaceManager.DrawStep();
            };
            filterReset.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList());
                inventoryTitle.Text = "== Inventory ==";
                scrollingList.ClearSelection();
                shipLayout.SetShip(GameManager.PlayerShip);
                updateDisplayInfo();
            };
            materialFilter.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList(ItemTypes.RawMaterial));
                inventoryTitle.Text = string.Format("== Inventory - Filter: {0} ==", ItemTypes.RawMaterial);
                scrollingList.ClearSelection();
                shipLayout.SetShip(GameManager.PlayerShip);
                updateDisplayInfo();
            };
            modFilter.Click += (sender, e) =>
            {
                setItemList(GameManager.PlayerShip.Inventory.GetInventoryList(ItemTypes.ShipMod));
                inventoryTitle.Text = string.Format("== Inventory - Filter: {0} ==", ItemTypes.ShipMod);
                scrollingList.ClearSelection();
                shipLayout.SetShip(GameManager.PlayerShip);
                updateDisplayInfo();
            };
            shipFilter.Click += (sender, e) =>
            {
                scrollingList.SetList(GameManager.PlayerFaction.OwnedShips);
                inventoryTitle.Text = string.Format("== Owned Ships ==");
                scrollingList.ClearSelection();
                shipLayout.SetShip(GameManager.PlayerShip);
                updateDisplayInfo();
            };
            #endregion

            #region Control Registration
            //Titles
            RegisterControl(shipDesignationTitle);
            RegisterControl(shipModelTitle);
            RegisterControl(shipAttackTitle);
            RegisterControl(shipDefenseTitle);
            RegisterControl(shipCargoTitle);
            RegisterControl(shipJumpTitle);

            //Buttons
            RegisterControl(backButton);
            RegisterControl(equipButton);
            RegisterControl(unequipButton);
            //RegisterControl(previousShip);
            //RegisterControl(nextShip);
            RegisterControl(filterReset);
            RegisterControl(materialFilter);
            RegisterControl(modFilter);
            RegisterControl(shipFilter);

            //Other
            RegisterControl(scrollingList);
            RegisterControl(descriptionBox);
            RegisterControl(shipLayout);
            #endregion
        }

        public override void OnEnable()
        {
            shipDesignationTitle.Text = GameManager.PlayerShip.Name;
            shipModelTitle.Text = GameManager.PlayerShip.Model;

            scrollingList.ClearList();
            List<InventorySlot> inventory = GameManager.PlayerShip.Inventory.GetInventoryList();
            setItemList(GameManager.PlayerShip.Inventory.GetInventoryList());

            shipLayout.SetShip(GameManager.PlayerShip);

            updateDisplayInfo();

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
        private void updateDisplayInfo()
        {
            shipLayout.UpdateButtons();

            if (scrollingList.HasSelection)
            {
                if (scrollingList.GetSelection().GetType() != typeof(Ship))
                {
                    InventoryListItem selection = (InventoryListItem)scrollingList.GetSelection();
                    Item item = selection.InventorySlot.Item;

                    descriptionBox.Text = string.Format("-{0}-\nWeight: {1}/{2}\n\n{3}",
                        item.Name,
                        item.Weight,
                        selection.InventorySlot.TotalWeight,
                        item.Description);
                }
            }
            else
                descriptionBox.Text = string.Empty;

            shipDesignationTitle.Text = shipLayout.Ship.Name;
            shipModelTitle.Text = shipLayout.Ship.Model;

            shipAttackTitle.Text = string.Format(" Attack: {0}", shipLayout.Ship.FirePower);
            shipDefenseTitle.Text = string.Format("Defense: {0}", shipLayout.Ship.DefenseRating);
            shipCargoTitle.Text = string.Format("  Cargo: {0}/{1}", shipLayout.Ship.Inventory.TotalWeight, shipLayout.Ship.CargoCapacity);
            shipJumpTitle.Text = string.Format("   Jump: {0}", shipLayout.Ship.JumpRadius);
        }

        private Title shipDesignationTitle, shipModelTitle;
        private Title shipAttackTitle, shipDefenseTitle, shipCargoTitle, shipJumpTitle;
        private Button backButton, equipButton, unequipButton;
        private Button filterReset, materialFilter, modFilter, shipFilter;
        private Button previousShip, nextShip;
        private ScrollingList scrollingList;
        private TextBox descriptionBox;
        private ShipLayout shipLayout;

        public class InventoryListItem : ListItem
        {
            public InventorySlot InventorySlot { get; set; }

            public InventoryListItem(InventorySlot slot)
            {
                this.InventorySlot = slot;
                this.ListText = string.Format("{0} - {1}", slot.Item.Name, slot.Quantity);
            }
        }
    }
}
