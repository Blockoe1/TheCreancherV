/*****************************************************************************
// File Name : DamageNumberManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/31/2026
// Last Modified : 5/31/2026
//
// Brief Description : Controls showing damage numbers for a particular enemy.
*****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class DamageNumberManager : Manager
    {
        [SerializeField] private ObjectPool<DamageNumber> numberPool;

        private readonly Dictionary<HealthData, DamageNumberRegistration> damageNumberRegistry = new();

        private class DamageNumberRegistration
        {
            private HealthData subscribedData;
            private Transform sourceTransform;
            private ObjectPool<DamageNumber> pool;

            public DamageNumberRegistration(HealthData subscribedData, Transform sourceTransform, ObjectPool<DamageNumber> pool)
            {
                this.subscribedData = subscribedData;
                this.sourceTransform = sourceTransform;
                this.pool = pool;

                subscribedData.HealthChangedEvent += PlayDamageNumber;
            }

            private void PlayDamageNumber(int healthChange)
            {
                DamageNumber num = pool.GetObject();
                num.Play(healthChange, GetScreenPos(sourceTransform.position), pool.ReturnObject);
            }

            // Call this before removing any registrations.
            public void CleanUp()
            {
                subscribedData.HealthChangedEvent -= PlayDamageNumber;
            }
        }

        public override void Deinit()
        {
            // Clear all damage numbers registered on deinit.

        }

        /// <summary>
        /// Adds a new health data class to show damage numbers for.
        /// </summary>
        /// <param name="health">The health data to show damage numbers for when updated.</param>
        /// <param name="sourceTransform">The transform that the damage numbers should appear at.</param>
        public void RegisterDamageNumber(HealthData health, Transform sourceTransform)
        {
            DamageNumberRegistration damageNum = new DamageNumberRegistration(health, sourceTransform, numberPool);
            damageNumberRegistry.Add(health, damageNum);
        }

        /// <summary>
        /// Unregisters a health data to display damage numbers.
        /// </summary>
        /// <param name="health">The health data to unregister.</param>
        public void UnregisterDamageNumber(HealthData health)
        {
            if (damageNumberRegistry.ContainsKey(health))
            {
                damageNumberRegistry[health].CleanUp();
                damageNumberRegistry.Remove(health);
            }
        }

        /// <summary>
        /// Unregisters all damage numbers.
        /// </summary>
        public void UnregisterAllDamageNumbers()
        {
            foreach(DamageNumberRegistration registration in damageNumberRegistry.Values)
            {
                registration.CleanUp();
            }
            damageNumberRegistry.Clear();
        }

        private static Vector3 GetScreenPos(Vector3 worldPos)
        {
            return UIManager.GameCamera.WorldToScreenPoint(worldPos);
        }
    }
}
