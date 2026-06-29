using System;
using UnityEngine;

namespace FoolsBrand
{
    public class EntityAnimator : AnimatorEvents
    {
        [Header("Hurt")]
        [SerializeField] private Color _hurtColor = Color.red;
        [SerializeField] private float _toHurtFlashDuration = 0.1f;

        [Header("Cast")]
        [SerializeField] private Color _castColor = Color.white;
        [SerializeField] private float _toCastFlashDuration = 0.1f;

        [SerializeField]
        private ParticleSystem _particles;

        public void ToHurtColorFlash()
        {
            try { ColorChangeAllRegions(_hurtColor, _toHurtFlashDuration); }
            catch (NullReferenceException) {}
        }

        public void ToCastColorFlash()
        {
            try { ColorChangeAllRegions(_castColor, _toCastFlashDuration); }
            catch (NullReferenceException) { }
        }

        public void PlayParticles()
        {
            _particles.Play();
        }
    }
}
