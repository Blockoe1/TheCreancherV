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
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Awake()
    {
        mainMenuAction.Enable();
        mainMenuAction.performed += MainMenuAction_performed;


        Init(this, this);

        GameStart();
    }

    private void MainMenuAction_performed(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    public override void GameStart()
    {
        base.GameStart();

        EnemyManager enemyManager = GetManager<EnemyManager>();
        enemyManager.SpawnRandomEnemy();

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
