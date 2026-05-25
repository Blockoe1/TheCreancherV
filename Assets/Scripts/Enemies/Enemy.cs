/*****************************************************************************
// File Name : Enemy.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script for enemies that controls their limbs and actions during combat.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private HealthStruct health;
        [SerializeField] private UnityEvent onDeathEvent;

        private Limb[] limbs;

        public HealthStruct Health => health;

        public void Init()
        {
            limbs = GetComponentsInChildren<Limb>();
            foreach (Limb limb in limbs)
            {
                limb.Init();
            }
        }
        

        /// <summary>
        /// Deals damage to an enemy, attacking a specific limb by index.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="limbIndex"></param>
        public void Attack(int damage, int limbIndex)
        {
            if (health.IsDead)
            {
                Debug.LogError($"Enemy {name} took damage while dead.");
                return;
            }
            int mainDamage = limbs[limbIndex].AttackLimb(damage);

            health.Value -= mainDamage;
            if (health.IsDead)
            {
                // Death Handling.
                onDeathEvent?.Invoke();
            }
        }
    }
}
