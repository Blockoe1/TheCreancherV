using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// The base class for all dice. Can also be used as the basic die variant
/// </summary>
public class DieBase : MonoBehaviour
{
    [SerializeField] private string _dieName = "Basic Die 1-6";
    [SerializeField, Tooltip("DO NOT CHANGE THE NUMBER OF FACES. The effects of each face")] private DieFace[] dieFaces = new DieFace[6];

    private int dieIndex = 0;

    public string DieName { get => _dieName; }

    public ReadOnlyArray<DieFace> Faces => dieFaces;

    /// <summary>
    /// The actual rolling of this die
    /// </summary>
    public DiceAction[] RollDie()
    {
        //Don't tell anyone that I'm not going to make the game break if there are more or less faces. Don't do it...maybe
        dieIndex = Random.Range(0, dieFaces.Length);
        if (!dieFaces[dieIndex].IsInitialized)
        {
            dieFaces[dieIndex].Initialize(this);
        }
        return dieFaces[dieIndex].GetActions();
    }

    /// <summary>
    /// Refreshes face text since subclasses can't trigger OnValidate.
    /// </summary>
    [Button]
    public void RefreshText()
    {
        foreach(var face in dieFaces)
        {
            face.RefreshText();
        }
    }
}
