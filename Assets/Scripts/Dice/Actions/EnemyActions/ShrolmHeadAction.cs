using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class ShrolmHeadAction : DiceAction
    {
        [SerializeField] private InvertColorEffect invertColorEffect;

        private string eatenDice;
        public override int PriorityValue => 100;

        public override IEnumerator PerformAction(ITargetable target, Combatant user)
        {
            eatenDice = DiceManager.Instance._reservedDie;
            DiceManager.Instance._reservedDie = null;
            yield return null;
        }
    }
}