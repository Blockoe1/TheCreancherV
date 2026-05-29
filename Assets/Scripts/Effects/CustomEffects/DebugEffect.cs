/*****************************************************************************
// File Name : DebugEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Prints all effect callbacks to the console.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class DebugEffect : Effect
    {
        public DebugEffect(Effect copy) : base(copy)
        {
        }

        public override Effect Copy()
        {
            return new DebugEffect(this);
        }

        public override int ModifyAttack(int dealtDamage)
        {
            Debug.Log("Effect modified attack of damage " + dealtDamage);
            return base.ModifyAttack(dealtDamage);
        }

        public override int ModifyDamage(int takenDamage)
        {
            Debug.Log("Effect modified attack of damage " + takenDamage);
            return base.ModifyDamage(takenDamage);
        }

        public override void OnEffectAdded(Combatant combatant, GameObject appliedObj)
        {
            Debug.Log("Debug Effect Added");
            base.OnEffectAdded(combatant, appliedObj);
        }

        public override void OnEffectRemoved(Combatant combatant)
        {
            Debug.Log("Debug Effect Removed");
            base.OnEffectRemoved(combatant);
        }

        public override void OnActionStart(Combatant combatant)
        {
            Debug.Log("Debug action start");
            base.OnActionStart(combatant);
        }

        public override void OnActionEnd(Combatant combatant)
        {
            Debug.Log("Debut Action End");
            base.OnActionEnd(combatant);
        }

        public override void OnDealDamage(Combatant combatant, ITargetable target, int damageDealt)
        {
            Debug.Log("Debug Damage dealt " + damageDealt);
            base.OnDealDamage(combatant, target, damageDealt);
        }

        public override void OnTakeDamage(Combatant combatant, Combatant attacker, int damageTaken)
        {
            Debug.Log("Debut Damage Taken " + damageTaken);
            base.OnTakeDamage(combatant, attacker, damageTaken);
        }
    }
}
