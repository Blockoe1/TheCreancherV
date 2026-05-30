using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class DiceManager : Manager
    {
        public static DiceManager Instance;

        [SerializeField] private GameObject _diceDatabaseReference;

        //Dice bags
        [SerializeField] private List<string> _drawBag;
        [SerializeField] private List<string> _discardBag;
        [SerializeField] private string _reservedDie;
        [SerializeField] private List<string> _rollingDice;
        private List<GameObject> diceInPlay = new();

        public static List<string> DiceGoingToCombat = new();

        [SerializeField] private List<GameObject> _diePositions;
        [SerializeField] private GameObject _reserveSlotPosition;

        private Dictionary<string, GameObject[]> diceLookup = new();

        public List<GameObject> DiceInPlay => diceInPlay;

        /// <summary>
        /// Initialize the dice bags
        /// </summary>
        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            diceInPlay.Clear();
            Instance = this;

            if (DiceDatabaseSetup.Instance == null)
            {
                GameObject ddRef = Instantiate(_diceDatabaseReference);
                ddRef.GetComponent<DiceDatabaseSetup>().QuickSetupInstance();
            }

            //Debug Feature
            if(DiceGoingToCombat.Count == 0)
            {
                foreach(string die in DiceDatabaseSetup.Instance.StartingDice)
                {
                    DiceGoingToCombat.Add(die);
                }
            }

            foreach (string die in DiceGoingToCombat)
            {
                _drawBag.Add(die);
            }

            //Grab the persistent data list of our dice and put it into the draw bag
            //Initialize the diceLookup - probably should gather a universal lookup table somewhere in a static. We can put that on the main menu
            //But for now, we'll keep that in here

            //Setup the lookup table
            diceLookup = new();
            foreach (string die in _drawBag)
            {
                if (!diceLookup.ContainsKey(die))
                {
                    diceLookup.Add(die, new GameObject[3]);
                }

                //Forgive me for this
                for (int i = 0; i < diceLookup[die].Length; i++)
                {
                    if (diceLookup[die][i] == null)
                    {
                        diceLookup[die][i] = Instantiate(DiceDatabase.AllDiceDict[die], transform);
                        diceLookup[die][i].SetActive(false);
                        break;
                    }
                }
            }

            ShuffleDeck();
            //StartTurn();
        }

        /// <summary>
        /// Called whenever the next player turn starts
        /// </summary>
        //public void StartTurn()
        //{
        //    DrawDice();
        //}


        public void DiscardDice(int index)
        {
            _discardBag.Add(_rollingDice[index]);
            _rollingDice.RemoveAt(index);
        }
        public void ClearDiceInPlay()
        {
            diceInPlay.Clear();
        }

        /// <summary>
        /// Draws 2 dice from the die bag
        /// </summary>
        public void DrawDice()
        {
            //Actually draw the dice
            for (int i = 0; i < 2; i++)
            {
                if (_drawBag.Count <= 0)
                {
                    ShuffleDeck();
                }

                _rollingDice.Add(_drawBag[0]);
                _drawBag.RemoveAt(0);
            }

            //Now make those dice appear
            for (int i = 0; i < _rollingDice.Count; i++)
            {
                string dice = _rollingDice[i].ToString();

                for (int j = 0; j < diceLookup[dice].Length; j++)
                {
                    if (!diceLookup[dice][j].activeSelf)
                    {
                        diceLookup[dice][j].transform.position = _diePositions[i].transform.position;
                        diceLookup[dice][j].transform.localScale = _diePositions[i].transform.localScale;
                        diceLookup[dice][j].SetActive(true);
                        diceLookup[dice][j].GetComponent<DieBase>().StartRolling();
                        diceInPlay.Add(diceLookup[dice][j]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the dice from the discard pile and shuffles the deck
        /// </summary>
        public void ShuffleDeck()
        {
            _drawBag.AddRange(_discardBag);
            _discardBag.Clear();

            //Shuffle algo
            int index = _drawBag.Count;
            while (index-- > 0)
            {
                int swapPosition = Random.Range(0, _drawBag.Count);
                (_drawBag[swapPosition], _drawBag[index]) = (_drawBag[index], _drawBag[swapPosition]);
            }
        }
    }
}
