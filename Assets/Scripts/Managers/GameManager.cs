using FoolsBrand;
using FoolsBrand.Enemies;
using FoolsBrand.UI;
using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Master game manager. Manages the individual managers
/// </summary>
public class GameManager : HierarchyManager
{
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        Init(this, this);

        GameStart();
    }

    public override void GameStart()
    {
        base.GameStart();

        EnemyManager enemyManager = GetManager<EnemyManager>();
        enemyManager.SpawnRandomEnemy();

        GetManager<CombatManager>().BeginCombat();

        // Debug start.

        GetManager<UIManager>().GetManager<LimbUIManager>().SetDisplays(enemyManager.CurrentEnemy);
    }
}
