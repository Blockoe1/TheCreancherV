/*****************************************************************************
// File Name : Effect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : custom temporary effect that can be applied to a combatant.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public abstract class Effect
    {
        [SerializeField] private int duration;

        public int Duration => duration;

        public Effect(Effect copy)
        {
            this.duration = copy.duration;
        }


        /// <summary>
        /// Copies the given effect.
        /// </summary>
        /// <returns></returns>
        public abstract Effect Copy();

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
        public virtual void OnActionStart(Combatant combatant) { }
        public virtual void OnActionEnd(Combatant combatant)
        {
            duration--;
        }
        public virtual void OnTakeDamage(Combatant combatant, Combatant attacker, int damageTaken) { }
        public virtual void OnDealDamage(Combatant combatant, ITargetable target,int damageDealt) { }
    }
}
