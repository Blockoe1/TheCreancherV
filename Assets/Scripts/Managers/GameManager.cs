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
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        diceBag.Init();
        enemyManager.Init();
    }

    private void Start()
    {
        enemyManager.SpawnRandomEnemy();
    }
}
