using FoolsBrand.Enemies;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class PlayerManager : Manager
    {
        [SerializeField] private PlayerCombatant player;

        public static HealthData PlayerHealth = null;
        private DiceManager diceManager;

        private int? targetedLimb = null;
        //private DiceAction[] diceActions = null;
        private MinPriorityQueue<DiceAction> actionQueue;

        public PlayerCombatant Player => player;

        public bool IsDead => player.Health.IsDead;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            PlayerHealth ??= player.Health;
            diceManager = gm.GetManager<DiceManager>();

            PlayerInputManager.OnLimbSelectedInput += PlayerInputManager_OnLimbSelectedInput;
            PlayerInputManager.OnRollButtonPressed += PlayerInputManager_OnRollButtonPressed;

            player.OnDeathEvent.AddListener(PlayerDead);
        }

        private void OnDestroy()
        {
            PlayerInputManager.OnLimbSelectedInput -= PlayerInputManager_OnLimbSelectedInput;
            PlayerInputManager.OnRollButtonPressed -= PlayerInputManager_OnRollButtonPressed;
        }

        private void PlayerInputManager_OnRollButtonPressed()
        {
            actionQueue = new MinPriorityQueue<DiceAction>();
            foreach (GameObject dice in diceManager.DiceInPlay)
            {
                Debug.Log(dice);
                DieBase die = dice.GetComponent<DieBase>();
                DiceAction[] actions = die.RollDie();
                foreach (DiceAction action in actions)
                {
                    actionQueue.Enqueue(action, action.PriorityValue);
                }
            }
        }

        /// <summary>
        /// Runs when the player dies
        /// </summary>
        private void PlayerDead()
        {
            RunManager.CombatLose();
        }

        private void PlayerInputManager_OnLimbSelectedInput(int limbIndex)
        {
            targetedLimb = limbIndex;
        }

        public IEnumerator Act(Combatant target)
        {
            Enemy enemyTarget = target as Enemy;

            Debug.Log("Turn Start");
            //Player turn start
            //Player draws dice
            targetedLimb = null;
            actionQueue = null;
            diceManager.DrawDice();

            //TODO - Dice Reservation

            //TODO - Dice Bonus by not rolling

            //Player rolls dice
            yield return new WaitUntil(() => actionQueue != null);
            Debug.Log("Dice Rolled");
            //Player selects part
            yield return new WaitUntil(() => targetedLimb != null);
            Debug.Log("Limb Targeted");
            //Actions
            //Dice get Discarded
            //Player End Turn


            while (actionQueue.Count > 0)
            {
                // Switch this to inheritance support later.
                DiceAction action = actionQueue.Dequeue();
                yield return StartCoroutine(action.PerformAction(enemyTarget.Limbs[(int)targetedLimb], player));
            }
            Debug.Log("Actions Taken");

            foreach (GameObject dice in diceManager.DiceInPlay)
            {
                dice.SetActive(false);
                diceManager.DiscardDice(0);
            }
            diceManager.ClearDiceInPlay();
            Debug.Log("Dice Discarded");

            yield return null;
        }
    }
}
