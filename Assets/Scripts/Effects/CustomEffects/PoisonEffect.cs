/*****************************************************************************
// File Name : PoisonEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Deals continual damage to the applied target every turn.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class PoisonEffect : Effect
    {
        [SerializeField] private int tickDamage;
        [SerializeField] private GameObject poisonEffect;

        private GameObject effectInstance;

        public PoisonEffect(Effect copy) : base(copy)
        {
            PoisonEffect pCopy = copy as PoisonEffect;
            poisonEffect = pCopy.poisonEffect;
            tickDamage = pCopy.tickDamage;
        }

        public override Effect Copy()
        {
            return new PoisonEffect(this);
        }

        public override void OnEffectAdded(Combatant combatant, GameObject appliedObj)
        {
            effectInstance = GameObject.Instantiate(poisonEffect, appliedObj.transform);
            Debug.Log(effectInstance);
        }

        public override void OnEffectRemoved(Combatant combatant)
        {
            Debug.Log("Poison Removed");
            GameObject.Destroy(effectInstance);
        }

        /// <summary>
        /// Deals damage to the main enemy health.
        /// </summary>
        /// <param name="combatant">The combatant to deal poison damage to.</param>
        public override void OnActionStart(Combatant combatant)
        {
            if (combatant.Health.IsDead)
            {
                return;
            }
            base.OnActionStart(combatant);
            combatant.Health.Value -= tickDamage;
            combatant.CheckForDeath();
        }
    }
}
