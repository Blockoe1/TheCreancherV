using FoolsBrand.UI;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "ShrolmHeadAction", menuName = "Scriptable Objects/Actions/Shrolm/Head")]
    public class ShrolmHeadAction : DiceAction
    {
        public override int PriorityValue => 100;


        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            DiceManager diceManager = DiceManager.Instance;
            diceManager.ClearReserveSlot();
            yield return null;
        }

        protected override void PlayVFX(ITargetable target, IActionSource source, Combatant user, GameObject effectPrefab)
        {
            Transform reservedDice = GameObject.Find("ReservePos").transform;
            GameObject.Instantiate(effectPrefab, reservedDice.position, Quaternion.identity);
        }
    }
}