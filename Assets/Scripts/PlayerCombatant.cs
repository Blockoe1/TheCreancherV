/*****************************************************************************
// File Name : PlayerCombatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Main combatant for the player.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoolsBrand
{
    public class PlayerCombatant : Combatant, IEffectable
    {
        [SerializeField] private int defense;

        private List<Effect> Effects = new List<Effect>();

        /// <summary>
        /// Player queries any effects for modifying or triggering on damage.
        /// </summary>
        /// <param name="damage">The damage to deal to the target.</param>
        /// <param name="target">The target of the attack.</param>
        /// <returns>The amount of damage dealt.</returns>
        public override int Attack(int damage, ITargetable target)
        {
            foreach (Effect effect in Effects)
            {
                damage = effect.ModifyAttack(damage);
            }
            int damageDealt = base.Attack(damage, target);
            foreach (Effect effect in Effects)
            {
                effect.OnDealDamage(this, target, damageDealt);
            }
            return damageDealt;
        }

        /// <summary>
        /// Queries effects for 
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public override int TakeDamage(int damage, Combatant source)
        {
            // Apply any damage reduction effects.
            foreach (Effect effect in Effects)
            {
                damage = effect.ModifyDamage(damage);
            }
            // Apply defense.
            damage = Mathf.Max(damage - defense, 0);

            int damageTaken = base.TakeDamage(damage, source);

            // Trigger any on damage effects.
            if (!Health.IsDead)
            {
                foreach (Effect effect in Effects)
                {
                    effect.OnTakeDamage(this, source, damageTaken);
                }
            }
            return damageTaken;
        }

        /// <summary>
        /// Handles the player taking their action.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override IEnumerator Act(Combatant target)
        {
            foreach (Effect effect in Effects)
            {
                effect.OnActionStart(this);
            }

            // Action action stuff.
            yield return null;

            foreach (Effect effect in Effects)
            {
                effect.OnActionEnd(this);
            }
            FlushEffects();
        }

        #region Effects
        /// <summary>
        /// Applies a temporary effect to this combatant.
        /// </summary>
        /// <param name="toApply"></param>
        public void ApplyEffect(Effect toApply)
        {
            Effects.Add(toApply.Copy());
        }

        /// <summary>
        /// Removes an effect by it's type name
        /// </summary>
        /// <param name="className"></param>
        public void RemoveEffect(string className)
        {
            Effects.RemoveAll(x => nameof(x) == className);
        }

        /// <summary>
        /// Removes all effects with 0 duration.
        /// </summary>
        public void FlushEffects()
        {
            Effects.RemoveAll(x => x.IsExpired);
        }
        #endregion
    }
}
