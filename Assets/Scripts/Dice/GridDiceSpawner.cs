/*****************************************************************************
// File Name : GridDiceSpawner.cs
// Author : Arcadia Koederitz
// Creation Date : 6/9/2026
// Last Modified : 6/9/2026
//
// Brief Description : Spawns all dice the player has on a dice grid during the out of combat scene.
*****************************************************************************/
using System;
using System.Collections.Specialized;
using UnityEngine;

namespace FoolsBrand
{
    public class GridDiceSpawner : MonoBehaviour
    {
        [SerializeField] private DiceGrid diceGrid;
        private void Start()
        {
            diceGrid.Init(null, null);

            // Spawn each dice in the player's bag and add them to the grid.
            foreach(string dice in DiceManager.DiceGoingToCombat)
            {
                SpawnDice(dice);
            }

            diceGrid.ToggleCamera(true);
        }

        private void SpawnDice(string die)
        {
            GameObject dieObject = Instantiate(DiceDatabase.AllDiceDict[die], transform);
            //dieObject.SetActive(false);
            diceGrid.RegisterDice(dieObject.GetComponent<DieBase>());
        }
    }
}
