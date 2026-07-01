/*****************************************************************************
// File Name : CorruptionMeter.cs
// Author : Arcadia Koederitz
// Creation Date : 6/9/2026
// Last Modified : 6/9/2026
//
// Brief Description : Displays the number of corrupt dice in the player's bag.
*****************************************************************************/
using System;
using TMPro;
using UnityEngine;

namespace FoolsBrand
{
    public class CorruptionMeter : MonoBehaviour
    {
        [SerializeField] private CanvasGroup corruptionGroup;
        [SerializeField] private RectTransform meterFill;
        [SerializeField] private TMP_Text corruptedText;
        [SerializeField] private float minAnchor;

        private int corruptedDiceCount;

        public void Init()
        {
            DiceManager.DiceChangedEvent += HandleDiceChanged;
            DieBase.DiceCorruptedEvent += HandleDiceCorrupted;

            // Log all corrupted dice already added on init.
            foreach(GameObject diceGO in DiceManager.Instance.DrawBag)
            {
                DieBase dice = diceGO.GetComponent<DieBase>();
                if (dice.Corrupted)
                {
                    corruptedDiceCount++;
                }
            }

            UpdateBar();
        }

        public void Deinit()
        {
            DiceManager.DiceChangedEvent -= HandleDiceChanged;
            DieBase.DiceCorruptedEvent -= HandleDiceCorrupted;
        }

        /// <summary>
        /// Updates the bar to show the correct number and fill.
        /// </summary>
        public void UpdateBar()
        {
            if (corruptedDiceCount > 0)
            {
                corruptionGroup.alpha = 1;
                corruptionGroup.blocksRaycasts = true;
                corruptionGroup.interactable = true;
                int corruptionThreshold = DiceManager.Instance.NumDiceHeld / 2 + 1;
                corruptedText.text = corruptedDiceCount + "/" + corruptionThreshold;
                float normalizedProgress = (float)corruptedDiceCount / corruptionThreshold;
                meterFill.anchorMax = new Vector2(normalizedProgress > 0 && minAnchor > 0 ?
                    Mathf.Max(normalizedProgress, minAnchor) : normalizedProgress, meterFill.anchorMax.y);
            }
            else
            {
                corruptionGroup.alpha = 0;
                corruptionGroup.blocksRaycasts = false;
                corruptionGroup.interactable = false;
            }

        }

        private void HandleDiceCorrupted(DieBase corruptedDice, bool isCorrupted)
        {
            corruptedDiceCount += isCorrupted ? 1 : -1;
            UpdateBar();
        }

        private void HandleDiceChanged(int diceNum, DieBase changedDice, bool wasAdded)
        {
            Debug.Log(changedDice);
            if (changedDice.Corrupted)
            {
                corruptedDiceCount += wasAdded ? 1 : -1;
            }
            UpdateBar();
        }
    }
}
