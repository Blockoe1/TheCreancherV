/*****************************************************************************
// File Name : Limb.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Controls an enemy's limbs and their relevant stats.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand.Enemies
{
    public class Limb : MonoBehaviour
    {
        #region CONSTS
        private const string BODY_NAME = "Body";
        #endregion

        [SerializeField] private bool isBody;
        [SerializeField, HideIf("isBody")] private HealthData health;
        [SerializeField] private int defense;
        [SerializeField] private float multiplier;
        [SerializeField] private DieBase attackDice;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> onDamageEvent;
        [SerializeField] private UnityEvent onDestroyEvent;

        #region Properties
        public bool HasAttack => attackDice != null;
        public HealthData Health => health;
        public string LimbName => isBody ? BODY_NAME : name;
        public int Defense => defense;
        public float Multiplier => multiplier;
        #endregion

        public void Init()
        {

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
        public int AttackLimb(int baseDamage)
        {
            if (health.IsDead)
            {
                Debug.LogError($"Limb {LimbName} took damage while dead.");
                return 0;
            }
            int damage = baseDamage - defense;

            // Deal damage to the limb.
            health.Value -= damage;
            onDamageEvent?.Invoke(damage);
            if (health.IsDead)
            {
                onDestroyEvent?.Invoke();
                Destroy(gameObject);
            }

            // Deal damage to the main enemy.
            return Mathf.RoundToInt(damage * multiplier);
        }
    }
}
