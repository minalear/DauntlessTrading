﻿using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class CombatScreen : Interface
    {
        public CombatScreen(InterfaceManager manager) 
            : base(manager)
        {
            backButton = new Button(null, "Back", 1, 1);
            backButton.Click += (sender, e) => InterfaceManager.ChangeInterface("Travel");

            attackButton = new Button(null, "Attack", 1, 4);
            attackButton.Click += (sender, e) =>
            {
                if (!shipList.HasSelection) return;

                GameManager.CombatSimulator.SetCombatants(GameManager.PlayerShip, (Ship)shipList.GetSelection());
                CombatGroup victor = GameManager.CombatSimulator.SimulateCombat();
                updateDisplayInformation();

                descriptionBox.Text = string.Format("{0} won!", victor.Ships[0].Name);
            };

            scanButton = new Button(null, "Scan", 1, 7);
            scanButton.Click += (sender, e) =>
            {
                if (!shipList.HasSelection) return;

                GameManager.CombatSimulator.SetCombatants(GameManager.PlayerShip, (Ship)shipList.GetSelection());
                double oddsToWin = GameManager.CombatSimulator.GetCombatOdds();

                string text = string.Format("{0}% chance to win.\n-\n", oddsToWin * 100.0);

                if (GameManager.CombatSimulator.GroupTwo.Ships[0].CanBeScanned(GameManager.PlayerShip))
                {
                    //List the equipped modules
                    foreach (ShipNode node in GameManager.CombatSimulator.GroupTwo.Ships[0].Nodes)
                    {
                        if (node.Empty) continue;
                        text += string.Format("{0} - {1}\n", node.ModType, node.Module.Name);
                    }

                    text += "-\n";

                    //List the carried items
                    List<InventorySlot> inventory = GameManager.CombatSimulator.GroupTwo.Ships[0].Inventory.GetInventoryList();
                    foreach (InventorySlot slot in inventory)
                    {
                        text += string.Format("{0} - #{1}\n", slot.Item.Name, slot.Quantity);
                    }
                }
                else
                    text += "Cannot be scanned.";

                descriptionBox.Text = text;
            };

            shipList = new ScrollingList(null, GraphicConsole.BufferWidth - 41, 1, 40, 20);
            shipList.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            descriptionBox = new TextBox(null, GraphicConsole.BufferWidth - 41, 22, 40, 14);
            descriptionBox.FillColor = new Color4(0.2f, 0.2f, 0.2f, 1f);

            shipList.Selected += (sender, e) =>
            {
                descriptionBox.Text = getShipDescription((Ship)shipList.GetSelection());
                InterfaceManager.DrawStep();
            };

            RegisterControl(backButton);
            RegisterControl(attackButton);
            RegisterControl(scanButton);
            RegisterControl(shipList);
            RegisterControl(descriptionBox);
        }

        public override void OnEnable()
        {
            updateDisplayInformation();
            descriptionBox.Text = string.Empty;

            base.OnEnable();
        }

        private string getShipDescription(Ship ship)
        {
            return string.Format("{0} - {1}\n{2} - {3}", ship.Name, ship.Model, ship.Pilot.Name, ship.Faction.Name);
        }
        private void updateDisplayInformation()
        {
            shipList.SetList(GameManager.GetShipsInJumpRadius(GameManager.PlayerShip));
        }

        private Button backButton, attackButton, scanButton;
        private ScrollingList shipList;
        private TextBox descriptionBox;
    }
}
