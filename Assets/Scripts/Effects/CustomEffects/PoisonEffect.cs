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
    [CreateAssetMenu(fileName = "PoisonEffect", menuName = "Scriptable Objects/Effects/Poison")]
    public class PoisonEffect : Effect
    {
        [SerializeField] private float tickDelay;
        [SerializeField] private ParticleSystem poisonEffectBurst;

        /// <summary>
        /// Deals damage to the main enemy health.
        /// </summary>
        /// <param name="combatant">The combatant to deal poison damage to.</param>
        public override IEnumerator OnActionStart(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            if (combatant.Health.IsDead)
            {
                yield break;
            }
            SpawnEffectToMeshRenderer(poisonEffectBurst, combatant.transform);
            combatant.Health.Value -= instance.Potency;
            combatant.CheckForDeath();

            yield return new WaitForSeconds(tickDelay);
        }

        public override ParticleSystem SpawnVFX(Transform parentTransform)
        {
            return SpawnEffectToMeshRenderer(visualEffect, parentTransform);
        }

        /// <summary>
        /// Spawns a particle effect bound to the mesh renderer on the parent transform.
        /// </summary>
        /// <param name="particleSystem"></param>
        /// <param name="parentTransform"></param>
        /// <returns></returns>
        private static ParticleSystem SpawnEffectToMeshRenderer(ParticleSystem particleSystem, Transform parentTransform)
        {
            ParticleSystem effectInstance = GameObject.Instantiate(particleSystem, parentTransform);
            MeshRenderer meshRenderer = parentTransform.GetComponentInChildren<MeshRenderer>();
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
