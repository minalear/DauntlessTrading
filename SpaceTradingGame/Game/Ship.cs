using System;
using System.Collections.Generic;
using OpenTK;

namespace SpaceTradingGame.Game
{
    public class Ship : Engine.UI.Controls.ListItem, ICloneable
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
            ID = _nextValidID++;

            this.shipName = "Name";
            this.shipModel = "Maverick Class I";

            this.MoveSpeed = 450f;

            this.shipInventory = new Inventory();
            this.nodes = new List<ShipNode>();
            this.WorldPosition = Vector2.Zero;

            this.ListText = shipModel;
        }
        public Ship(string name, string model)
        {
            ID = _nextValidID++;

            this.shipName = name;
            this.shipModel = model;

            this.MoveSpeed = 450f;

            this.shipInventory = new Inventory();
            this.nodes = new List<ShipNode>();
            this.WorldPosition = Vector2.Zero;

            this.ListText = shipModel;
        }

        public void EquipModule(ShipMod mod, bool removeFromInventory)
        {
            //Attempt to equip module to a node with the same type
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ModType == mod.ModType)
                {
                    if (removeFromInventory) shipInventory.RemoveItem(mod, 1);
                    if (!nodes[i].Empty) shipInventory.AddItem(nodes[i].Module, 1);

                    nodes[i].Empty = false;
                    nodes[i].Module = mod;

                    UpdateShipStats();

                    return;
                }
            }

            //Failed to find the specified node, now try to equip to the first empty node with type any
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ModType == ShipMod.ShipModTypes.Any && nodes[i].Empty)
                {
                    nodes[i].Empty = false;
                    nodes[i].Module = mod;

                    UpdateShipStats();

                    return;
                }
            }
        }
        public void EquipModule(ShipNode node, ShipMod mod, bool removeFromInventory)
        {
            if (node.ModType == mod.ModType || node.ModType == ShipMod.ShipModTypes.Any)
            {
                if (removeFromInventory) shipInventory.RemoveItem(mod, 1);
                if (!node.Empty) shipInventory.AddItem(node.Module, 1);

                node.Empty = false;
                node.Module = mod;

                UpdateShipStats();
            }
        }
        public void UnequipModule(ShipNode node, bool addToInventory)
        {
            if (node.Empty) return;
            if (addToInventory) shipInventory.AddItem(node.Module, 1);
            node.Empty = true;
            node.Module = null;

            UpdateShipStats();
        }
        public bool HasModule(ShipMod.ShipModTypes type)
        {
            foreach (ShipNode node in nodes)
            {
                if (node.ModType == type && !node.Empty)
                    return true;
            }

            return false;
        }
        public bool CanFly()
        {
            return (HasModule(ShipMod.ShipModTypes.Cockpit) && HasModule(ShipMod.ShipModTypes.WarpCore));
        }
        public bool CanBeDetected(Ship scanner)
        {
            return (scanner.ScanningPower >= StealthPower);
        }
        public bool CanBeScanned(Ship scanner)
        {
            return (scanner.ScanningPower >= ScanningDefense);
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
                    firePower += node.Module.FirePowerMod;
                    defenseRating += node.Module.DefenseMod;
                    cargoCapacity += node.Module.CargoMod;
                    baseJumpRadius += node.Module.JumpMod;

                    if (node.Module.ModType == ShipMod.ShipModTypes.EmShield)
                    {
                        ScanningDefense += node.Module.Grade;
                    }
                    else if (node.Module.ModType == ShipMod.ShipModTypes.Scanner)
                    {
                        ScanningPower += node.Module.Grade;
                    }
                    else if (node.Module.ModType == ShipMod.ShipModTypes.StealthGen)
                    {
                        StealthPower += node.Module.Grade;
                    }
                }
            }
        }
        public void SetCurrentSystem(StarSystem system)
        {
            if (CurrentSystem != null)
                CurrentSystem.VisitingShips.Remove(this);

            WorldPosition = system.Coordinates;
            CurrentSystem = system;
            CurrentSystem.VisitingShips.Add(this);
        }
        public void SetPilot(Pilot pilot)
        {
            Pilot = pilot;
            Faction = pilot.Faction;

            pilot.Ship = this;
        }

        public override string ToString()
        {
            return string.Format("== {0} ==\nFire: {1}\nDfns: {2}\nCargo: {3}\nJump: {4}", 
                shipModel, firePower, defenseRating, cargoCapacity, baseJumpRadius);
        }

        public object Clone()
        {
            Ship newShip = new Ship(Name, Model);
            foreach (ShipNode node in Nodes)
            {
                newShip.Nodes.Add((ShipNode)node.Clone());
            }

            newShip.UpdateShipStats();
            newShip.Value = Value;
            newShip.Description = Description;

            return newShip;
        }

        public int ID { get; private set; }
        public string Name { get { return shipName; } set { shipName = value; } }
        public string Model { get { return shipModel; } set { shipModel = value; } }
        public int FirePower { get { return this.firePower; } set { this.firePower = value; } }
        public int DefenseRating { get { return this.defenseRating; } set { this.defenseRating = value; } }
        public int CargoCapacity { get { return cargoCapacity; } set { cargoCapacity = value; } }
        public double JumpRadius { get { return baseJumpRadius; } set { baseJumpRadius = value; } }
        public float MoveSpeed { get; set; }
        public string Description { get; set; }
        public int ScanningPower { get; set; }
        public int ScanningDefense { get; set; }
        public int StealthPower { get; set; }
        public Inventory Inventory { get { return shipInventory; } set { shipInventory = value; } }
        public List<ShipNode> Nodes { get { return nodes; } set { nodes = value; } }
        public int Value { get; set; }
        public Pilot Pilot { get; private set; }
        public Faction Faction { get; set; }
        public Vector2 WorldPosition { get; set; }
        public StarSystem CurrentSystem { get; private set; }

        private static int _nextValidID = 0;
        public static void ResetIDCounter()
        {
            _nextValidID = 0;
        }
    }
}
