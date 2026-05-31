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

        public PoisonEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            PoisonEffect copy = new PoisonEffect(this);
            poisonEffect = copy.poisonEffect;
            tickDamage = copy.tickDamage;
            return copy;
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            effectInstance = GameObject.Instantiate(poisonEffect, appliedObj.transform);
            //Debug.Log(effectInstance);
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            //Debug.Log("Poison Removed");
            GameObject.Destroy(effectInstance);
        }

        /// <summary>
        /// Deals damage to the main enemy health.
        /// </summary>
        /// <param name="combatant">The combatant to deal poison damage to.</param>
        public override void OnActionStart(Combatant combatant, IEffectable effectSource)
        {
            if (combatant.Health.IsDead)
            {
                return;
            }
            base.OnActionStart(combatant, effectSource);
            combatant.Health.Value -= tickDamage;
            combatant.CheckForDeath();
        }
    }
}
