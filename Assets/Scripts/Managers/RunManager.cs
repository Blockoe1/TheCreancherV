using FoolsBrand.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    public static class RunManager
    {
        private const int BOSS_ENCOUNTER = 8;

        private static readonly Color WIN_FADE_COLOR = Color.black;//new Color();
        private static readonly Color LOSE_FADE_COLOR = Color.black;

        private static bool win = false;
        public static int CurrentEncounterNum { get; internal set; }
        public static int PlayerHealth { get; set; } = -1;
        public static bool Win { get => win; set => win = value; }

        /// <summary>
        /// Called when a new combat is started. Switches the scene and selects the new enemy
        /// </summary>
        public static void StartNewCombat()
        {
            //Debug.Log("Combat Start");
            TransitionManager.LoadScene("MainCombat");
            if(CurrentEncounterNum == BOSS_ENCOUNTER)
            {
                AudioManager.Instance.SetMusic(MusicType.BossMusic);
            }
            else
            {
                AudioManager.Instance.SetMusic(MusicType.InCombat);
            }
        }

        /// <summary>
        /// Called when a combat is won, take the player to new dice selection
        /// </summary>
        public static void CombatWin()
        {
            //Debug.Log("Combat Win");
            CurrentEncounterNum++;
            TransitionManager.LoadScene("OutOfCombat");
            AudioManager.Instance.SetMusic(MusicType.OutOfCombat);
        }

        /// <summary>
        /// Called once all enemies in the EnemyManager are defeated to give the player a win screen.
        /// </summary>
        public static void WinRun()
        {
            // TODO: Implement winning.
            win = true;
            CleanUpRun();
            TransitionManager.LoadScene("EndScreen", WIN_FADE_COLOR, TransitionType.Fade);
        }

        /// <summary>
        /// Called when the combat is lost, take the player to the main menu
        /// </summary>
        public static void CombatLose()
        {
            //Debug.Log("Combat Lose");
            win = false;
            CleanUpRun();
            TransitionManager.LoadScene("EndScreen", LOSE_FADE_COLOR, TransitionType.Fade);
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
        public static void BeginNewGame(List<string> startingDice, int startingEncounter)
        {
            CurrentEncounterNum = startingEncounter;
            foreach (string die in startingDice)
            {
                DiceManager.DiceGoingToCombat.Add(die);
            }
            StartNewCombat();
        }
    }
}
