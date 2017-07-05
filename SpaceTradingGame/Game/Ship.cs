using System;
using System.Collections.Generic;
using OpenTK;

namespace SpaceTradingGame.Game
{
    public class Ship : Engine.UI.Controls.ListItem
    {
        private string shipName;
        private string shipModel;
        private int firePower;
        private int defenseRating;
        private int cargoCapacity;
        private double baseJumpRadius;

        private Inventory shipInventory;

        private List<ShipNode> nodes;

        public Ship()
        {
            this.shipName = "Name";
            this.shipModel = "Maverick Class I";

            this.shipInventory = new Inventory();
            this.nodes = new List<ShipNode>();
            this.WorldPosition = Vector2.Zero;

            this.ListText = shipModel;
        }
        public Ship(string name, string model)
        {
            this.shipName = name;
            this.shipModel = model;

            this.shipInventory = new Inventory();
            this.nodes = new List<ShipNode>();
            this.WorldPosition = Vector2.Zero;

            this.ListText = shipModel;
        }

        public void EquipModification(ShipMod mod, bool removeFromInventory)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ModType == mod.ModType)
                {
                    if (removeFromInventory) shipInventory.RemoveItem(mod, 1);
                    if (!nodes[i].Empty) shipInventory.AddItem(nodes[i].Modification, 1);

                    nodes[i].Empty = false;
                    nodes[i].Modification = mod;

                    UpdateShipStats();

                    return;
                }
            }
        }
        public void EquipModification(ShipNode node, ShipMod mod, bool removeFromInventory)
        {
            if (node.ModType == mod.ModType || node.ModType == ShipMod.ShipModTypes.Any)
            {
                if (removeFromInventory) shipInventory.RemoveItem(mod, 1);
                if (!node.Empty) shipInventory.AddItem(node.Modification, 1);

                node.Empty = false;
                node.Modification = mod;

                UpdateShipStats();
            }
        }
        public void UpdateShipStats()
        {
            firePower = 0;
            defenseRating = 0;
            cargoCapacity = 0;
            baseJumpRadius = 0;

            foreach (ShipNode node in nodes)
            {
                if (!node.Empty)
                {
                    firePower += node.Modification.FirePowerMod;
                    defenseRating += node.Modification.DefenseMod;
                    cargoCapacity += node.Modification.CargoMod;
                    baseJumpRadius += node.Modification.JumpMod;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("== {0} ==\nFire: {1}\nDfns: {2}\nCargo: {3}\nJump: {4}", 
                shipModel, firePower, defenseRating, cargoCapacity, baseJumpRadius);
        }

        public string Name { get { return shipName; } set { shipName = value; } }
        public string Model { get { return shipModel; } set { shipModel = value; } }
        public int FirePower { get { return this.firePower; } set { this.firePower = value; } }
        public int DefenseRating { get { return this.defenseRating; } set { this.defenseRating = value; } }
        public int CargoCapacity { get { return cargoCapacity; } set { cargoCapacity = value; } }
        public double BaseJumpRadius { get { return baseJumpRadius; } set { baseJumpRadius = value; } }
        public Inventory Inventory { get { return shipInventory; } set { shipInventory = value; } }
        public List<ShipNode> Nodes { get { return nodes; } set { nodes = value; } }
        public Faction Faction { get; set; }
        public Vector2 WorldPosition { get; set; }
    }
}
