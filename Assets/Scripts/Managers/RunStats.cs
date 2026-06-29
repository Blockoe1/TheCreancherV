/*****************************************************************************
// File Name : RunStats.cs
// Author : Arcadia Koederitz
// Creation Date : 6/28/2026
// Last Modified : 6/28/2026
//
// Brief Description : Controls tracking stats about a run.
*****************************************************************************/
using FoolsBrand.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoolsBrand
{
    public class RunStats
    {
        private static readonly string[] IGNORED_DICE = new string[] { "Basic Die" };

        private int totalDice;
        private int enemiesKilled;
        private string lastEnemy;
        private readonly Dictionary<string, int> totalDiceRolled = new Dictionary<string, int>();

        private Enemy currentEnemy;

        public int EnemiesKilled => enemiesKilled;

        public string LastEnemy => lastEnemy;

        public int TotalDice => totalDice;

        public string FavoredDice
        {
            get
            {
                string mostString = "";
                int mostRolled = 0;
                foreach(var dice in totalDiceRolled.Keys)
                {
                    if (totalDiceRolled[dice] > mostRolled)
                    {
                        mostString = dice;
                        mostRolled = totalDiceRolled[dice];
                    }
                }
                return mostString;
            }
        }

        public RunStats()
        {
            EnemyManager.EnemySpawnEvent += HandleEnemySpawned;
            DieBase.DiceRolledEvent += HandleDiceRoll;
        }

        public void CleanUp()
        {
            EnemyManager.EnemySpawnEvent -= HandleEnemySpawned;
            DieBase.DiceRolledEvent -= HandleDiceRoll;
        }

        /// <summary>
        /// Log the amount of dice the player has at the start of combat.
        /// </summary>
        public void OnNewCombat()
        {
            totalDice = DiceManager.DiceGoingToCombat.Count;
        }

        private void HandleEnemySpawned(Enemy enemy)
        {
            if (currentEnemy != null)
            {
                currentEnemy.OnDeathEvent.RemoveListener(HandleEnemyDeath);
            }

            lastEnemy = enemy.EnemyName;
            currentEnemy = enemy;
            enemy.OnDeathEvent.AddListener(HandleEnemyDeath);
        }

        private void HandleEnemyDeath()
        {
            enemiesKilled++;
            currentEnemy.OnDeathEvent.RemoveListener(HandleEnemyDeath);
            currentEnemy = null;
        }

        private void HandleDiceRoll(DieBase obj)
        {
            if(totalDiceRolled.ContainsKey(obj.DieName))
            {
                totalDiceRolled[obj.DieName]++;
            }
            else if (!IGNORED_DICE.Contains(obj.DieName))
            {
                // Ignored dice get tracked once so they can show up if no other dice are used, but they don't tick up.
                totalDiceRolled.Add(obj.DieName, 1);
            }
        }
    }
}
