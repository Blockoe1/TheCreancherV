/*****************************************************************************
// File Name : CorruptionAction.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 6/8/2026
//
// Brief Description : Corrupts a number of the user's dice
*****************************************************************************/
using FoolsBrand.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "CorruptionAction", menuName = "Scriptable Objects/Actions/Corruption")]
    public class CorruptionAction : DiceAction
    {
        [SerializeField] private string corruptionDeathSound;
        [SerializeField] private ParticleSystem corruptionDeathEffects;
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
            die.GetComponent<DieBase>().SetCorruption(true);

            if(corruptableDice.Count < allDice.Count / 2f)
            {
                if(user is PlayerCombatant)
                {
                    DieToCorruption(user);
                }
                else if (target is PlayerCombatant targetPlayer)
                {
                    DieToCorruption(targetPlayer);
                }
            }

            yield return null;
        }

        protected override void PlayVFX(ITargetable target, IActionSource source, Combatant user, GameObject effectPrefab)
        {
            if (user is PlayerCombatant)
            {
                GameObject.Instantiate(effectPrefab, user.GetEffectTransform().position, Quaternion.identity);
            }
            else if (target is PlayerCombatant targetPlayer)
            {
                GameObject.Instantiate(effectPrefab, target.GetEffectTransform().position, Quaternion.identity);
            }
        }

        private void DieToCorruption(Combatant deathTarget)
        {
            AudioManager.Instance.PlayOneShot(corruptionDeathSound);
            Instantiate(corruptionDeathEffects, deathTarget.GetEffectTransform().position, Quaternion.identity);
            deathTarget.TakeDamage(999999999, deathTarget);
        }
    }
}
