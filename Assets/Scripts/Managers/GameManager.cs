using FoolsBrand;
using FoolsBrand.Enemies;
using System;
using UnityEngine;

/// <summary>
/// Master game manager. Manages the individual managers
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceBag;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CombatManager combatManager;

    public EnemyManager EnemyManager => enemyManager;

    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        diceBag.Init();
        enemyManager.Init(this);
        combatManager.Init(this);
    }

    // Need to move things out of start at some point.
    private void Start()
    {
        enemyManager.SpawnRandomEnemy();

        combatManager.BeginCombat();
    }
}
