/*****************************************************************************
// File Name : EnemyUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/28/2026
// Last Modified : 5/28/2026
//
// Brief Description : Manages enemy UI and health bars.
*****************************************************************************/
using CustomAttributes;
using FoolsBrand.Enemies;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EnemyUIManager : Manager
    {
        [SerializeField] private HealthBar enemyHealthBar;
        [SerializeField] private TMP_Text enemyNameText;

        [SerializeField, ShowIfNull] private CanvasGroup enemyGroup;

        private DamageNumberManager dnm;
        private Enemy currentEnemy;

        private void Reset()
        {
            enemyGroup = GetComponent<CanvasGroup>();
        }

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            dnm = parentManager.GetManager<DamageNumberManager>();
            EnemyManager.EnemySpawnEvent += LoadEnemy;
        }

        public override void Deinit()
        {
            EnemyManager.EnemySpawnEvent -= LoadEnemy;
        }

        // Setup the enemy's health bar
        private void LoadEnemy(Enemy enemy)
        {
            if (currentEnemy != null)
            {
                currentEnemy.OnDeathEvent.RemoveListener(OnEnemyDeath);
            }

            currentEnemy = enemy;

            if (currentEnemy != null)
            {
                ToggleEnemyUI(true);
                currentEnemy.OnDeathEvent.AddListener(OnEnemyDeath);
                enemyHealthBar.SetTargetHealth(currentEnemy.Health);
                enemyNameText.text = currentEnemy.EnemyName;
                dnm.RegisterDamageNumber(enemy.Health, enemy.transform);
            }
            else
            {
                ToggleEnemyUI(false);
            }
        }

        public void OnEnemyDeath()
        {
            currentEnemy.OnDeathEvent.RemoveListener(OnEnemyDeath);
            dnm.UnregisterDamageNumber(currentEnemy.Health);
            currentEnemy = null;
            ToggleEnemyUI(false);
        }

        public void ToggleEnemyUI(bool shown)
        {
            enemyGroup.alpha = shown ? 1 : 0;
        }
    }
}
