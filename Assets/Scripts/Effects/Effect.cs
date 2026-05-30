/*****************************************************************************
// File Name : Effect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : custom temporary effect that can be applied to a combatant.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public abstract class Effect
    {
        [SerializeField] private bool hasDuration = true;
        [SerializeField, ShowIf("hasDuration"), AllowNesting] protected int duration;

        public bool IsExpired => hasDuration && duration <= 0;

        public Effect(Effect copy)
        {
            this.duration = copy.duration;
            this.hasDuration = copy.hasDuration;
        }


        /// <summary>
        /// Copies the given effect.
        /// </summary>
        /// <returns></returns>
        public abstract Effect Copy();

        public virtual void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj) { }

        /// <summary>
        /// Called before damage is dealt to apply any damage modifications.
        /// </summary>
        /// <param name="dealtDamage">The base damage the combatant would deal.</param>
        /// <returns>The modified damage amount.</returns>
        public virtual int ModifyAttack(int dealtDamage) { return dealtDamage; }
        /// <summary>
        /// Called before damage is taken.
        /// </summary>
        /// <param name="takenDamage">The base damage the combatant is taking.</param>
        /// <returns>The modified damage from this effect.</returns>
        public virtual int ModifyDamage(int takenDamage) { return takenDamage; }
        public virtual void OnActionStart(Combatant combatant, IEffectable effectSource) { }
        public virtual void OnActionEnd(Combatant combatant, IEffectable effectSource)
        {
            if (hasDuration)
            {
                duration--;
            }
        }
        public virtual void OnTakeDamage(Combatant combatant, IEffectable effectSource, Combatant attacker, int damageTaken) { }
        public virtual void OnDealDamage(Combatant combatant, IEffectable effectSource, ITargetable target,int damageDealt) { }

        public virtual void OnEffectRemoved(Combatant combatant, IEffectable effectSource) { }
    }
}
