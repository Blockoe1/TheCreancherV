/*****************************************************************************
// File Name : Combatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script any entity that can deal and recieve damage in combat.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand
{
    public abstract class Combatant : MonoBehaviour
    {
        [SerializeField] private HealthStruct health;
        [SerializeField] private UnityEvent onDeathEvent;

        public HealthStruct Health => health;

        /// <summary>
        /// Makes this combatant take damage.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            if (health.IsDead)
            {
                Debug.LogError($"Combatant {name} took damage while dead.");
                return;
            }

            health.Value -= damage;
            if (health.IsDead)
            {
                // Death Handling.
                onDeathEvent?.Invoke();
            }
        }

        /// <summary>
        /// Makes this combatant perform a certain list of combat actions.
        /// </summary>
        /// <param name="actions">The actions to perform.</param>
        public IEnumerator ProcessActions(List<DiceAction> actions)
        {
            MinPriorityQueue<DiceAction> sortedActions = new MinPriorityQueue<DiceAction>();
            foreach(DiceAction action in actions)
            {
                // Need to make sure we re-order the type enum to include the execution order.
                sortedActions.Enqueue(action, action.PriorityValue);
            }

            while(sortedActions.Count > 0)
            {
                // Switch this to inheritance support later.
                DiceAction action = sortedActions.Dequeue();
                action.PerformAction();

            }
            yield return null;
        }

        /// <summary>
        /// Called when its this combatant's turn to act.
        /// </summary>
        public abstract IEnumerator Act();
    }
}
