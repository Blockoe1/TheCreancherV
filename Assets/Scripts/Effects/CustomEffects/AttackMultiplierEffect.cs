/*****************************************************************************
// File Name : AttackmultiplierEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 6/1/2026
// Last Modified : 6/1/2026
//
// Brief Description : Multiplies damage dealt during the next attack.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "AttackMultiplierEffect", menuName = "Scriptable Objects/Effects/Attack Multiplier")]
    public class AttackMultiplierEffect : Effect
    {
        [SerializeField] private float multiplier; 
        public AttackMultiplierEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            AttackMultiplierEffect copy = new AttackMultiplierEffect(this);
            copy.multiplier = multiplier;
            return copy;
        }

        public override int ModifyAttack(int dealtDamage)
        {
            markRemove = true;
            return Mathf.CeilToInt(dealtDamage * multiplier);
        }
    }
}
