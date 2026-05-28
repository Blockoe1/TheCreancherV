/*****************************************************************************
// File Name : Limb.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls an enemy's limbs and their relevant stats.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand.Enemies
{
    public class Limb : MonoBehaviour, ITargetable
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
        public DiceAction[] RollAttack()
        {
            if (attackDice == null)
            {
                Debug.LogWarning($"Enemy {transform.parent.gameObject.name} does not have an attack dice assigned to it's {name} limb.");
            }
            return attackDice.RollDie();
        }

        /// <summary>
        /// Attacks this limb, outputting the damage that is dealt to the main enemy health.
        /// </summary>
        /// <param name="baseDamage">The damage to deal to the limb.</param>
        /// <returns></returns>
        public void TakeDamage(int baseDamage, Combatant source)
        {
            if (health.IsDead)
            {
                Debug.LogError($"Limb {LimbName} took damage while dead.");
                return;
            }
            int damage = baseDamage - defense;

            // Deal damage to the limb.
            if (!isBody)
            {
                health.Value -= damage;
                onDamageEvent?.Invoke(damage);
                if (health.IsDead)
                {
                    LimbDestroyed();
                    onDestroyEvent?.Invoke();
                    gameObject.SetActive(false);
                }
            }

            // Deal damage to the main enemy.
            parentEnemy.TakeDamage(Mathf.RoundToInt(damage * multiplier), source);
        }

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
