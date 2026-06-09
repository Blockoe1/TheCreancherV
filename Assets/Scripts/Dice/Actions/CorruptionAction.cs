/*****************************************************************************
// File Name : CorruptionAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 6/8/2026
//
// Brief Description : Corrupts a number of the user's dice
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "CorruptionAction", menuName = "Scriptable Objects/Actions/Corruption")]
    public class CorruptionAction : DiceAction
    {
        public override int PriorityValue => 0;

        /// <summary>
        /// Corrupts a random dice
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="user"></param>
        /// <param name="value"></param>
        /// <param name="sourceFace"></param>
        /// <returns></returns>
        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            List<GameObject> allDice = DiceManager.Instance.AllDice;
            List<GameObject> corruptableDice = new();

            foreach (GameObject dice in allDice)
            {
                if (dice.GetComponent<DieBase>().Corrupted)
                {
                    continue;
                }

                corruptableDice.Add(dice);
            }

            GameObject die = corruptableDice[Random.Range(0, corruptableDice.Count)];
            corruptableDice.Remove(die);
            die.GetComponent<DieBase>().Corrupt();

            if(corruptableDice.Count < allDice.Count / 2)
            {
                if(user is PlayerCombatant)
                {
                    user.TakeDamage(999999999, user);
                }
                else if (target is PlayerCombatant)
                {
                    target.TakeDamage(999999999, user);
                }
            }

            yield return null;
        }
    }
}
