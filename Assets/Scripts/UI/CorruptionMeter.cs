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
            DiceManager.DiceAddedEvent += HandleDiceAdded;
            DieBase.DiceCorruptedEvent += HandleDiceCorrupted;

            UpdateBar();
        }

        public void Deinit()
        {
            DiceManager.DiceAddedEvent -= HandleDiceAdded;
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
                int corruptionThreshold = DiceManager.Instance.NumDiceHeld / 2 + 1;
                corruptedText.text = corruptedDiceCount + "/" + corruptionThreshold;
                float normalizedProgress = (float)corruptedDiceCount / corruptionThreshold;
                meterFill.anchorMax = new Vector2(normalizedProgress > 0 && minAnchor > 0 ?
                    Mathf.Max(normalizedProgress, minAnchor) : normalizedProgress, meterFill.anchorMax.y);
            }
            else
            {
                corruptionGroup.alpha = 0;
            }

        }

        private void HandleDiceCorrupted(DieBase corruptedDice, bool isCorrupted)
        {
            corruptedDiceCount += isCorrupted ? 1 : -1;
            UpdateBar();
        }

        private void HandleDiceAdded(int diceNum, DieBase addedDice)
        {
            UpdateBar();
        }
    }
}
