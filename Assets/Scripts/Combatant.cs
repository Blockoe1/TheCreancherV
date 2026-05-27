/*****************************************************************************
// File Name : Combatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script any entity that can deal and recieve damage in combat.
*****************************************************************************/
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
        public void ProcessActions(List<Action> actions)
        {
            MinPriorityQueue<Action> sortedActions = new MinPriorityQueue<Action>();
            foreach(Action action in actions)
            {
                // Need to make sure we re-order the type enum to include the execution order.
                sortedActions.Enqueue(action, (int)action.Type);
            }

            while(sortedActions.Count > 0)
            {
                // Switch this to inheritance support later.
                Action action = sortedActions.Dequeue();
                switch (action.Type)
                {
                    case Action.ActionTypes.ATTACK:
                        Debug.Log("Dealt " + action.Value.ToString() + " damage.");
                        break;
                    case Action.ActionTypes.HEAL:
                        Debug.Log("Applied " + action.Value.ToString() + " healing to self.");
                        break;
                    case Action.ActionTypes.CORRUPTION:
                        Debug.Log("Applied corruption to " + action.Value.ToString() + " dice.");
                        break;
                    case Action.ActionTypes.POSION:
                        Debug.Log("Applied " + action.Value.ToString() + " poison.");
                        break;
                }

            }
        }

        /// <summary>
        /// Called when its this combatant's turn to act.
        /// </summary>
        public abstract void Act();
    }
}
