using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace FoolsBrand
{
    /// <summary>
    /// Handles adding dice to the dice bag, as well as possibly inventory stuff in the future
    /// </summary>
    public class DiceAdditionManager : Manager
    {
        private string[] diceRewards = new string[3];
        [SerializeField] private TMP_Text[] buttonText;

        [SerializeField] private DiceManager diceManager;
        public void Start()
        {
            diceManager.SetupDiceDictionary();

            List<string> validDice = DiceDatabase.AllDiceDict.Keys.ToList();
            for (int i = 0; i < diceRewards.Length; i++)
            {
                diceRewards[i] = validDice[Random.Range(0, validDice.Count)];
                validDice.Remove(diceRewards[i]);
                buttonText[i].text = diceRewards[i];
            }
        }

        public void SelectDie(int selectionIndex)
        {
            DiceManager.DiceGoingToCombat.Add(diceRewards[selectionIndex]);

            RunManager.StartNewCombat();
        }
    }
}