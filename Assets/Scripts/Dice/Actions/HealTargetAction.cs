/*****************************************************************************
// File Name : HealTargetAction.cs
// Author : Arcadia Koederitz
// Creation Date : 6/5/2026
// Last Modified : 6/5/2026
//
// Brief Description : Heals the target based on the result of the dice.
*****************************************************************************/
using FoolsBrand.Enemies;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "HealTargetAction", menuName = "Scriptable Objects/Actions/Heal Target")]
    public class HealTargetAction : DiceAction
    {
        public override int PriorityValue => 51;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            if (target is Limb targetLimb)
            {
                targetLimb.ParentEnemy.Health.Value += value;
                if (!targetLimb.IsBody)
                {
                    target.Health.Value += value;
                }
            }
            else
            {
                target.Health.Value += value;
            }

            yield return null;
        }
    }
}
