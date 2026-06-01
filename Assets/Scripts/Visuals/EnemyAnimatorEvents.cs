using System;
using UnityEngine;

namespace FoolsBrand
{
    public class EnemyAnimatorEvents : AnimatorEvents
    {
        [SerializeField] private Color _hurtColor = Color.red;
        [SerializeField] private float _toHurtFlashDuration = 0.1f;

        public void ToHurtColorFlash()
        {
            try
            { ColorChangeAllRegions(_hurtColor, _toHurtFlashDuration); }
            catch (NullReferenceException) {}
        }
    }
}
