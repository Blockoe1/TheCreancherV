/*****************************************************************************
// File Name : HealAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Heals the user based on the result.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public class HealAction : DiceAction
    {
        public override int PriorityValue => 50;

        public override void PerformAction(ITargetable target, Combatant user)
        {
            Debug.Log($"{user} healed for {value}.");
        }
    }
}
