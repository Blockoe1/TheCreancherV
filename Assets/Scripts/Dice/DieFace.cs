using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// The face of a die
/// </summary>
public class DieFace
{
    [SerializeField, Tooltip("Events can be found in the DieBase class")] private UnityEvent _rollDie;

    /// <summary>
    /// Actions that happen when you roll the die
    /// </summary>
    public UnityEvent RollDie { get => _rollDie; set => _rollDie = value; }
}
