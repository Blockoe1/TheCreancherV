/*****************************************************************************
// File Name : Enemy.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script for enemies that controls their limbs and actions during combat.
*****************************************************************************/
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace FoolsBrand.Enemies
{
    public class Enemy : Combatant
    {
        [field: SerializeField, Tooltip("Determines how many actions the combatant gets in a turn.  " +
"Use decimals to make enemies move every other turn.")]
        public float BaseActionValue { get; set; } = 1;

        private float actionValue;

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
        /// Notify all limbs that the enemy has taken damage.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="source"></param>
        public override void TakeDamage(int damage, Combatant source)
        {
            base.TakeDamage(damage, source);
        }

        private Limb GetRandomLimbWeighted(Limb[] limbs)
        {
            int totalWeight = 0;
            foreach(Limb limb in limbs)
            {
                totalWeight += limb.AttackWeight;
            }

            int random = UnityEngine.Random.Range(0, totalWeight);
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

        private void ExecuteForActiveLimbs(Action<Limb> toExecute)
        {
            foreach (var limb in Limbs)
            {
                if (!limb.Health.IsDead)
                {
                    toExecute(limb);
                }
            }
        }

        /// <summary>
        /// When enemies act, they choose a random limb and execute an attack based on that limb's attack dice.
        /// </summary>
        public override IEnumerator Act(Combatant target)
        {
            // Enemy needs to accrue enough ActionValue to act.  BaseActionValue can be reduced by limbs being destroyed.
            actionValue += BaseActionValue;

            while (actionValue >= 1)
            {
                actionValue--;
                // Flush the attack limbs array of dead limbs.
                attackLimbs = attackLimbs.Where(x => x != null && !x.Health.IsDead).ToArray();

                Limb attackLimb = GetRandomLimbWeighted(attackLimbs);
                DiceAction[] actions = attackLimb.RollAttack();

                yield return StartCoroutine(ProcessActions(actions, target));
            }
        }
    }
}
