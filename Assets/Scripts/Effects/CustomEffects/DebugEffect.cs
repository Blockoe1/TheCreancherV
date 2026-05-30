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

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            Debug.Log("Debug Effect Added");
            base.OnEffectAdded(combatant, effectSource, appliedObj);
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debug Effect Removed");
            base.OnEffectRemoved(combatant, effectSource);
        }

        public override void OnActionStart(Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debug action start");
            base.OnActionStart(combatant, effectSource);
        }

        public override void OnActionEnd(Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debut Action End");
            base.OnActionEnd(combatant, effectSource);
        }

        public override void OnDealDamage(Combatant combatant, IEffectable effectSource, ITargetable target, int damageDealt)
        {
            Debug.Log("Debug Damage dealt " + damageDealt);
            base.OnDealDamage(combatant, effectSource, target, damageDealt);
        }

        public override void OnTakeDamage(Combatant combatant, IEffectable effectSource, Combatant attacker, int damageTaken)
        {
            Debug.Log("Debut Damage Taken " + damageTaken);
            base.OnTakeDamage(combatant, effectSource, attacker, damageTaken);
        }
    }
}
