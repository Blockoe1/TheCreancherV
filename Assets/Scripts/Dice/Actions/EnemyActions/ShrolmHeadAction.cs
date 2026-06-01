using FoolsBrand.UI;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class ShrolmHeadAction : DiceAction
    {
        public override int PriorityValue => 100;

        DiceManager diceManager;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user)
        {
            diceManager = DiceManager.Instance;
            diceManager.ClearReserveSlot();
            yield return null;
        }
    }
}