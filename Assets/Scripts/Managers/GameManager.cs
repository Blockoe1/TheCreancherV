using FoolsBrand;
using FoolsBrand.Enemies;
using FoolsBrand.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Master game manager. Manages the individual managers
/// </summary>
public class GameManager : HierarchyManager
{
    [SerializeField, Tooltip("Manually sets the current encounter when this manager inits.  Set to -1 to ignore.")]
    private int debugStartEncounter = -1;
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        if (debugStartEncounter >= 0)
        {
            RunManager.CurrentEncounterNum = debugStartEncounter;
        }
        Init(this, this);

        GameStart();
    }

    public override void GameStart()
    {
        base.GameStart();

        EnemyManager enemyManager = GetManager<EnemyManager>();
        enemyManager.SpawnNextEnemy();

        GetManager<CombatManager>().BeginCombat();
    } 

    private void OnDestroy()
    {
        Deinit();
    }
}
