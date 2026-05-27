/*****************************************************************************
// File Name : Enemy.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script for enemies that controls their limbs and actions during combat.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace FoolsBrand.Enemies
{
    public class Enemy : Combatant
    {
        private Limb[] limbs;

        public bool IsDead => Health.IsDead;

        public ReadOnlyArray<Limb> Limbs => limbs;
        
        public void Init()
        {
            limbs = GetComponentsInChildren<Limb>();
            foreach (Limb limb in limbs)
            {
                limb.Init();
            }
        }

        public void AttackEnemy(int damage, int limbIndex)
        {
            int mainDamage = limbs[limbIndex].AttackLimb(damage);
            TakeDamage(mainDamage);
        }

        private Limb GetRandomLimb()
        {
            return limbs[Random.Range(0, limbs.Length)];
        }

        /// <summary>
        /// When enemies act, they choose a random limb and execute an attack based on that limb's attack dice.
        /// </summary>
        public override IEnumerator Act()
        {
            List<DiceAction> actions = GetRandomLimb().RollAttack();
            yield return StartCoroutine(ProcessActions(actions));
        }
    }
}
