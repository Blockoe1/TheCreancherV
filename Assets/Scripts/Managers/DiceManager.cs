using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles dice, rolling, picking up, etc.
/// </summary>
public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [SerializeField] private List<GameObject> _allDice;
    private static Dictionary<string, GameObject> allDiceDict;

    //Dice bags
    [SerializeField] private List<string> _drawBag;
    [SerializeField] private List<string> _discardBag;
    [SerializeField] private string _reservedDie;
    [SerializeField] private List<string> _rollingDice;

    [SerializeField] private List<GameObject> _diePositions;

    private Dictionary<string, GameObject[]> diceLookup = new();

    /// <summary>
    /// Initialize the dice bags
    /// </summary>
    public void Init()
    {
        Instance = this;
        SetupDiceDict();

        //Initialize our dice bag
        //TEMP
        foreach (string die in allDiceDict.Keys)
        {
            for(int i = 0; i < Random.Range(3, 50); i++)
            {
                _drawBag.Add(die);
            }
        }
        //Grab the persistent data list of our dice and put it into the draw bag
        //Initialize the diceLookup - probably should gather a universal lookup table somewhere in a static. We can put that on the main menu
        //But for now, we'll keep that in here

        //Setup the lookup table
        foreach(string die in _drawBag)
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
                    diceLookup[die][i] = Instantiate(allDiceDict[die], transform);
                    diceLookup[die][i].SetActive(false);
                    break;
                }
            }
        }

        ShuffleDeck();
        StartTurn();
    }

    /// <summary>
    /// Called whenever the next player turn starts
    /// </summary>
    public void StartTurn()
    {
        DrawDice();
    }

    /// <summary>
    /// Draws 2 dice from the die bag
    /// </summary>
    public void DrawDice()
    {
        //Actually draw the dice
        for(int i = 0; i < 2; i++)
        {
            if(_drawBag.Count <= 0)
            {
                ShuffleDeck();
            }

            _rollingDice.Add(_drawBag[0]);
            _drawBag.RemoveAt(0);
        }

        //Now make those dice appear
        for(int i = 0; i < _rollingDice.Count; i++)
        {
            string dice = _rollingDice[i].ToString();

            for(int j = 0; j < diceLookup[dice].Length; j++) 
            {
                if (!diceLookup[dice][j].activeSelf)
                {
                    diceLookup[dice][j].transform.position = _diePositions[i].transform.position;
                    diceLookup[dice][j].transform.localScale = _diePositions[i].transform.localScale;
                    diceLookup[dice][j].SetActive(true);
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

    public void ApplyEffects(/*Target*/)
    {

    }

    /// <summary>
    /// Setup the dice dictionary. Move this to a static at the beginning of the game
    /// </summary>
    public void SetupDiceDict()
    {
        if(allDiceDict != null)
        {
            return;
        }

        allDiceDict = new Dictionary<string, GameObject>();

        foreach(GameObject die in _allDice)
        {
            allDiceDict.Add(die.GetComponent<DieBase>().name, die);
        }
    }
}
