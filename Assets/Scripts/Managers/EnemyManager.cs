/*****************************************************************************
// File Name : EnemyManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Manages all singleton logic pertaining to enemies, such as spawning.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class EnemyManager : Manager
    {
        private const string ENEMY_POS_TAG = "EnemyPos";

        [SerializeField] private Enemy[] encounterableEnemies;
        [SerializeField] private Enemy bossEnemy;

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

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            enemyPos = GameObject.FindGameObjectWithTag(ENEMY_POS_TAG).transform;
        }

        /// <summary>
        /// Spawns a random enemy from the encounter enemies.
        /// </summary>
        public void SpawnRandomEnemy()
        {
            SpawnEnemy(encounterableEnemies[UnityEngine.Random.Range(0, encounterableEnemies.Length)]);
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

            Enemy spawnedEnemy = Instantiate(prefab, enemyPos.transform.position, Quaternion.identity, transform);
            spawnedEnemy.Init();
            currentEnemy = spawnedEnemy;
            return spawnedEnemy;
        }
    }
}
