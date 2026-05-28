/*****************************************************************************
// File Name : UIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Parent manager that controls all child UI scripts.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class UIManager : HierarchyManager
    {
        /// <summary>
        /// Switch this to a different manager this is temporary.
        /// When the roll button gets pressed, call the invoke
        /// </summary>
        public void OnRollPressed()
        {
            PlayerInputManager.OnRollPressed();
        }
    }
}
