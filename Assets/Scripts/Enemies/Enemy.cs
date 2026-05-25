/*****************************************************************************
// File Name : Enemy.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script for enemies that controls their limbs and actions during combat.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int maxHealth;

        private int health;

        private Limb[] limbs;

        public void Init()
        {
            limbs = GetComponentsInChildren<Limb>();
        }

        /// <summary>
        /// Deals damage to an enemy, attacking a specific limb by index.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="limbIndex"></param>
        public void Attack(int damage, int limbIndex)
        {
            int mainDamage = limbs[limbIndex].AttackLimb(damage);


        }
    }
}
