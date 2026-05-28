using CustomAttributes;
using FoolsBrand;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// The face of a die
/// </summary>
public class DieFace
{
    [SerializeField] private Sprite faceSprite;
    [SerializeReference, ClassDropdown(typeof(DiceAction))] private DiceAction[] faceActions;

    public DiceAction[] GetActions()
    {
        return faceActions;
    }
}
