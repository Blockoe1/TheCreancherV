using CustomAttributes;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "BuffClaws", menuName = "Scriptable Objects/Actions/Buff Claws")]
    public class BuffClaws : DiceAction
    {
        [SerializeField, ShowNestedEditor] private Effect effect;
        private bool targetSelf;
        [SerializeField, Tooltip("Controls the order that actions occur in.  Lower priority goes first.  " +
            "Attack is priority 100")]
        private int priority = 90;
        public override int PriorityValue => priority;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            target = GameObject.Find("Claws").GetComponent<ITargetable>();
            if (!targetSelf && target is IEffectable targetEffectable)
            {
                targetEffectable.ApplyEffect(effect, value);
            }
            else if (targetSelf && source is IEffectable selfEffectable)
            {
                selfEffectable.ApplyEffect(effect, value);
            }
            else
            {
                Debug.Log("Apply Effect Failed");
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
