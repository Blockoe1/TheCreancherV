using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

namespace FoolsBrand
{
    /// <summary>
    /// Handles adding dice to the dice bag, as well as possibly inventory stuff in the future
    /// </summary>
    public class DiceAdditionManager : Manager
    {
        private const int DEFAULT_DICE_SELECTION_NUM = 3;

        private string[] diceRewards = new string[3];
        [SerializeField] private Transform[] _dicePositions;
        [SerializeField] private GameObject _diceDatabaseReference;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private DieSelectionInfo[] _diceSelectionInfoBoxes;
        [SerializeField] private SelectionOverride[] overrides;

        [System.Serializable]
        private struct SelectionOverride
        {
            [SerializeField] private bool use;
            [SerializeField, Range(1, 3), ShowIf(nameof(use)), AllowNesting] private int selectionNum;
            [SerializeField, ShowIf(nameof(use)), AllowNesting] private string[] validDice;
        }

        public void Start()
        {
            if (DiceDatabaseSetup.Instance == null)
            {
                GameObject ddRef = Instantiate(_diceDatabaseReference);
                ddRef.GetComponent<DiceDatabaseSetup>().QuickSetupInstance();

                if (DiceManager.DiceGoingToCombat.Count == 0)
                {
                    foreach (string die in DiceDatabaseSetup.Instance.StartingDice)
                    {
                        DiceManager.DiceGoingToCombat.Add(die);
                    }
                }
            }

            List<string> validDice = DiceDatabase.RewardDice;
            for (int i = 0; i < diceRewards.Length; i++)
            {
                diceRewards[i] = validDice[Random.Range(0, validDice.Count)];
                validDice.Remove(diceRewards[i]);

                GameObject die = Instantiate(DiceDatabase.AllDiceDict[diceRewards[i]], _dicePositions[i]);
                DieBase dieBase = die.GetComponent<DieBase>();
                _diceSelectionInfoBoxes[i].SetupInfo(dieBase.DieName, dieBase.DieDescription);
            }

            StartCoroutine(RotateDice());
        }

        private IEnumerator RotateDice()
        {
            while (true)
            {
                foreach (Transform t in _dicePositions)
                {
                    t.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
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