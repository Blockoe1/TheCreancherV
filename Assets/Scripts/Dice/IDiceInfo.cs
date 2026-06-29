/*****************************************************************************
// File Name : IDice.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Interface for viewing dice information fields.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public interface IDiceInfo
    {
        string DieName { get; }
        string DieDescription { get; }
        bool IsReserved { get; }
        bool IsClickable { get; set; }

        void ShowHoverOutline();
        void HideHoverOutline();
    }
}
