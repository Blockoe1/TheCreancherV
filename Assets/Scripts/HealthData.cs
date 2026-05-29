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

        public float HealthProportion => health / (float)maxHealth;
        public bool IsDead => isDead;

        public int Value
        {
            get 
            {
                return health; 
            }
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                HealthChangedEvent?.Invoke(health);
                if (health <= 0)
                {
                    isDead = true;
                }
            }
        }
    }
}
