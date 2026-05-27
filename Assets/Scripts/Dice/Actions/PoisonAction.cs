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
    public class PoisonAction : DiceAction
    {
        public override int PriorityValue => 101;

        public override void PerformAction()
        {
            Debug.Log("Poison");
        }
    }
}
