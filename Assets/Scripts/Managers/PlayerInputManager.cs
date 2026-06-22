/*****************************************************************************
// File Name : PlayerInputManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Middleman between UI and game logic that handles all player input.
*****************************************************************************/
using System;

namespace FoolsBrand
{
    public static class PlayerInputManager
    {
        public static event Action<int> OnLimbSelectedInput;
        public static event Action OnRollButtonPressed;
        public static event Action<int> OnReserveInput;

        public static bool StopInput => PauseMenu.IsGamePaused || TransitionManager.IsTransitioning;

        public static void LimbSelected(int limbIndex)
        {
            if (StopInput) { return;  }
            OnLimbSelectedInput?.Invoke(limbIndex);
        }

        public static void OnRollPressed()
        {
            if (StopInput) { return; }
            OnRollButtonPressed?.Invoke();
        }

        public static void ReservePressed(int diceIndex)
        {
            if (StopInput) { return; }
            OnReserveInput?.Invoke(diceIndex);
        }
    }
}
