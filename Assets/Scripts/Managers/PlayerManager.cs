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
        [SerializeField] private float selectLimbWaitTime = 0.25f;
        [SerializeField] private GameObject[] reservationButtons;

        public static HealthData PlayerHealth = null;
        private DiceManager diceManager;
        private LimbUIManager limbUIManager;
        private DiceUIManager diceUI;

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
            diceUI = gm.GetManager<UIManager>().GetManager<DiceUIManager>();

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
                if (die.IsReserved)
                {
                    continue;
                }
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
            StartCoroutine(LoseRoutine());
        }
        private IEnumerator LoseRoutine()
        {
            // Delay a frame to let the death animation begin.
            yield return null;
            float animationDuration = player.Animator.GetAnimationDuration();
            yield return new WaitForSeconds(animationDuration);
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
            //Make reservation buttons appear
            diceUI.ToggleReserveButtons(true);
            diceUI.ToggleRollButton(true);
            limbUIManager.ToggleTargeting(true);

            //TODO - Dice Bonus by not rolling

            //Player rolls dice
            while (targetedLimb == null)
            {
                // When the player rolls, remove ability to reserve.
                if (actionQueue != null)
                {
                    diceUI.ToggleRollButton(false);
                    diceUI.ToggleReserveButtons(false);
                }
                yield return null;
            }

            diceUI.ToggleReserveButtons(false);
            diceUI.ToggleRollButton(false);
            limbUIManager.ToggleTargeting(false);

            // If the player hasn't rolled yet, roll the dice automatically and apply a damage boost.
            if (actionQueue == null)
            {
                PlayerInputManager_OnRollButtonPressed();
                yield return new WaitForSeconds(selectLimbWaitTime);
            }

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
