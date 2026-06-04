/*****************************************************************************
// File Name : ApplyEffectAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Inflicts poison on a poisonable target.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "ApplyEffectAction", menuName = "Scriptable Objects/Actions/Apply Effect")]
    public class ApplyEffectAction : DiceAction
    {
        [SerializeField] private Effect effect;
        [SerializeField, Tooltip("If set to true, the action applies the effect to the user.  False for the target.")] 
        private bool targetSelf;
        [SerializeField, Tooltip("Controls the order that actions occur in.  Lower priority goes first.  " +
            "Attack is priority 100")] 
        private int priority = 90;
        public override int PriorityValue => priority;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            if (!targetSelf && target is IEffectable targetEffectable)
            {
                targetEffectable.ApplyEffect(effect);
            }
            if (targetSelf && source is IEffectable selfEffectable)
            {
                selfEffectable.ApplyEffect(effect);
            }
            else
            {
                Debug.Log("Poisoned Failed");
            }
            yield return null;
        }

        protected override void PlayVFX(ITargetable target, IActionSource source, Combatant user, GameObject effectPrefab)
        {
            if (targetSelf)
            {
                GameObject.Instantiate(effectPrefab, user.GetEffectTransform().position, Quaternion.identity);
            }
            else
            {
                GameObject.Instantiate(effectPrefab, target.GetEffectTransform().position, Quaternion.identity);
            }
            
        }
    }
}
