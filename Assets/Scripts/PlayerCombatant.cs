/*****************************************************************************
// File Name : PlayerCombatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Main combatant for the player.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class PlayerCombatant : Combatant
    {
        [SerializeField] private int defense;

        protected override int CalcDamage(int inDamage)
        {
            return Mathf.Max(inDamage - defense, 0);
        }

        public override IEnumerator Act(Combatant target)
        {
            throw new System.NotImplementedException();
        }
    }
}
