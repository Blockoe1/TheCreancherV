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
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField, Tooltip("Determines how many actions the combatant gets in a turn.  " +
"Use decimals to make enemies move every other turn.")]
        public float BaseActionValue { get; set; } = 1;

        private float actionValue;

        private Limb[] limbs;
        private Limb[] attackLimbs;

        private Limb attackLimb;

        public ReadOnlyArray<Limb> Limbs => limbs;   
        
        public override void Init()
        {
            base.Init();
            limbs = GetComponentsInChildren<Limb>();
            foreach (Limb limb in limbs)
            {
                limb.Init(this);
            }

            attackLimbs = limbs.Where(x => x.HasAttack).ToArray();
        }

        public override int Attack(int damage, ITargetable target)
        {
            if (attackLimb == null)
            {
                Debug.LogError("Attack limb was not set.");
                return 0;
            }

            damage = attackLimb.QueryAttackModifiers(damage);
            int damageDealt = base.Attack(damage, target);
            attackLimb.TriggerOnDamage(this, target, damageDealt);
            return damageDealt;
        }

        /// <summary>
        ///  Destroy all limbs when the enemy is killed.
        /// </summary>
        protected override void OnDeath()
        {
            // Kill All Limbs
            if (limbs != null)
            {
                foreach (Limb limb in limbs)
                {
                    if (limb == null) { continue; }
                    if (!limb.IsDead)
                    {
                        limb.OnLimbDeath();
                    }
                }
            }
            base.OnDeath();
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
            if (IsDead) { yield break; }
            foreach (var limb in Limbs)
            {
                yield return limb.OnActionStart();
            }

            if (IsDead) { yield break; }
            // Enemy needs to accrue enough ActionValue to act.  BaseActionValue can be reduced by limbs being destroyed.
            actionValue += BaseActionValue;

            while (actionValue >= 1)
            {
                actionValue--;
                // Flush the attack limbs array of dead limbs.
                attackLimbs = attackLimbs.Where(x => x != null && !x.Health.IsDead).ToArray();

                attackLimb = GetRandomLimbWeighted(attackLimbs);

                MinPriorityQueue<DiceActionInfo> actions = attackLimb.RollAttack();

                yield return StartCoroutine(ProcessActions(actions, attackLimb, target));
                attackLimb = null;
            }

            foreach (var limb in Limbs)
            {
                yield return limb.OnActionEnd();
            }
        }
    }
}
