using FoolsBrand;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all dice. Can also be used as the basic die variant
/// </summary>
public class DieBase : MonoBehaviour
{
    [SerializeField] private string _dieName = "Basic Die 1-6";
    [SerializeField, Tooltip("DO NOT CHANGE THE NUMBER OF FACES. The effects of each face")] private DieFace[] dieFaces = new DieFace[6];

    private int dieIndex = 0;
    private bool corrupted = false;

    private List<DiceAction> actions = new();

    public string DieName { get => _dieName; }

    /// <summary>
    /// The actual rolling of this die
    /// </summary>
    public DiceAction[] RollDie()
    {
        //Don't tell anyone that I'm not going to make the game break if there are more or less faces. Don't do it...maybe
        dieIndex = Random.Range(0, dieFaces.Length);
        return dieFaces[dieIndex].GetActions();
    }

    /// <summary>
    /// Applying the effect of the die to the enemy's limb
    /// </summary>
    public List<DiceAction> ApplyEffect()
    {
        actions.Clear();
        //target = target;
        dieFaces[dieIndex].RollDie.Invoke();
        //Send this information off to the gamemanager
        return actions;
    }

    ///// <summary>
    ///// How much damage the die deals
    ///// </summary>
    ///// <param name="damage"></param>
    //public void FaceDamage(int damage)
    //{
    //    actions.Add(new(Action.ActionTypes.ATTACK, damage));
    //}

    ///// <summary>
    ///// How much poison the die deals
    ///// </summary>
    ///// <param name="poison"></param>
    //public void FacePoison(int poison)
    //{
    //    actions.Add(new(Action.ActionTypes.POSION, poison));
    //}

    ///// <summary>
    ///// How much healing the die deals
    ///// </summary>
    ///// <param name="healing"></param>
    //public void FaceSelfHeal(int healing)
    //{
    //    actions.Add(new(Action.ActionTypes.HEAL, healing));
    //}

    ///// <summary>
    ///// Increase corruption on the die
    ///// </summary>
    //public void FaceCorruption(int corruption)
    //{
    //    actions.Add(new(Action.ActionTypes.CORRUPTION, corruption));
    //}
}
