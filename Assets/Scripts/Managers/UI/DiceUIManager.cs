/*****************************************************************************
// File Name : DiceUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : UI amanger for dice reserving and rolling.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceUIManager : Manager
    {
        [SerializeField] private CanvasGroup reserveButtons;
        [SerializeField] private CanvasGroup rollButton;
        [SerializeField, Range(0, 1)] private float disabledAlpha;

        public void ToggleReserveButtons(bool isVisible)
        {
            ToggleGroup(reserveButtons, isVisible);
        }

        public void ToggleRollButton(bool isVisible)
        {
            ToggleGroup(rollButton, isVisible);
        }

        private void ToggleGroup(CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : disabledAlpha;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }

        /// <summary>
        /// Switch this to a different manager this is temporary.
        /// When the roll button gets pressed, call the invoke
        /// </summary>
        public void OnRollPressed()
        {
            PlayerInputManager.OnRollPressed();
        }

        public void OnReservePressed(int index)
        {
            PlayerInputManager.ReservePressed(index);
        }
    }
}
