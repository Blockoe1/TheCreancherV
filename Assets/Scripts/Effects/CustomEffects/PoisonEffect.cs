/*****************************************************************************
// File Name : PoisonEffect.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Deals continual damage to the applied target every turn.
*****************************************************************************/
using FoolsBrand.Audio;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "PoisonEffect", menuName = "Scriptable Objects/Effects/Poison")]
    public class PoisonEffect : Effect
    {
        [SerializeField] private float tickDelay;
        [SerializeField] private ParticleSystem poisonEffectBurst;
        [SerializeField] private string poisonDamageSoundName;

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
            SpawnEffectToMeshRenderer(poisonEffectBurst, combatant.transform, effectSource.GetEffectMesh());
            combatant.Health.Value -= instance.Potency;
            AudioManager.Instance.PlayOneShot(poisonDamageSoundName);
            combatant.CheckForDeath();
            yield return new WaitForSeconds(tickDelay);
        }

        public override ParticleSystem SpawnVFX(Transform parentTransform, IEffectable effectSource)
        {
            return SpawnEffectToMeshRenderer(visualEffect, parentTransform, effectSource.GetEffectMesh());
        }

        /// <summary>
        /// Spawns a particle effect bound to the mesh renderer on the parent transform.
        /// </summary>
        /// <param name="particleSystem"></param>
        /// <param name="parentTransform"></param>
        /// <returns></returns>
        private static ParticleSystem SpawnEffectToMeshRenderer(ParticleSystem particleSystem, 
            Transform parentTransform, MeshRenderer meshRenderer)
        {
            ParticleSystem effectInstance = GameObject.Instantiate(particleSystem, parentTransform);
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
