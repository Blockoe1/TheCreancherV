/*****************************************************************************
// File Name : CorruptionAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Corrupts a number of the user's dice
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class CorruptionAction : DiceAction
    {
        public override int PriorityValue => 0;

        public override IEnumerator PerformAction(ITargetable target,  Combatant user)
        {
            Debug.Log($"{user} corrupted {value} of their dice.");
            yield return null;
        }
    }
}
