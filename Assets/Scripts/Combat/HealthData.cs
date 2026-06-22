/*****************************************************************************
// File Name : Health.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base class for enemy and player health.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class HealthData
    {
        [SerializeField] private int maxHealth;
        [SerializeField, ReadOnly] private int health;

        public event Action<int> HealthChangedEvent;

        private bool isDead;
        private bool isLowHealth;

        public int Max => maxHealth;
        public float HealthProportion => health / (float)maxHealth;
        public bool IsDead => isDead;
        public bool IsLowHealth => isLowHealth;

        public int Value
        {
            get 
            {
                return health; 
            }
            set
            {
                int healthChange = value - health;
                health = Mathf.Clamp(value, 0, maxHealth);
                HealthChangedEvent?.Invoke(healthChange);
                if (health <= 0)
                {
                    isDead = true;
                }
                if (health < maxHealth / 3)
                {                     isLowHealth = true;
                }
                else
                {
                    isLowHealth = false;
                }
            }
        }
    }
}
