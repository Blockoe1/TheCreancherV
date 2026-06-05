/*****************************************************************************
// File Name : HealAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Heals the user based on the result.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "HealAction", menuName = "Scriptable Objects/Actions/Heal")]
    public class HealAction : DiceAction
    {
        public override int PriorityValue => 50;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            Debug.Log($"Healed {user} for {value}");
            user.Health.Value += value;
            yield return null;
        }

        protected override void PlayVFX(ITargetable target, IActionSource source, Combatant user, GameObject effectPrefab)
        {
            GameObject.Instantiate(effectPrefab, user.GetEffectTransform().position, Quaternion.identity);
            
        }
    }
}
