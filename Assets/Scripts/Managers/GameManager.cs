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
    [SerializeField] private InputAction mainMenuAction;
    [SerializeField, Tooltip("Manually sets the current encounter when this manager inits.  Set to -1 to ignore.")]
    private int debugStartEncounter = -1;
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        mainMenuAction.Enable();
        mainMenuAction.performed += MainMenuAction_performed;

        if (debugStartEncounter >= 0)
        {
            RunManager.CurrentEncounterNum = debugStartEncounter;
        }
        Init(this, this);

        GameStart();
    }

    private void MainMenuAction_performed(InputAction.CallbackContext obj)
    {
        RunManager.CombatLose();
    }

    public override void GameStart()
    {
        base.GameStart();

        EnemyManager enemyManager = GetManager<EnemyManager>();
        enemyManager.SpawnNextEnemy();

        GetManager<CombatManager>().BeginCombat();

        // Debug start.

        //GetManager<UIManager>().GetManager<LimbUIManager>().SetDisplays(enemyManager.CurrentEnemy);
    } 

    private void OnDestroy()
    {
        mainMenuAction.performed -= MainMenuAction_performed;
        Deinit();
    }
}
