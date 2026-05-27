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

        private CombatState state;

        private enum CombatState
        {
            Starting,
            PlayerAction,
            EnemyAction,
            Victory,
            Defeat
        }

        public override void Init(GameManager gm)
        {
            enemyManager = gm.EnemyManager;
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

                yield return null; // TODO: Add a player combatant.

                if (CheckCombatState())
                {
                    break;
                }

                // Enemies take action.
                state = CombatState.EnemyAction;

                foreach(Enemy enemy in enemyManager.CurrentEnemies)
                {
                    yield return StartCoroutine(enemy.Act());
                }

                if (CheckCombatState())
                {
                    break;
                }

                yield return new WaitForSeconds(debugWait);
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
            if (false) // TODO: Add check for player death.
            {
                state = CombatState.Defeat;
                return true;
            }


            // Check for victory.
            if (enemyManager.CurrentEnemies.Count == 0)
            {
                state = CombatState.Victory;
                return true;
            }

            return false;
        }
    }
}
