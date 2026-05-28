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
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace FoolsBrand.Enemies
{
    public class Enemy : Combatant
    {
        private Limb[] limbs;

        private Limb[] attackLimbs;

        public bool IsDead => Health.IsDead;

        public ReadOnlyArray<Limb> Limbs => limbs;
        
        public void Init()
        {
            limbs = GetComponentsInChildren<Limb>();
            foreach (Limb limb in limbs)
            {
                limb.Init(this);
            }

            attackLimbs = limbs.Where(x => x.HasAttack).ToArray();
        }

        /// <summary>
        /// Get a limb by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Limb GetLimb(int index)
        {
            return limbs[index];
        }

        private Limb GetRandomLimbWeighted(Limb[] limbs)
        {
            int totalWeight = 0;
            foreach(Limb limb in limbs)
            {
                totalWeight += limb.AttackWeight;
            }

            int random = Random.Range(0, totalWeight);
            for(int i = 0; i < limbs.Length; i++)
            {
                random -= limbs[i].AttackWeight;
                if (random < 0)
                {
                    return limbs[i];
                }
            }
            return limbs[^1];
        }

        /// <summary>
        /// When enemies act, they choose a random limb and execute an attack based on that limb's attack dice.
        /// </summary>
        public override IEnumerator Act(Combatant target)
        {
            // Flush the attack limbs array of dead limbs.
            attackLimbs = attackLimbs.Where(x => x != null && !x.Health.IsDead).ToArray();

            Limb attackLimb = GetRandomLimbWeighted(attackLimbs);
            DiceAction[] actions = attackLimb.RollAttack();
            yield return StartCoroutine(ProcessActions(actions, target));
        }
    }
}
