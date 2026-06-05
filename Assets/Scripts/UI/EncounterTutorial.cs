/*****************************************************************************
// File Name : EncounterTutorial.cs
// Author : Arcadia Koederitz
// Creation Date : 6/5/2026
// Last Modified : 6/5/2026
//
// Brief Description : Stores data for a tutorial to display.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    [System.Serializable]
    public struct EncounterTutorial
    {
        [field: SerializeField] public AdvanceCondition AdvanceCondition { get; private set; }
        [field: SerializeField, TextArea] public string TutorialText { get; private set; }
    }

    public enum AdvanceCondition
    {
        Click,
        LimbSelected,
        RollPressed,
        ReservePressed
    }
}
