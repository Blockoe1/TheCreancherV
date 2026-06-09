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
    [SerializeField] private int faceValue;
    [SerializeField] private TMP_Text faceTextObj;
    [SerializeField] private DiceAction[] faceActions;

    private DieBase parentDice;

    public DieBase ParentDice => parentDice;
    public int FaceValue => faceValue;

    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Initializes this face with a reference to the parent dice and initializes all actions with a reference to 
    /// this face.
    /// </summary>
    /// <param name="dieBase"></param>
    public void Initialize(DieBase dieBase)
    {
        parentDice = dieBase;
    }

    public DiceActionInfo[] GetActions()
    {
        DiceActionInfo[] actionInfo = new DiceActionInfo[faceActions.Length];
        for(int i = 0; i < faceActions.Length; i++)
        {
            actionInfo[i] = new DiceActionInfo(this, faceActions[i]);
        }
        return actionInfo;
    }

    public string GetFaceText()
    {
        return faceText.Replace(VALUE_CHAR, (faceActions.Length > 0 ? faceValue.ToString() : ""));
    }

    /// <summary>
    /// Refreshes the text displayed on the dice model's face.
    /// </summary>
    public void RefreshText()
    {
        if (faceTextObj == null) { return; }
        faceTextObj.text = GetFaceText();
    }

    public void IncreaseValue(int valueIncrease = 1)
    {
        faceValue += valueIncrease;
        RefreshText();
    }
}
