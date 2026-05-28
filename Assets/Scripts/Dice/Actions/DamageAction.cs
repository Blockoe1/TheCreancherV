/*****************************************************************************
// File Name : DamageAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Deals damage to the target.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class DamageAction : DiceAction
    {
        public override int PriorityValue => 100;

        public override IEnumerator PerformAction(ITargetable target, Combatant user)
        {
            Debug.Log($"{user} attacked {target} for {value}.");
            target.TakeDamage(value, user);
            yield return null;
        }
    }
}
