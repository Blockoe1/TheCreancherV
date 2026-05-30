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

    public int Value
    {
        get => value;
        set => this.value = value;
    }

    //public enum ActionTypes
    //{
    //    ATTACK,
    //    HEAL,
    //    POSION,
    //    CORRUPTION
    //}
    //public Action(ActionTypes type, int value)
    //{
    //    Type = type;
    //    Value = value;
    //}

    //public ActionTypes Type;
    //public int Value;
}
