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
        [SerializeField] private Sprite icon;
        [SerializeField, TextArea, Tooltip("Use #potency or #duration to insert the potency and duration " +
            "values of the instance into the description.")] 
        private string description;
        [SerializeField, Tooltip("Determines what effect the value of the dice face this effect was rolled on has on the effect.")] 
        private ValueUsage valueUsage;
        [SerializeField] private bool hasDuration;
        [SerializeField, ShowIf(nameof(ShowDuration)), AllowNesting] 
        protected int baseDuration = 1;
        [SerializeField, ShowIf(nameof(ShowPotency)), AllowNesting] protected int basePotency = 1;
        [SerializeField] private bool allowStacking;
        [SerializeField] protected ParticleSystem visualEffect;

        public Sprite Icon => icon;
        public bool ShowDuration => hasDuration && valueUsage != ValueUsage.Duration;
        public bool ShowPotency => valueUsage != ValueUsage.Potency && UsesPotency;
        public bool AllowStacking => allowStacking;
        public bool HasDuration => hasDuration;
        public int BaseDuration => baseDuration;
        public string Description => description;

        public virtual bool UsesPotency => true;

        private enum ValueUsage
        {
            None,
            Potency,
            Duration
        }

        /// <summary>
        /// Creates a new instance of this effect.
        /// </summary>
        /// <param name="value">The value to use that determines the strength of the effect.</param>
        /// <returns></returns>
        public virtual EffectInstance CreateInstance(int value)
        {
            int potency = (valueUsage == ValueUsage.Potency) ? value : basePotency;
            int duration = (valueUsage == ValueUsage.Duration) ? value : baseDuration;
            return new EffectInstance(this, potency, duration);
        }

        /// <summary>
        /// Spawns the VFX object for this effect on the applied transform.
        /// </summary>
        /// <param name="parentTransform"></param>
        /// <returns></returns>
        public virtual ParticleSystem SpawnVFX(Transform parentTransform, IEffectable effectable)
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
