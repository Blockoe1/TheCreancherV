using FoolsBrand.Enemies;
using FoolsBrand.UI;
using System;
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
        [SerializeField] private float playerDeathDelay;

        private DiceManager diceManager;
        private LimbUIManager limbUIManager;
        private DiceUIManager diceUI;

        private int? targetedLimb = null;
        //private DiceAction[] diceActions = null;
        private MinPriorityQueue<DiceActionInfo> actionQueue;

        public PlayerCombatant Player => player;

        public bool IsDead => player.Health.IsDead;

        public static event Action<DieBase> DiceRolledEvent;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            if (RunManager.PlayerHealth == -1)
            {
                RunManager.PlayerHealth = player.Health.Value;
            }
            else
            {
                player.Health.Value = RunManager.PlayerHealth;
            }
            diceManager = gm.GetManager<DiceManager>();
            limbUIManager = gm.GetManager<UIManager>().GetManager<LimbUIManager>();
            diceUI = gm.GetManager<UIManager>().GetManager<DiceUIManager>();

            PlayerInputManager.OnLimbSelectedInput += PlayerInputManager_OnLimbSelectedInput;
            PlayerInputManager.OnRollButtonPressed += PlayerInputManager_OnRollButtonPressed;

            player.OnDeathEvent.AddListener(PlayerDead);
            player.Health.HealthChangedEvent += UpdateSavedPlayerHealth;

            player.Init();
        }
        public override void Deinit()
        {
            PlayerInputManager.OnLimbSelectedInput -= PlayerInputManager_OnLimbSelectedInput;
            PlayerInputManager.OnRollButtonPressed -= PlayerInputManager_OnRollButtonPressed;
            player.Health.HealthChangedEvent -= UpdateSavedPlayerHealth;
        }

        /// <summary>
        /// Updates the saved player health for the run.
        /// </summary>
        /// <param name="healthChange"></param>
        private void UpdateSavedPlayerHealth(int healthChange)
        {
            RunManager.PlayerHealth = player.Health.Value;
        }

        private void PlayerInputManager_OnRollButtonPressed()
        {
            actionQueue = new MinPriorityQueue<DiceActionInfo>();
            foreach (GameObject dice in diceManager.DiceInPlay)
            {
                DieBase die = dice.GetComponent<DieBase>();
                if (die.IsReserved)
                {
                    continue;
                }
                
                DiceActionInfo[] actions = die.RollDie();
                DiceRolledEvent?.Invoke(die);
                foreach (DiceActionInfo actionInfo in actions)
                {
                    actionQueue.Enqueue(actionInfo, actionInfo.Action.PriorityValue);
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
            yield return new WaitForSeconds(playerDeathDelay);
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
            diceUI.SetCanReserve(true);
            diceUI.ToggleRollButton(true);

            //TODO - Dice Bonus by not rolling
            yield return new WaitUntil(() => actionQueue != null);
            diceUI.ToggleRollButton(false);
            diceUI.SetCanReserve(false);
            limbUIManager.ToggleTargeting(true);
            //Player rolls dice
            yield return new WaitUntil(() => targetedLimb != null);
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
                diceManager.DiscardDice(0);
            }

            yield return null;
        }
    }
}
