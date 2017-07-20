using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class CombatSimulator
    {
        public CombatGroup GroupOne { get { return groupOne; } }
        public CombatGroup GroupTwo { get { return groupTwo; } }

        public CombatSimulator(GameManager manager)
        {
            gameManager = manager;
        }

        public void SetCombatants(CombatGroup one, CombatGroup two)
        {
            groupOne = one;
            groupTwo = two;
        }
        public void SetCombatants(Ship one, Ship two)
        {
            groupOne = new CombatGroup(one);
            groupTwo = new CombatGroup(two);
        }
        
        public CombatGroup SimulateCombat()
        {
            if (groupOne == null || groupTwo == null)
                throw new ArgumentNullException("You must set CombatGroups before simulating combat.");

            //Variance to add variability to combat
            double varOne = RNG.NextDouble(0.9, 1.1);
            double varTwo = RNG.NextDouble(0.9, 1.1);

            double groupOneValue = groupOne.CombatValue;
            double groupTwoValue = groupTwo.CombatValue;

            groupOneValue *= varOne;
            groupTwoValue *= varTwo;

            //Determine winner/loser
            CombatGroup winner = (groupOneValue > groupTwoValue) ? groupOne : groupTwo;
            CombatGroup  loser = (groupOneValue < groupTwoValue) ? groupOne : groupTwo;

            //If the player lost, send them to the end screen
            if (loser.IsPlayerGroup)
                gameManager.LoseGame();

            //Calculate and reward loot
            Inventory loot = getLoot(loser);
            winner.Ships[0].Inventory.AddInventoryList(loot.GetInventoryList());

            //Destroy the losers
            foreach (Ship ship in loser.Ships)
            {
                gameManager.Destroy(ship);
            }

            groupOne = null;
            groupTwo = null;

            return winner;
        }
        public double GetCombatOdds()
        {
            if (groupOne == null || groupTwo == null)
                throw new ArgumentNullException("You must set CombatGroups before calculating odds.");

            double total = groupOne.CombatValue + groupTwo.CombatValue;
            return (groupOne.CombatValue / total).Truncate(2);
        }

        private Inventory getLoot(CombatGroup group)
        {
            Inventory loot = new Inventory();
            foreach (Ship ship in group.Ships)
            {
                //Add the ship's inventory to the loot list
                loot.AddInventoryList(ship.Inventory.GetInventoryList());

                //Add the modules of each ship to the loot list
                foreach (ShipNode node in ship.Nodes)
                {
                    if (node.Empty) continue;
                    loot.AddItem(node.Module, 1);
                }
            }

            return loot;
        }

        private CombatGroup groupOne, groupTwo;
        private GameManager gameManager;
    }

    public class CombatGroup
    {
        public Ship[] Ships { get; private set; }
        public bool IsPlayerGroup { get; private set; }
        public double CombatValue { get; private set; }

        public CombatGroup(params Ship[] ships)
        {
            Ships = ships;
            CombatValue = CalculateCombatValue();
        }

        public double CalculateCombatValue()
        {
            double totalValue = 0.0;
            foreach (Ship ship in Ships)
            {
                totalValue += ship.DefenseRating * DEFENSE_VALUE;
                totalValue += ship.FirePower * DAMAGE_VALUE;
                totalValue += ship.JumpRadius * JUMP_RADIUS_VALUE;

                if (ship.Pilot.IsPlayer) IsPlayerGroup = true;
            }

            return totalValue;
        }

        const double DEFENSE_VALUE = 1.5;
        const double DAMAGE_VALUE = 2.0;
        const double JUMP_RADIUS_VALUE = 0.01;
    }
}
