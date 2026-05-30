using CustomAttributes;
using FoolsBrand;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Stores types of actions
/// </summary>
[System.Serializable]
public abstract class DiceAction
{
    [SerializeField] protected int value;

    public abstract int PriorityValue { get; }
    public abstract IEnumerator PerformAction(ITargetable target, Combatant user);

    protected DieFace parentFace;

    public int Value
    {
        get => value;
        set => this.value = value;
    }

    /// <summary>
    /// Initializes this action with a reference to the owned face.
    /// </summary>
    public void Initialize(DieFace face)
    {
        parentFace = face;
    }
}
