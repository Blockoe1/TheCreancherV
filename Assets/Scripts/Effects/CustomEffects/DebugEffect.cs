/*****************************************************************************
// File Name : DebugEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Prints all effect callbacks to the console.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "DebugEffect", menuName = "Scriptable Objects/Effects/Debug")]
    public class DebugEffect : Effect
    {

        public override int ModifyAttack(EffectInstance instance, int dealtDamage)
        {
            Debug.Log("Effect modified attack of damage " + dealtDamage);
            return base.ModifyAttack(instance, dealtDamage);
        }

        public override int ModifyDamage(EffectInstance instance, int takenDamage)
        {
            Debug.Log("Effect modified attack of damage " + takenDamage);
            return base.ModifyDamage(instance, takenDamage);
        }

        public override void OnEffectAdded(EffectInstance instance, Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            Debug.Log("Debug Effect Added");
            base.OnEffectAdded(instance, combatant, effectSource, appliedObj);
        }

        public override void OnEffectRemoved(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debug Effect Removed");
            base.OnEffectRemoved(instance, combatant, effectSource);
        }

        public override IEnumerator OnActionStart(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debug action start");
            yield return base.OnActionStart(instance, combatant, effectSource);
        }

        public override IEnumerator OnActionEnd(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Debut Action End");
            yield return base.OnActionEnd(instance, combatant, effectSource);
        }

        public override void OnDealDamage(EffectInstance instance, Combatant combatant, IEffectable effectSource, ITargetable target, int damageDealt)
        {
            Debug.Log("Debug Damage dealt " + damageDealt);
            base.OnDealDamage(instance, combatant, effectSource, target, damageDealt);
        }

        public override void OnTakeDamage(EffectInstance instance, Combatant combatant, IEffectable effectSource, Combatant attacker, int damageTaken)
        {
            Debug.Log("Debut Damage Taken " + damageTaken);
            base.OnTakeDamage(instance, combatant, effectSource, attacker, damageTaken);
        }
    }
}
