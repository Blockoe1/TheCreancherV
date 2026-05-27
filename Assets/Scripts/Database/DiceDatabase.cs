using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Database that stores all of the dice in the game
    /// </summary>
    public static class DiceDatabase
    {
        private static Dictionary<string, GameObject> allDiceDict;

        /// <summary>
        /// Read Only. All of the dice loaded into the game
        /// </summary>
        public static Dictionary<string, GameObject> AllDiceDict { get => allDiceDict; }


        /// <summary>
        /// Setup the dice dictionary. Move this to a static at the beginning of the game
        /// </summary>
        public static void SetupDiceDict(List<GameObject> allDice)
        {
            if (allDiceDict != null)
            {
                return;
            }

            allDiceDict = new Dictionary<string, GameObject>();

            foreach (GameObject die in allDice)
            {
                allDiceDict.Add(die.GetComponent<DieBase>().name, die);
            }
        }
    }
}
