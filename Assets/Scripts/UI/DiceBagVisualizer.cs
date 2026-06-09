/*****************************************************************************
// File Name : DiceBagVisualizer.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Controls viewing dice information on the pause menu.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    public class DiceBagVisualizer : MonoBehaviour
    {
        [SerializeField] private DiceGridManager diceGrid;

        public void ToggleBag(bool canSee)
        {
            diceGrid.ToggleCamera(canSee);
        }
    }
}
