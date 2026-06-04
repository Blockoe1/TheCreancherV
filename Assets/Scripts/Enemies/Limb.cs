/*****************************************************************************
// File Name : Limb.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls an enemy's limbs and their relevant stats.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand.Enemies
{
    public class Limb : MonoBehaviour, ITargetable, IEffectable, IActionSource
    {
        #region CONSTS
        private const string BODY_NAME = "Body";
        #endregion
        [SerializeField] private bool isBody;
        [SerializeField, HideIf("isBody")] private HealthData health;
        [SerializeField] private int defense;
        [SerializeField] private float multiplier;
        [SerializeField, ShowIf("HasAttack")] private int attackWeight = 1;
        [SerializeField] private DieBase attackDice;
        [SerializeField, Tooltip("Adds this string to the end of an animation name for actions perfomed by this limb.  " +
            "Only needs to be set if the limb has a custom animation.")]
        private string limbAnimNameSuffix;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> onDamageEvent;
        [SerializeField] private UnityEvent onDestroyEvent;

        protected Enemy parentEnemy;

        private readonly List<EffectInstance> Effects = new List<EffectInstance>();

        #region Properties
        public bool IsDead => (!isBody && health.IsDead) || (parentEnemy != null && parentEnemy.IsDead);
        public bool IsBody => isBody;
        public bool HasAttack => attackDice != null;
        public HealthData Health => health;
        public string LimbName => isBody ? BODY_NAME : name;
        public int Defense => defense;
        public int AttackWeight => attackWeight;
        public float Multiplier => multiplier;
        public UnityEvent OnDestroyEvent => onDestroyEvent;
        #endregion

        public void Init(Enemy parentEnemy)
        {
            this.parentEnemy = parentEnemy;

            LimbStart();
        }

        /// <summary>
        /// Queries this limb's attack dice for the damage to deal from an attack.
        /// </summary>
        /// <remarks>Does not yet apply custom effects.</remarks>
        /// <returns>The damage dealt by this limb.</returns>
        public MinPriorityQueue<DiceActionInfo> RollAttack()
        {
            if (attackDice == null)
            {
                Debug.LogWarning($"Enemy {transform.parent.gameObject.name} does not have an attack dice assigned to it's {name} limb.");
            }
            DiceActionInfo[] actions = attackDice.RollDie();
            MinPriorityQueue<DiceActionInfo> sortedActions = new MinPriorityQueue<DiceActionInfo>();
            foreach(DiceActionInfo actionInfo in actions)
            {
                // Need to make sure we re-order the type enum to include the execution order.
                sortedActions.Enqueue(actionInfo, actionInfo.Action.PriorityValue);
            }

            return sortedActions;
        }

        /// <summary>
        /// Attacks this limb, outputting the damage that is dealt to the main enemy health.
        /// </summary>
        /// <param name="baseDamage">The damage to deal to the limb.</param>
        /// <returns></returns>
        public int TakeDamage(int baseDamage, Combatant source)
        {
            if (health.IsDead)
            {
                return 0;
            }

            // Apply any damage reduction effects.
            foreach (EffectInstance effect in Effects)
            {
                baseDamage = effect.ModifyDamage(baseDamage);
            }
            // Apply defense
            int damage = Mathf.Max(baseDamage - defense, 0);

            // Deal damage to the limb.
            if (!isBody)
            {
                int preHealth = health.Value;
                health.Value -= damage;
                int damageTaken = preHealth - health.Value;
                onDamageEvent?.Invoke(damage);
                if (health.IsDead)
                {
                    // If the limb dies to damage, disable it.
                    gameObject.SetActive(false);
                    OnLimbDeath();
                }

                // Trigger any on damage effects.
                if (!Health.IsDead)
                {
                    for (int i = 0; i < Effects.Count; i++)
                    {
                        if (IsDead) { break; }
                        Effects[i].OnTakeDamage(parentEnemy, this, source, damageTaken);
                    }
                }
            }

            // Deal damage to the main enemy.
            return parentEnemy.TakeDamage(Mathf.RoundToInt(damage * multiplier), source);
        }

        /// <summary>
        /// Plays an animation by name and returns the clip played.
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public AnimationClip PlayAnimation(string animationName)
        {
            return parentEnemy.PlayAnimation(animationName + limbAnimNameSuffix);
        }

        /// <summary>
        /// Effects play at the position of the limb.
        /// </summary>
        /// <returns></returns>
        public Transform GetEffectTransform()
        {
            return transform;
        }

        #region Effects
        /// <summary>
        /// Applie a custom effect to this limb.
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
            instance.OnEffectAdded(parentEnemy, this, gameObject);
            Effects.Add(instance);
        }

        /// <summary>
        /// Start/end functions called by the base enemy
        /// </summary>
        public IEnumerator OnActionStart()
        {
            for(int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) { yield break; }
                yield return Effects[i].OnActionStart(parentEnemy, this);
            }
        }
        public IEnumerator OnActionEnd()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) { yield break; }
                yield return Effects[i].OnActionEnd(parentEnemy, this);
            }

            FlushEffects();
        }

        /// <summary>
        /// Applies any attack modifiers that this limb has on it when the enemy attacks using this limb.
        /// </summary>
        /// <param name="damage">The base damage of the attack.</param>
        /// <returns>The modified damage amount.</returns>
        public int QueryAttackModifiers(int damage)
        {
            foreach (EffectInstance effect in Effects)
            {
                damage = effect.ModifyAttack(damage);
            }
            return damage;
        }

        /// <summary>
        /// Calls any triggered effects when this limb attacks.
        /// </summary>
        /// <param name="enemy">The enemy that attacked with this limb.</param>
        /// <param name="target">The target of the attack.</param>
        /// <param name="damageDealt">The damage dealt.</param>
        public void TriggerOnDamage(Enemy enemy, ITargetable target, int damageDealt)
        {
            for(int i = 0; i < Effects.Count; i++)
            {
                if (IsDead) {  break; }
                Effects[i].OnDealDamage(enemy, this, target, damageDealt);
            }
        }

        /// <summary>
        /// Removes an effect by it's type name
        /// </summary>
        /// <param name="className"></param>
        public void RemoveEffect(Effect toRemove)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].Effect == toRemove)
                {
                    Effects[i].OnEffectRemoved(parentEnemy, this);
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Removes all effects from the limb.
        /// </summary>
        public void RemoveAllEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].OnEffectRemoved(parentEnemy, this);
            }

            Effects.Clear();
        }

        /// <summary>
        /// Removes all effects that have their duration expired.
        /// </summary>
        public void FlushEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].IsExpired)
                {
                    Effects[i].OnEffectRemoved(parentEnemy, this);
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion

        public void OnLimbDeath()
        {
            LimbDestroyed();
            onDestroyEvent?.Invoke();
            RemoveAllEffects();
        }

        #region Custom Effect Functions
        protected virtual void LimbStart() { }

        protected virtual void LimbDestroyed() { }
        #endregion
    }
}
