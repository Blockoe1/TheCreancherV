/*****************************************************************************
// File Name : DiceActionInfo.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Pair an action with it's die face source so that value can be passed to it.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public struct DiceActionInfo
    {
        public DieFace Face {  get; set; }
        public DiceAction Action { get; set; }

        public DiceActionInfo(DieFace face, DiceAction action)
        {
            Face = face;
            Action = action;
        }
    }
}
