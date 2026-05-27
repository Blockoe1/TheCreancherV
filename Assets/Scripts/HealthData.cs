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

        public event Action<int> HealthChangedEvent;

        private bool isDead;
        private bool initialized;
        private int health;

        public float HealthProportion => health / maxHealth;
        public bool IsDead => isDead;

        public int Value
        {
            get 
            {
                Initialize();
                return health; 
            }
            set
            {
                Initialize();
                health = Mathf.Clamp(health + value, 0, maxHealth);
                HealthChangedEvent?.Invoke(health);
                if (health <= 0)
                {
                    isDead = true;
                }
            }
        }

        private void Initialize()
        {
            if (!initialized)
            {
                health = maxHealth;
                initialized = true;
            }
        }
    }
}
