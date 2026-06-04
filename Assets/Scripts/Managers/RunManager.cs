using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public static class RunManager
    {
        public static int CurrentEncounterNum { get; private set; }
        public static int PlayerHealth { get; set; } = -1;

        /// <summary>
        /// Called when a new combat is started. Switches the scene and selects the new enemy
        /// </summary>
        public static void StartNewCombat()
        {
            //Debug.Log("Combat Start");
            SceneManager.LoadScene("MainCombat");
        }

        /// <summary>
        /// Called when a combat is won, take the player to new dice selection
        /// </summary>
        public static void CombatWin()
        {
            //Debug.Log("Combat Win");
            CurrentEncounterNum++;
            SceneManager.LoadScene("OutOfCombat");
        }

        /// <summary>
        /// Called once all enemies in the EnemyManager are defeated to give the player a win screen.
        /// </summary>
        public static void WinRun()
        {
            // TODO: Implement winning.
            Debug.Log("Win!");
            CleanUpRun();
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Called when the combat is lost, take the player to the main menu
        /// </summary>
        public static void CombatLose()
        {
            //Debug.Log("Combat Lose");
            CleanUpRun();
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Called whenever a run is ended, win or lose.
        /// </summary>
        private static void CleanUpRun()
        {
            PlayerHealth = -1;
            CurrentEncounterNum = 0;
            DiceManager.ClearDice();
        }

        /// <summary>
        /// Called when the game is started from the main menu
        /// </summary>
        /// <param name="startingDice"></param>
        public static void BeginNewGame(List<string> startingDice)
        {
            CurrentEncounterNum = 0;
            foreach (string die in startingDice)
            {
                DiceManager.DiceGoingToCombat.Add(die);
            }
            StartNewCombat();
        }
    }
}
