/*****************************************************************************
// File Name : DamageAction.cs
// Author : Lucas Fehlberg
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Buff's the face's value
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "DamageAction", menuName = "Scriptable Objects/Actions/BuffFace")]
    public class BuffFaceAction : DiceAction
    {
        public override int PriorityValue => 1;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            sourceFace.AddValue();
            Debug.Log(value);
            yield return null;
        }
    }
}
