/*****************************************************************************
// File Name : PlayerCombatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Main combatant for the player.
*****************************************************************************/
using FoolsBrand.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoolsBrand
{
    public class PlayerCombatant : Combatant, IEffectable, IActionSource
    {
        [SerializeField] private float postActDelay;
        [field: SerializeField] public Transform DamageNumberPoint { get; private set; }
        [SerializeField] private Transform effectPoint;

        private List<EffectInstance> Effects = new List<EffectInstance>();

        private MinPriorityQueue<DiceActionInfo> actionQueue;
        private Limb targetedLimb;

        public event Action<EffectInstance> EffectAppliedEvent;
        public event Action PlayerActEvent;

        /// <summary>
        /// Player queries any effects for modifying or triggering on damage.
        /// </summary>
        /// <param name="damage">The damage to deal to the target.</param>
        /// <param name="target">The target of the attack.</param>
        /// <returns>The amount of damage dealt.</returns>
        public override int Attack(int damage, ITargetable target)
        {
            foreach (EffectInstance effect in Effects)
            {
                damage = effect.ModifyAttack(damage);
            }
            int damageDealt = base.Attack(damage, target);
            for(int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) { break; }
                Effects[i].OnDealDamage(this, this, target, damageDealt);
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
            foreach (EffectInstance effect in Effects)
            {
                damage = effect.ModifyDamage(damage);
            }
            // Apply defense.
            damage = Mathf.Max(damage - Defense, 0);

            int damageTaken = base.TakeDamage(damage, source);

            // Trigger any on damage effects.
            if (!Health.IsDead)
            {
                for (int i = 0; i < Effects.Count; i++)
                {
                    if (IsDead) { break; }
                    Effects[i].OnTakeDamage(this, this, source, damageTaken);
                }
            }
            return damageTaken;
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            RemoveAllEffects();
        }

        /// <summary>
        /// Sets the target and actions that the player will perform when they act.
        /// </summary>
        /// <param name="actionQueue">The actions that the player will take.</param>
        /// <param name="targetedLimb">The limb the player is targeting.</param>
        public void SetActData(MinPriorityQueue<DiceActionInfo> actionQueue, Limb targetedLimb)
        {
            this.actionQueue = actionQueue;
            this.targetedLimb = targetedLimb;
        }

        /// <summary>
        /// Handles the player taking their action.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override IEnumerator Act(Combatant target)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) { yield break; }
                yield return Effects[i].OnActionStart(this, this);
            }

            yield return StartCoroutine(ProcessActions(actionQueue, this, targetedLimb));

            for (int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) { yield break; }
                yield return Effects[i].OnActionEnd(this, this);
            }
            PlayerActEvent?.Invoke();
            FlushEffects();

            // Clear action data.
            actionQueue = null;
            targetedLimb = null;

            yield return new WaitForSeconds(postActDelay);
        }

        /// <summary>
        /// Player effects need to be offset.
        /// </summary>
        /// <returns></returns>
        public override Transform GetEffectTransform()
        {
            return effectPoint;
        }

        #region Effects
        /// <summary>
        /// Applies a temporary effect to this combatant.
        /// </summary>
        /// <param name="toApply"></param>
        public void ApplyEffect(Effect toApply, int value)
        {
            if (!toApply.AllowStacking && Effects.Any(X => X.Effect == toApply))
            {
                // Prevent duplicates being added if stacking is not allowed.
                return;
            }
            EffectInstance instance = toApply.CreateInstance(value);
            instance.OnEffectAdded(this, this, gameObject);
            Effects.Add(instance);
            EffectAppliedEvent?.Invoke(instance);
        }

        /// <summary>
        /// Removes an effect by it's type name
        /// </summary>
        /// <param name="className"></param>
        public void RemoveEffect(Effect toRemove)
        {
            for(int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].Effect == toRemove)
                {
                    Effects[i].OnEffectRemoved(this, this);
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Removes all effects on the player.
        /// </summary>
        public void RemoveAllEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].OnEffectRemoved(this, this);
            }

            Effects.Clear();
        }

        /// <summary>
        /// Removes all effects with 0 duration.
        /// </summary>
        public void FlushEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].IsExpired)
                {
                    Effects[i].OnEffectRemoved(this, this);
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion
    }
}
