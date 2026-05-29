/*****************************************************************************
// File Name : PoisonAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Inflicts poison on a poisonable target.
*****************************************************************************/
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class PoisonAction : DiceAction
    {
        [SerializeField] private PoisonEffect poison;
        public override int PriorityValue => 101;

        public override IEnumerator PerformAction(ITargetable target, Combatant user)
        {
            if (target is IEffectable effectable)
            {
                Debug.Log($"{user} poisoned {target} for {value}.");
                effectable.ApplyEffect(poison);
            }
            else
            {
                Debug.Log("Poisoned Failed");
            }
            yield return null;
        }
    }
}
