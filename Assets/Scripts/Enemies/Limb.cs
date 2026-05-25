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
        [SerializeField, HideIf("isBody")] private int maxHealth;
        [SerializeField] private int defense;
        [SerializeField] private float multiplier;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> onDamageEvent;
        [SerializeField] private UnityEvent onDestroyEvent;

        private int health;
        private bool isDead;

        public event Action<int> HealthChangedEvent;

        #region Properties
        public bool IsDead => isDead;
        public string LimbName => isBody ? BODY_NAME : name;
        public float HealthProportion => health / maxHealth;
        public int Defense => defense;
        public float Multiplier => multiplier;
        private int Health
        {
            get {  return health; }
            set
            {
                health = Mathf.Clamp(health + value, 0, maxHealth);
                HealthChangedEvent?.Invoke(health);
            }
        }
        #endregion

        /// <summary>
        /// Attacks this limb, outputting the damage that is dealt to the main enemy health.
        /// </summary>
        /// <param name="baseDamage">The damage to deal to the limb.</param>
        /// <returns></returns>
        public int AttackLimb(int baseDamage)
        {
            int damage = baseDamage - defense;

            // Deal damage to the limb.
            Health -= damage;
            onDamageEvent?.Invoke(damage);
            if (Health == 0)
            {
                isDead = true;
                onDestroyEvent?.Invoke();
                Destroy(gameObject);
            }

            // Deal damage to the main enemy.
            return Mathf.RoundToInt(damage * multiplier);
        }
    }
}
