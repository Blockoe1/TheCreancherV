/*****************************************************************************
// File Name : PoisonEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Deals continual damage to the applied target every turn.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class PoisonEffect : Effect
    {
        [SerializeField] private int tickDamage;
        [SerializeField] private float tickDelay;
        [SerializeField] private ParticleSystem poisonEffect;
        [SerializeField] private ParticleSystem poisonEffectBurst;

        private ParticleSystem effectInstance;

        public PoisonEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            PoisonEffect copy = new PoisonEffect(this);
            copy.poisonEffect = poisonEffect;
            copy.poisonEffectBurst = poisonEffectBurst;
            copy.tickDamage = tickDamage;
            copy.tickDelay = tickDelay;
            return copy;
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            if (poisonEffect != null)
            {
                SpawnEffectToMeshRenderer(poisonEffect, appliedObj.transform);
            }
            //Debug.Log(effectInstance);
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            //Debug.Log("Poison Removed");
            if (effectInstance != null)
            {
                GameObject.Destroy(effectInstance.gameObject);
            }
        }

        /// <summary>
        /// Deals damage to the main enemy health.
        /// </summary>
        /// <param name="combatant">The combatant to deal poison damage to.</param>
        public override IEnumerator OnActionStart(Combatant combatant, IEffectable effectSource)
        {
            if (combatant.Health.IsDead)
            {
                yield break;
            }
            SpawnEffectToMeshRenderer(poisonEffectBurst, combatant.transform);
            combatant.Health.Value -= tickDamage;
            combatant.CheckForDeath();

            yield return new WaitForSeconds(tickDelay);
        }

        private static ParticleSystem SpawnEffectToMeshRenderer(ParticleSystem particleSystem, Transform transform)
        {
            ParticleSystem effectInstance = GameObject.Instantiate(particleSystem, transform);
            MeshRenderer meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            //Debug.LogError(meshRenderer);
            if (meshRenderer != null)
            {
                var shape = effectInstance.shape;
                shape.meshRenderer = meshRenderer;
            }
            return effectInstance;
        }
    }
}
