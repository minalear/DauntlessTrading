using System;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class CombatSimulator
    {
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

            double groupOneValue = groupOne.GetCombatValue();
            double groupTwoValue = groupTwo.GetCombatValue();

            groupOneValue *= varOne;
            groupTwoValue *= varTwo;

            return (groupOneValue > groupTwoValue) ? groupOne : groupTwo;
        }

        private CombatGroup groupOne, groupTwo;
        private GameManager gameManager;
    }

    public class CombatGroup
    {
        public Ship[] Ships { get; private set; }
        public bool IsPlayerGroup { get; private set; }

        public CombatGroup(params Ship[] ships)
        {
            Ships = ships;
        }

        public double GetCombatValue()
        {
            double totalValue = 0.0;
            foreach (Ship ship in Ships)
            {
                totalValue += ship.DefenseRating * DEFENSE_VALUE;
                totalValue += ship.FirePower * DAMAGE_VALUE;
                totalValue += ship.JumpRadius * JUMP_RADIUS_VALUE;
            }

            return totalValue;
        }

        const double DEFENSE_VALUE = 1.5;
        const double DAMAGE_VALUE = 2.0;
        const double JUMP_RADIUS_VALUE = 0.01;
    }
}
