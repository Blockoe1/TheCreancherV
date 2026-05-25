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

    private Dictionary<string, GameObject[]> diceLookup;

    /// <summary>
    /// Initialize the dice bags
    /// </summary>
    public void Init()
    {
        Instance = this;
        SetupDiceDict();

        //Initialize our dice bag

        //Grab the persistent data list of our dice and put it into the draw bag
        //Initialize the diceLookup - probably should gather a universal lookup table somewhere in a static. We can put that on the main menu
        //But for now, we'll keep that in here
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
