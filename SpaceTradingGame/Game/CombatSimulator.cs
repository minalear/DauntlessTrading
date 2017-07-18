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

            double varOne = RNG.NextDouble(0.9, 1.1);
            double varTwo = RNG.NextDouble(0.9, 1.1);

            double groupOneValue = groupOne.CombatValue;
            double groupTwoValue = groupTwo.CombatValue;

            groupOneValue *= varOne;
            groupTwoValue *= varTwo;

            CombatGroup winner = (groupOneValue > groupTwoValue) ? groupOne : groupTwo;
            CombatGroup  loser = (groupOneValue < groupTwoValue) ? groupOne : groupTwo;

            if (loser.IsPlayerGroup)
                gameManager.LoseGame();

            Inventory loot = getLoot(loser);
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
                loot.AddInventoryList(ship.Inventory.GetInventoryList());
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
