using FoolsBrand;
using FoolsBrand.Enemies;
using FoolsBrand.UI;
using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Master game manager. Manages the individual managers
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private Manager[] managers;

    [SerializeField] private DiceManager diceBag;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private UIManager uiManager;

    public EnemyManager EnemyManager => enemyManager;

    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        foreach(Manager manager in managers)
        {
            manager.Init(this);
        }

        foreach(Manager manager in managers)
        {
            manager.GameStart();
        }
        enemyManager.Init(this);
        combatManager.Init(this);

        uiManager.Init(this);
    }
    
    /// <summary>
    /// Gets a manager object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetManager<T>() where T: Manager
    {
        return (T)Array.Find(managers, x => x is T);
    }

    // Need to move things out of start at some point.
    private void Start()
    {
        enemyManager.SpawnRandomEnemy();

        combatManager.BeginCombat();

        // Debug start.

        GetComponentInChildren<LimbUIManager>().SetDisplays(enemyManager.CurrentEnemy);
    }
}
