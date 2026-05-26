using System.Collections.Generic;
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
            for(int i = 0; i < Random.Range(1, 5); i++)
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
