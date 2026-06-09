using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class DiceManager : Manager
    {
        public static DiceManager Instance;

        [SerializeField] private GameObject _diceDatabaseReference;
        [SerializeField] private DiceGridManager diceGrid;
        [SerializeField] private int maxBagSize;

        //Dice bags
        //RIP THIS WHOLE THING OUT AND REPLACE IT.
        [SerializeField] private List<GameObject> _drawBag;
        [SerializeField] private List<GameObject> _discardBag;
        public GameObject _reservedDie = null;
        [SerializeField] private List<GameObject> _rollingDice;
        //private List<GameObject> diceInPlay = new();
        //private GameObject reservedDieGO;

        public static List<string> DiceGoingToCombat = new();
        private readonly List<string> allDice = new();

        [SerializeField] private List<GameObject> _diePositions;
        [SerializeField] private GameObject _reserveSlotPosition;

        private int numDiceHeld;

        //private Dictionary<string, GameObject[]> diceLookup = new();
        public List<GameObject> DiceInPlay 
        {
            get
            {
                //Easily the stupidest thing I've ever done
                List<GameObject> diceInPlay = new List<GameObject>();
                diceInPlay.AddRange(_rollingDice);
                return diceInPlay;
            }
        }

        public List<GameObject> AllDice
        {
            get
            {
                List<GameObject> allDice = new List<GameObject>();
                allDice.AddRange(_rollingDice);
                if (_reservedDie != null)
                {
                    allDice.Add(_reservedDie);
                }
                allDice.AddRange(_discardBag);
                allDice.AddRange(_drawBag);
                return allDice;
            }
        }

        public int NumDiceLeft => _drawBag.Count + _discardBag.Count + _rollingDice.Count + (_reservedDie == null ? 0 : 1);
        public DiceGridManager DiceGrid => diceGrid;

        /// <summary>
        /// Initialize the dice bags
        /// </summary>
        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            Instance = this;

            diceGrid.Init(gm, parentManager);

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

            foreach(string die in DiceGoingToCombat)
            {
                AddDice(die);
            }

            PlayerInputManager.OnReserveInput += ReserveDie;

            ShuffleDeck();
            //StartTurn();
        }

        public override void Deinit()
        {
            PlayerInputManager.OnReserveInput -= ReserveDie;
        }

        ///// <summary>
        ///// Called whenever the next player turn starts
        ///// </summary>
        //public void StartTurn()
        //{
        //    DrawDice();
        //}

        /// <summary>
        /// Dice reservation
        /// </summary>
        /// <param name="index">Index of which die is getting reserved</param>
        public void ReserveDie(int index)
        {
            if(_reservedDie == null)
            {
                //If there's no die reserved, draw a new one
                _reservedDie = _rollingDice[index];
                _reservedDie.transform.position = _reserveSlotPosition.transform.position;
                _reservedDie.transform.localScale = _reserveSlotPosition.transform.localScale;
                _reservedDie.GetComponent<DieBase>().IsReserved = true;

                _rollingDice.RemoveAt(index);

                if (_drawBag.Count <= 0)
                {
                    ShuffleDeck();
                }
                _rollingDice.Insert(index, _drawBag[0]);
                _drawBag.RemoveAt(0);

                //string dice = _rollingDice[index].ToString();

                //for (int j = 0; j < diceLookup[dice].Length; j++)
                //{
                //    if (!diceLookup[dice][j].activeSelf)
                //    {
                //        diceLookup[dice][j].transform.position = _diePositions[index].transform.position;
                //        diceLookup[dice][j].transform.localScale = _diePositions[index].transform.localScale;
                //        diceLookup[dice][j].SetActive(true);
                //        diceLookup[dice][j].GetComponent<DieBase>().StartRolling();
                //        diceInPlay.Insert(index, diceLookup[dice][j]);
                //        break;
                //    }
                //}

                _rollingDice[index].transform.position = _diePositions[index].transform.position;
                _rollingDice[index].transform.localScale = _diePositions[index].transform.localScale;
                DieBase dice = _rollingDice[index].GetComponent<DieBase>();
                diceGrid.CheckOutDice(dice);
                //_rollingDice[index].SetActive(true);
                dice.StartRolling();

                return;
            }

            //If there is a die reserved, swap them
            //GameObject reservation = reservedDieGO;
            //string reservationString = _reservedDie;

            //(reservedDieGO, diceInPlay[index]) = (diceInPlay[index], reservedDieGO);
            (_rollingDice[index], _reservedDie) = (_reservedDie, _rollingDice[index]);

            _reservedDie.transform.position = _reserveSlotPosition.transform.position;
            _reservedDie.transform.localScale = _reserveSlotPosition.transform.localScale;
            _reservedDie.GetComponent<DieBase>().IsReserved = true;

            _rollingDice[index].transform.position = _diePositions[index].transform.position;
            _rollingDice[index].transform.localScale = _diePositions[index].transform.localScale;
            _rollingDice[index].GetComponent<DieBase>().IsReserved = false;
        }

        public void DiscardDice(int index)
        {
            diceGrid.ReturnDice(_rollingDice[index].GetComponent<DieBase>());
            _discardBag.Add(_rollingDice[index]);
            _rollingDice.RemoveAt(index);
        }

        /// <summary>
        /// Clears the reserve slot from play
        /// </summary>
        [Button("Delete Reserve Slot")]
        public void ClearReserveSlot()
        {
            if(_reservedDie == null)
            {
                Debug.Log("No Die in slot");
                return;
            }

            Debug.Log(NumDiceLeft);
            if(NumDiceLeft <= 3)
            {
                Debug.Log("Not enough dice");
                return;
            }

            diceGrid.RemoveDice(_reservedDie.GetComponent<DieBase>());
            _reservedDie.SetActive(false);
            _reservedDie = null;
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
                //string dice = _rollingDice[i].ToString();
                DieBase dice = _rollingDice[i].GetComponent<DieBase>();
                //_rollingDice[i].SetActive(true);
                diceGrid.CheckOutDice(dice);
                _rollingDice[i].transform.position = _diePositions[i].transform.position;
                _rollingDice[i].transform.localScale = _diePositions[i].transform.localScale;
                dice.StartRolling();

                //for (int j = 0; j < diceLookup[dice].Length; j++)
                //{
                //    if (!diceLookup[dice][j].activeSelf)
                //    {
                //        diceLookup[dice][j].transform.position = _diePositions[i].transform.position;
                //        diceLookup[dice][j].transform.localScale = _diePositions[i].transform.localScale;
                //        diceLookup[dice][j].SetActive(true);
                //        diceLookup[dice][j].GetComponent<DieBase>().StartRolling();
                //        diceInPlay.Add(diceLookup[dice][j]);
                //        break;
                //    }
                //}
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

        /// <summary>
        /// Clears all the dice you've gathered this run.
        /// </summary>
        public static void ClearDice()
        {
            DiceGoingToCombat.Clear();
        }

        /// <summary>
        /// Adds a dice to the player's draw bag.
        /// </summary>
        /// <param name="die"></param>
        public void AddDice(string die)
        {
            if (numDiceHeld < maxBagSize)
            {
                GameObject dieObject = Instantiate(DiceDatabase.AllDiceDict[die], transform);
                //dieObject.SetActive(false);
                diceGrid.RegisterDice(dieObject.GetComponent<DieBase>());
                _drawBag.Add(dieObject);
                numDiceHeld++;
                allDice.Add(die);
            }

            //if (!diceLookup.ContainsKey(die))
            //{
            //    diceLookup.Add(die, new GameObject[3]);
            //}

            ////Forgive me for this
            //for (int i = 0; i < diceLookup[die].Length; i++)
            //{
            //    if (diceLookup[die][i] == null)
            //    {
            //        diceLookup[die][i] = Instantiate(DiceDatabase.AllDiceDict[die], transform);
            //        diceLookup[die][i].SetActive(false);
            //        break;
            //    }
            //}
        }

        public int CountDiceNum(string diceName)
        {
            int count = 0;
            foreach(string dice in allDice)
            {
                if(diceName == dice)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
