/*****************************************************************************
// File Name : CombatManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls the progression of combat.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public class DamageAction : DiceAction
    {
        public override int PriorityValue => 100;

        public override void PerformAction()
        {
            Debug.Log("Attack");
        }
    }
}
