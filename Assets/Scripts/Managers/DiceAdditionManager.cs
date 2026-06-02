using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace FoolsBrand
{
    /// <summary>
    /// Handles adding dice to the dice bag, as well as possibly inventory stuff in the future
    /// </summary>
    public class DiceAdditionManager : Manager
    {
        private string[] diceRewards = new string[3];
        [SerializeField] private Transform[] _dicePositions;
        [SerializeField] private GameObject _diceDatabaseReference;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private DiceManager diceManager;
        public void Start()
        {
            if (DiceDatabaseSetup.Instance == null)
            {
                GameObject ddRef = Instantiate(_diceDatabaseReference);
                ddRef.GetComponent<DiceDatabaseSetup>().QuickSetupInstance();
            }

            List<string> validDice = DiceDatabase.AllDiceDict.Keys.ToList();
            for (int i = 0; i < diceRewards.Length; i++)
            {
                diceRewards[i] = validDice[Random.Range(0, validDice.Count)];
                validDice.Remove(diceRewards[i]);

                Instantiate(DiceDatabase.AllDiceDict[diceRewards[i]], _dicePositions[i]);
            }

            StartCoroutine(RotateDice());
        }

        private IEnumerator RotateDice()
        {
            while (true)
            {
                foreach (Transform t in _dicePositions)
                {
                    t.Rotate(Vector3.up, rotationSpeed, Space.World);
                }
                yield return null;
            }
        }

        public void SelectDie(int selectionIndex)
        {
            DiceManager.DiceGoingToCombat.Add(diceRewards[selectionIndex]);
            //foreach(string die in DiceManager.DiceGoingToCombat)
            //{
            //    Debug.Log(die);
            //}

            RunManager.StartNewCombat();
        }
    }
}