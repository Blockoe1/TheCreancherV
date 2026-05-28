/*****************************************************************************
// File Name : CombatManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls the progression of combat.
*****************************************************************************/
using FoolsBrand.Enemies;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class CombatManager : Manager
    {
        [SerializeField] private float debugWait;
        private EnemyManager enemyManager;
        private PlayerManager playerManager;

        private CombatState state;

        private enum CombatState
        {
            Starting,
            PlayerAction,
            EnemyAction,
            Victory,
            Defeat
        }

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            enemyManager = gm.GetManager<EnemyManager>();
            playerManager = gm.GetManager<PlayerManager>();
        }

        public void BeginCombat()
        {
            StartCoroutine(CombatLoop());
        }

        public IEnumerator CombatLoop()
        {
            // Start combat.
            state = CombatState.Starting;

            while(true)
            {
                // Player takes action
                state = CombatState.PlayerAction;

                yield return StartCoroutine(playerManager.Act(enemyManager.CurrentEnemy));

                if (CheckCombatState())
                {
                    break;
                }

                // Enemies take action.
                state = CombatState.EnemyAction;

                yield return StartCoroutine(enemyManager.CurrentEnemy.Act(playerManager.Player));

                if (CheckCombatState())
                {
                    break;
                }

                //yield return new WaitForSeconds(debugWait);
            }
            if (state == CombatState.Victory)
            {
                // Execute victory logic and move to dice selection.
            }
            else if (state == CombatState.Defeat)
            {
                // Broadcast to the GameManager to end the run.
            }
            else
            {
                Debug.LogError("Combat has ended, but the current combat state is set to " + state);
            }
        }

        // Returns true if combat has ended.
        private bool CheckCombatState()
        {
            // Check for death.
            if (playerManager.IsDead)
            {
                state = CombatState.Defeat;
                return true;
            }

            // Check for victory.
            if (enemyManager.CurrentEnemy == null)
            {
                state = CombatState.Victory;
                return true;
            }

            return false;
        }
    }
}
