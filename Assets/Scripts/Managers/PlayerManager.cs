using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class PlayerManager : Manager
    {
        [SerializeField] private HealthStruct playerHealth = new();

        public static HealthStruct? PlayerHealth = null;
        private DiceManager diceManager;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            PlayerHealth ??= playerHealth;
            gm.GetManager<DiceManager>();
        }

        public void Act()
        {
            List<DiceAction> allActions = new();
            MinPriorityQueue<DiceAction> actionQueue = new MinPriorityQueue<DiceAction>();
            foreach (GameObject dice in diceManager.DiceInPlay)
            {
                DieBase die = dice.GetComponent<DieBase>();
                DiceAction[] actions = die.RollDie();
                foreach (DiceAction action in actions)
                {
                    actionQueue.Enqueue(action, action.PriorityValue);
                }
                //allActions.AddRange(die.ApplyEffect());
                dice.SetActive(false);


                diceManager.DiscardDice(0);
            }

            diceManager.ClearDiceInPlay();

            while (actionQueue.Count > 0)
            {
                // Switch this to inheritance support later.
                DiceAction action = actionQueue.Dequeue();
                action.PerformAction(default, null);
            }
        }
    }
}
