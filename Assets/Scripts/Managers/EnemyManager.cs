/*****************************************************************************
// File Name : EnemyManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Manages all singleton logic pertaining to enemies, such as spawning.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class EnemyManager : Manager
    {
        private const string ENEMY_POS_TAG = "EnemyPos";

        [SerializeField] private Enemy[] encounterableEnemies;

        private Enemy currentEnemy;
        public Enemy CurrentEnemy
        {
            get
            {
                if (currentEnemy.IsDead)
                {
                    currentEnemy = null;
                }

                return currentEnemy;
            }
        }

        private Transform enemyPos;

        public static event Action<Enemy> EnemySpawnEvent;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            enemyPos = GameObject.FindGameObjectWithTag(ENEMY_POS_TAG).transform;
        }

        /// <summary>
        /// Runs when the enemy dies
        /// </summary>
        private void EnemyDead()
        {
            StartCoroutine(DeathDelayRoutine());
        }
        /// <summary>
        /// Delays advancing the run manager until the enemy plays it's death animation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeathDelayRoutine()
        {
            // Delay a frame to let the death animation begin.
            yield return null;
            float animationDuration = currentEnemy.Animator.GetAnimationDuration();
            yield return new WaitForSeconds(animationDuration);
            if (RunManager.CurrentEncounterNum == encounterableEnemies.Length - 1)
            {
                // If this was the last encounter, win the game.
                RunManager.WinRun();
            }
            else
            {
                RunManager.CombatWin();
            }
            
        }

        /// <summary>
        /// Spawns a random enemy from the encounter enemies.
        /// </summary>
        public void SpawnNextEnemy()
        {
            SpawnEnemy(encounterableEnemies[RunManager.CurrentEncounterNum]);
        }

        /// <summary>
        /// Spawns an enemy from a prefab and properly initializes it.
        /// </summary>
        /// <param name="prefab">The prefab enemy to spawn.</param>
        /// <returns></returns>
        public Enemy SpawnEnemy(Enemy prefab)
        {
            if (currentEnemy != null)
            {
                Debug.LogWarning("Enemy was spawned before th eprevious enemy was destroyed.");
            }

            Enemy spawnedEnemy = Instantiate(prefab, enemyPos.transform.position, enemyPos.transform.rotation);
            spawnedEnemy.transform.SetParent(transform, true);
            spawnedEnemy.Init();
            spawnedEnemy.gameObject.name = spawnedEnemy.gameObject.name.Replace("(Clone)", "");
            currentEnemy = spawnedEnemy;
            EnemySpawnEvent?.Invoke(spawnedEnemy);
            currentEnemy.OnDeathEvent.AddListener(EnemyDead);
            return spawnedEnemy;
        }
    }
}
