using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public static class RunManager
    {
        /// <summary>
        /// Called when a new combat is started. Switches the scene and selects the new enemy
        /// </summary>
        public static void StartNewCombat()
        {
            Debug.Log("Combat Start");
            SceneManager.LoadScene("MainCombat");
        }

        /// <summary>
        /// Called when a combat is won, take the player to new dice selection
        /// </summary>
        public static void CombatWin()
        {
            Debug.Log("Combat Win");
        }

        /// <summary>
        /// Called when the combat is lost, take the player to the main menu
        /// </summary>
        public static void CombatLose()
        {
            Debug.Log("Combat Lose");
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Called when the game is started from the main menu
        /// </summary>
        /// <param name="startingDice"></param>
        public static void BeginNewGame(List<string> startingDice)
        {
            DiceManager.DiceGoingToCombat = startingDice;
            StartNewCombat();
        }
    }
}
