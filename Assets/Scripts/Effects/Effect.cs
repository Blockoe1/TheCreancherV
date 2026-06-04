/*****************************************************************************
// File Name : Effect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : custom temporary effect that can be applied to a combatant.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public abstract class Effect : ScriptableObject
    {
        [SerializeField] private bool hasDuration;
        [SerializeField, ShowIf("hasDuration"), AllowNesting] private bool useValueAsDuration;
        [SerializeField, HideIf("useValueAsDuration"), AllowNesting] protected int baseDuration;
        [SerializeField] protected ParticleSystem visualEffect;

        public bool HasDuration => hasDuration;
        public bool UseValueAsDuration => useValueAsDuration;
        public int BaseDuration => baseDuration;

        /// <summary>
        /// Spawns the VFX object for this effect on the applied transform.
        /// </summary>
        /// <param name="parentTransform"></param>
        /// <returns></returns>
        public virtual ParticleSystem SpawnVFX(Transform parentTransform)
        {
            if (visualEffect == null) { return null; }
            return GameObject.Instantiate(visualEffect, parentTransform);
        }

        public virtual void OnEffectAdded(EffectInstance instance, Combatant combatant, IEffectable effectSource, GameObject appliedObj) { }

        /// <summary>
        /// Called before damage is dealt to apply any damage modifications.
        /// </summary>
        /// <param name="dealtDamage">The base damage the combatant would deal.</param>
        /// <returns>The modified damage amount.</returns>
        public virtual int ModifyAttack(EffectInstance instance, int dealtDamage) { return dealtDamage; }
        /// <summary>
        /// Called before damage is taken.
        /// </summary>
        /// <param name="takenDamage">The base damage the combatant is taking.</param>
        /// <returns>The modified damage from this effect.</returns>
        public virtual int ModifyDamage(EffectInstance instance, int takenDamage) { return takenDamage; }
        public virtual IEnumerator OnActionStart(EffectInstance instance, Combatant combatant, IEffectable effectSource) 
        {
            yield break;
        }
        public virtual IEnumerator OnActionEnd(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            yield break;
        }
        public virtual void OnTakeDamage(EffectInstance instance, Combatant combatant, IEffectable effectSource, Combatant attacker, int damageTaken) { }
        public virtual void OnDealDamage(EffectInstance instance, Combatant combatant, IEffectable effectSource, ITargetable target,int damageDealt) { }

        public virtual void OnEffectRemoved(EffectInstance instance, Combatant combatant, IEffectable effectSource) { }
    }
}
