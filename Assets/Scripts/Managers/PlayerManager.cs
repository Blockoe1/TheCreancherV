using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class PlayerManager : Manager
    {
        [SerializeField] private PlayerCombatant player;
        [SerializeField] private HealthData playerHealth = new();

        public static HealthData PlayerHealth = null;
        private DiceManager diceManager;

        public PlayerCombatant Player => player;

        public bool IsDead => player.Health.IsDead;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            PlayerHealth ??= playerHealth;
            diceManager = gm.GetManager<DiceManager>();
        }

        public IEnumerator Act(Combatant target)
        {
            diceManager.DrawDice();

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
                yield return StartCoroutine(action.PerformAction(target, player));
            }
            yield return null;
        }
    }
}
