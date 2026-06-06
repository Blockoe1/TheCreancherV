/*****************************************************************************
// File Name : AddDiceAction.cs
// Author : Arcadia Koederitz
// Creation Date : 6/5/2026
// Last Modified : 6/5/2026
//
// Brief Description : Adds a special dice to the player's bag.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "AddDiceAction", menuName = "Scriptable Objects/Actions/Tweasel/Add Dice")]
    public class AddDiceAction : DiceAction
    {
        [SerializeField] private string diceString;
        public override int PriorityValue => 80;

        public override IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
        {
            DiceManager.Instance.AddDice(diceString);
            yield return null;
        }
    }
}
