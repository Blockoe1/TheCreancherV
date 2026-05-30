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
    [SerializeField, Tooltip("If set to true, the top action's value will be shown on the die face.")] 
    private bool appendValueToFace = true;
    [SerializeField] private string faceText;
    [SerializeField] private TMP_Text faceTextObj;
    [SerializeReference, ClassDropdown(typeof(DiceAction))] private DiceAction[] faceActions;

    public DiceAction[] GetActions()
    {
        return faceActions;
    }

    private string GetFaceText()
    {
        return faceText + (appendValueToFace && faceActions.Length > 0 ? faceActions[0].Value : "");
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
