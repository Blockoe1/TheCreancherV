/*****************************************************************************
// File Name : EnemyManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Manages all singleton logic pertaining to enemies, such as spawning.
*****************************************************************************/
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class EnemyManager : Manager
    {
        private const string ENEMY_POS_TAG = "EnemyPos";

        [SerializeField] private Enemy[] encounterableEnemies;
        [SerializeField] private Enemy bossEnemy;

        private readonly List<Enemy> currentEnemies = new List<Enemy>();

        private Transform enemyPos;

        public IReadOnlyList<Enemy> CurrentEnemies
        {
            get
            {
                // Flush dead enemies.
                for(int i = 0; i < currentEnemies.Count; i++)
                {
                    if (currentEnemies[i].IsDead)
                    {
                        currentEnemies.RemoveAt(i);
                        i--;
                    }
                }
                return currentEnemies;
            }
        }

        public override void Init(GameManager gm)
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
            Enemy spawnedEnemy = Instantiate(prefab, enemyPos.transform.position, Quaternion.identity, transform);
            spawnedEnemy.Init();
            currentEnemies.Add(spawnedEnemy);
            return spawnedEnemy;
        }
    }
}
