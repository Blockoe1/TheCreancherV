using NaughtyAttributes;
using System.Collections.Generic;
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

        //Dice bags
        //RIP THIS WHOLE THING OUT AND REPLACE IT.
        [SerializeField] private List<GameObject> _drawBag;
        [SerializeField] private List<GameObject> _discardBag;
        public GameObject _reservedDie = null;
        [SerializeField] private List<GameObject> _rollingDice;
        //private List<GameObject> diceInPlay = new();
        //private GameObject reservedDieGO;

        public static List<string> DiceGoingToCombat = new();

        [SerializeField] private List<GameObject> _diePositions;
        [SerializeField] private GameObject _reserveSlotPosition;

        //private Dictionary<string, GameObject[]> diceLookup = new();

        public List<GameObject> DiceInPlay => _rollingDice;
        public int NumDiceLeft => _drawBag.Count + _discardBag.Count + _rollingDice.Count + (_reservedDie == null ? 0 : 1);

        /// <summary>
        /// Initialize the dice bags
        /// </summary>
        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
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

            foreach(string die in DiceGoingToCombat)
            {
                GameObject dieObject = Instantiate(DiceDatabase.AllDiceDict[die], transform);
                dieObject.SetActive(false);
                _drawBag.Add(dieObject);
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
                _rollingDice[index].SetActive(true);
                _rollingDice[index].GetComponent<DieBase>().StartRolling();

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

                _rollingDice[i].SetActive(true);
                _rollingDice[i].transform.position = _diePositions[i].transform.position;
                _rollingDice[i].transform.localScale = _diePositions[i].transform.localScale;
                _rollingDice[i].GetComponent<DieBase>().StartRolling();

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
            GameObject dieObject = Instantiate(DiceDatabase.AllDiceDict[die], transform);
            dieObject.SetActive(false);
            _drawBag.Add(dieObject);

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
    }
}
