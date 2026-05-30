using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class SelfModifyAttackAction : DiceAction
    {
        [SerializeField] private TempModifyAttackEffect modifyAttackEffect;

        public override int PriorityValue => 100;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user)
        {
            if (source is IEffectable effectable)
            {
                effectable.ApplyEffect(modifyAttackEffect);
            }
            else
            {
                Debug.Log("Modify attack failed");
            }
            yield return null;
        }
    }
}