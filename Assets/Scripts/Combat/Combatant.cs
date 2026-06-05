/*****************************************************************************
// File Name : Combatant.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script any entity that can deal and recieve damage in combat.
*****************************************************************************/
using CustomAttributes;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace FoolsBrand
{
    public abstract class Combatant : MonoBehaviour, ITargetable
    {
        [SerializeField] private HealthData health;
        [field: SerializeField] public CombatantAnimator Animator { get; private set; }
        [SerializeField] private UnityEvent onDeathEvent;

        public HealthData Health => health;
        public UnityEvent OnDeathEvent => onDeathEvent;
        public bool IsDead => health.IsDead;
        
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
                OnDeath();
            }
        }

        protected virtual void OnDeath() { }

        /// <summary>
        /// Makes this combatant perform a certain list of combat actions.
        /// </summary>
        /// <param name="actions">The actions to perform.</param>
        public IEnumerator ProcessActions(MinPriorityQueue<DiceActionInfo> actions, IActionSource source, ITargetable target)
        {
            //MinPriorityQueue<DiceAction> sortedActions = new MinPriorityQueue<DiceAction>();
            //foreach(DiceAction action in actions)
            //{
            //    // Need to make sure we re-order the type enum to include the execution order.
            //    sortedActions.Enqueue(action, action.PriorityValue);
            //}

            while(actions.Count > 0)
            {
                // Stop performing actions if either side dies.
                if (target.IsDead || IsDead) { break; }
                DiceActionInfo action = actions.Dequeue();
                yield return StartCoroutine(action.Action.PlayAction(target, source, this, action.Face.FaceValue, action.Face));

            }
        }

        /// <summary>
        /// Called when its this combatant's turn to act.
        /// </summary>
        public abstract IEnumerator Act(Combatant target);

        private void OnDestroy()
        {
            onDeathEvent.RemoveAllListeners();
            OnDeath();
        }

        public AnimationInfo PlayAnimation(string name)
        {
            if (Animator == null) { return null; }
            return Animator.PlayAnimation(name);
        }

        /// <summary>
        /// Effects play at the position of the combatant.
        /// </summary>
        /// <returns></returns>
        public virtual Transform GetEffectTransform()
        {
            return transform;
        }
    }
}
