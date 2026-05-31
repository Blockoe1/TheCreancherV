using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class ShrolmEyeAction : DiceAction
    {
        [SerializeField] private InvertColorEffect invertColorEffect;

        public override int PriorityValue => 100;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user)
        {
            if (target is IEffectable effectable)
            {
                effectable.ApplyEffect(invertColorEffect);
            }
            else
            {
                Debug.Log("Modify attack failed");
            }
            yield return null;
        }
    }
}