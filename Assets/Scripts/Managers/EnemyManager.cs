/*****************************************************************************
// File Name : EnemyManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Manages all singleton logic pertaining to enemies, such as spawning.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        private const string ENEMY_POS_TAG = "EnemyPos";

        [SerializeField] private Enemy[] encounterableEnemies;
        [SerializeField] private Enemy bossEnemy;

        private Transform enemyPos;

        public void Init()
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
            return spawnedEnemy;
        }
    }
}
