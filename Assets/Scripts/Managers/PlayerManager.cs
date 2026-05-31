using FoolsBrand.Enemies;
using FoolsBrand.UI;
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
        private LimbUIManager limbUIManager;

        private int? targetedLimb = null;
        //private DiceAction[] diceActions = null;
        private MinPriorityQueue<DiceAction> actionQueue;

        public PlayerCombatant Player => player;

        public bool IsDead => player.Health.IsDead;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            PlayerHealth ??= player.Health;
            diceManager = gm.GetManager<DiceManager>();
            limbUIManager = gm.GetManager<UIManager>().GetManager<LimbUIManager>();

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

            //Player turn start
            //Player draws dice
            targetedLimb = null;
            actionQueue = null;
            diceManager.DrawDice();

            limbUIManager.ToggleTargeting(true);

            //TODO - Dice Reservation

            //TODO - Dice Bonus by not rolling

            //Player rolls dice
            yield return new WaitUntil(() => actionQueue != null);
            //Player selects part
            yield return new WaitUntil(() => targetedLimb != null);
            //Actions
            //Dice get Discarded
            //Player End Turn
            limbUIManager.ToggleTargeting(false);

            player.SetActData(actionQueue, enemyTarget.Limbs[(int)targetedLimb]);
            yield return StartCoroutine(player.Act(enemyTarget));

            foreach (GameObject dice in diceManager.DiceInPlay)
            {
                dice.SetActive(false);
                diceManager.DiscardDice(0);
            }
            diceManager.ClearDiceInPlay();

            yield return null;
        }
    }
}
