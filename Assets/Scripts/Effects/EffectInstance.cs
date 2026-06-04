/*****************************************************************************
// File Name : EffectInstance.cs
// Author : Arcadia Koederitz
// Creation Date : 6/3/2026
// Last Modified : 6/3/2026
//
// Brief Description : Specific instance of an effect that tracks potency and duration while applied to a combatant.
*****************************************************************************/
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace FoolsBrand
{
    public class EffectInstance
    {
        private readonly Effect effect;
        private readonly int potency;
        private int duration;

        private ParticleSystem vfxInstance;

        public bool MarkRemove { get; set; }

        public int Potency => potency;
        public int Duration => duration;
        public bool IsExpired => (effect.HasDuration && duration <= 0) || MarkRemove;

        public EffectInstance(Effect effect, int potency, int duration)
        {
            this.effect = effect;
            this.potency = potency;
            this.duration = duration;
        }

        public void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            vfxInstance = effect.SpawnVFX(effectSource.GetEffectTransform());

            effect.OnEffectAdded(this, combatant, effectSource, appliedObj);
        }

        /// <summary>
        /// Called before damage is dealt to apply any damage modifications.
        /// </summary>
        /// <param name="dealtDamage">The base damage the combatant would deal.</param>
        /// <returns>The modified damage amount.</returns>
        public int ModifyAttack(int dealtDamage)
        {
            return effect.ModifyAttack(this, dealtDamage);
        }
        /// <summary>
        /// Called before damage is taken.
        /// </summary>
        /// <param name="takenDamage">The base damage the combatant is taking.</param>
        /// <returns>The modified damage from this effect.</returns>
        public int ModifyDamage(int takenDamage)
        {
            return effect.ModifyDamage(this, takenDamage);
        }
        public IEnumerator OnActionStart(Combatant combatant, IEffectable effectSource)
        {
            yield return effect.OnActionStart(this, combatant, effectSource);
        }
        public IEnumerator OnActionEnd(Combatant combatant, IEffectable effectSource)
        {
            // Decrement duration.
            if (effect.HasDuration)
            {
                duration--;
            }
            yield return effect.OnActionEnd(this, combatant, effectSource);
        }
        public void OnTakeDamage(Combatant combatant, IEffectable effectSource, Combatant attacker, int damageTaken)
        {
            effect.OnTakeDamage(this, combatant, effectSource, attacker, damageTaken);
        }
        public void OnDealDamage(Combatant combatant, IEffectable effectSource, ITargetable target, int damageDealt)
        {
            effect.OnDealDamage(this, combatant, effectSource, target, damageDealt);
        }

        public void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            if (vfxInstance != null)
            {
                GameObject.Destroy(vfxInstance.gameObject);
            }
            effect.OnEffectRemoved(this, combatant, effectSource);
        }
    }
}
