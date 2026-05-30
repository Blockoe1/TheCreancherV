/*****************************************************************************
// File Name : Limb.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls an enemy's limbs and their relevant stats.
*****************************************************************************/
using NaughtyAttributes;
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
        [Header("Events")]
        [SerializeField] private UnityEvent<int> onDamageEvent;
        [SerializeField] private UnityEvent onDestroyEvent;

        protected Enemy parentEnemy;

        private readonly List<Effect> Effects = new List<Effect>();

        #region Properties
        public bool IsDead => !isBody && health.IsDead;
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
        public MinPriorityQueue<DiceAction> RollAttack()
        {
            if (attackDice == null)
            {
                Debug.LogWarning($"Enemy {transform.parent.gameObject.name} does not have an attack dice assigned to it's {name} limb.");
            }
            DiceAction[] actions = attackDice.RollDie();
            MinPriorityQueue<DiceAction> sortedActions = new MinPriorityQueue<DiceAction>();
            foreach(DiceAction action in actions)
            {
                // Need to make sure we re-order the type enum to include the execution order.
                sortedActions.Enqueue(action, action.PriorityValue);
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
            foreach (Effect effect in Effects)
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
                    LimbDestroyed();
                    onDestroyEvent?.Invoke();
                    gameObject.SetActive(false);
                }

                // Trigger any on damage effects.
                if (!Health.IsDead)
                {
                    foreach (Effect effect in Effects)
                    {
                        effect.OnTakeDamage(parentEnemy, this, source, damageTaken);
                    }
                }
            }

            // Deal damage to the main enemy.
            return parentEnemy.TakeDamage(Mathf.RoundToInt(damage * multiplier), source);
        }

        #region Effects
        /// <summary>
        /// Applie a custom effect to this limb.
        /// </summary>
        /// <param name="toApply"></param>
        public void ApplyEffect(Effect toApply)
        {
            Effect copy = toApply.Copy();
            copy.OnEffectAdded(parentEnemy, this, gameObject);
            Effects.Add(copy);
        }

        /// <summary>
        /// Start/end functions called by the base enemy
        /// </summary>
        public void OnActionStart()
        {
            foreach (Effect effect in Effects)
            {
                effect.OnActionStart(parentEnemy, this);
            }
        }
        public void OnActionEnd()
        {
            foreach (Effect effect in Effects)
            {
                effect.OnActionEnd(parentEnemy, this);
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
            foreach (Effect effect in Effects)
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
            foreach (Effect effect in Effects)
            {
                effect.OnDealDamage(enemy, this, target, damageDealt);
            }
        }

        /// <summary>
        /// Removes an effect by it's type name
        /// </summary>
        /// <param name="className"></param>
        public void RemoveEffect(string className)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].GetType().Name == className)
                {
                    Effects[i].OnEffectRemoved(parentEnemy, this);
                    Effects.RemoveAt(i);
                    i--;
                }
            }
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

        #region Custom Effect Functions
        protected virtual void LimbStart() { }

        protected virtual void LimbDestroyed() { }
        #endregion

        #region Debug
        [ContextMenu("Kill")]
        private void Kill()
        {
            LimbDestroyed();
            onDestroyEvent?.Invoke();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
