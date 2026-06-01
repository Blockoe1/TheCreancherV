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

        public static void LimbSelected(int limbIndex)
        {
            OnLimbSelectedInput?.Invoke(limbIndex);
        }

        public static void OnRollPressed()
        {
            OnRollButtonPressed?.Invoke();
        }
    }
}
