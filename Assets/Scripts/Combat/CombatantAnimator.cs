/*****************************************************************************
// File Name : CombatantAnimator.cs
// Author : Arcadia Koederitz
// Creation Date : 6/4/2026
// Last Modified : 6/4/2026
//
// Brief Description : Controls playing animations for combatants.
*****************************************************************************/
using CustomAttributes;
using System;
using UnityEngine;

namespace FoolsBrand
{
    [RequireComponent(typeof(Combatant))]
    public class CombatantAnimator : MonoBehaviour
    {
        private const string HURT_ANIM_NAME = "T_HURT";
        private const string DEATH_ANIM_NAME = "T_DEAD";

        [SerializeField] protected Animator animator;
        [SerializeField, ShowIfNull] private Combatant combatant;

        private void Reset()
        {
            combatant = GetComponent<Combatant>();
        }

        private void Awake()
        {
            combatant.Health.HealthChangedEvent += PlayDamageAnimation;
            combatant.OnDeathEvent.AddListener(PlayDeathAnimation);
        }

        private void OnDestroy()
        {
            combatant.Health.HealthChangedEvent -= PlayDamageAnimation;
            combatant.OnDeathEvent.RemoveListener(PlayDeathAnimation);
        }


        private void PlayDamageAnimation(int healthChange)
        {
            if (healthChange < 0)
            {
                PlayAnimation(HURT_ANIM_NAME);
            }
        }

        private void PlayDeathAnimation()
        {
            PlayAnimation(DEATH_ANIM_NAME);
        }

        /// <summary>
        /// Plays an animation by name and returns the clip played.
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public AnimationInfo PlayAnimation(string animationName)
        {
            if (animator == null || animationName == "") { return null; }
            animator.SetTrigger(animationName);
            animator.Update(0);
            // Makes a few assumptions:
            // 1. The clip we want is on layer  0.
            // 2. The clip is in index 0 in the array
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            return new AnimationInfo(clip, info);
        }

        /// <summary>
        /// Gets the duration of the currently playing animation.
        /// </summary>
        /// <returns></returns>
        public float GetAnimationDuration()
        {
            animator.Update(0);
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }
}
