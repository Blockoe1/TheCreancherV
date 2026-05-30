using CustomAttributes;
using FoolsBrand;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// The face of a die
/// </summary>
public class DieFace
{
    private const string VALUE_CHAR = "#";

    [SerializeField] private string faceText = "#";
    [SerializeField] private TMP_Text faceTextObj;
    [SerializeReference, ClassDropdown(typeof(DiceAction))] private DiceAction[] faceActions;

    private DieBase parentDice;

    public DieBase ParentDice => parentDice;

    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Initializes this face with a reference to the parent dice and initializes all actions with a reference to 
    /// this face.
    /// </summary>
    /// <param name="dieBase"></param>
    public void Initialize(DieBase dieBase)
    {
        parentDice = dieBase;
        foreach (var action in faceActions)
        {
            action.Initialize(this);
        }
    }

    public DiceAction[] GetActions()
    {
        return faceActions;
    }

    private string GetFaceText()
    {
        return faceText.Replace(VALUE_CHAR, (faceActions.Length > 0 ? faceActions[0].Value.ToString() : ""));
    }

    /// <summary>
    /// Refreshes the text displayed on the dice model's face.
    /// </summary>
    public void RefreshText()
    {
        if (faceTextObj == null) { return; }
        faceTextObj.text = GetFaceText();
    }
}
