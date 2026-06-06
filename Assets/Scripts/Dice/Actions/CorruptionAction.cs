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
    [CreateAssetMenu(fileName = "CorruptionAction", menuName = "Scriptable Objects/Actions/Corruption")]
    public class CorruptionAction : DiceAction
    {
        public override int PriorityValue => 0;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            Debug.Log($"{user} corrupted {value} of their dice.");
            yield return null;
        }
    }
}
