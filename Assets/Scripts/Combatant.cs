/*****************************************************************************
// File Name : Combatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script any entity that can deal and recieve damage in combat.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand
{
    public abstract class Combatant : MonoBehaviour, ITargetable
    {
        [SerializeField] private HealthData health;
        [SerializeField] private UnityEvent onDeathEvent;

        public HealthData Health => health;
        public UnityEvent OnDeathEvent => onDeathEvent;
        
        /// <summary>
        /// Makes this combatant attack a target.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="target"></param>
        public virtual int Attack(int damage, ITargetable target)
        {
            int damageDealt = target.TakeDamage(damage, this);
            return damageDealt;
        }

        /// <summary>
        /// Makes this combatant take damage.
        /// </summary>
        /// <param name="damage"></param>
        public virtual int TakeDamage(int damage, Combatant source)
        {
            if (health.IsDead)
            {
                return 0;
            }

            int preHealth = health.Value;
            health.Value -= damage;
            int damageTaken = preHealth - health.Value;

            CheckForDeath();
            return damageTaken;
        }

        public void CheckForDeath()
        {
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
        public IEnumerator ProcessActions(DiceAction[] actions, ITargetable target)
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
                yield return StartCoroutine(action.PerformAction(target, this));

            }
            yield return null;
        }

        /// <summary>
        /// Called when its this combatant's turn to act.
        /// </summary>
        public abstract IEnumerator Act(Combatant target);

        private void OnDestroy()
        {
            onDeathEvent.RemoveAllListeners();
        }
    }
}
