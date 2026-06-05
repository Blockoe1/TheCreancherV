/*****************************************************************************
// File Name : TutorialSequence.cs
// Author : Arcadia Koederitz
// Creation Date : 6/5/2026
// Last Modified : 6/5/2026
//
// Brief Description : Stores a sequence of tutorials to display.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    [CreateAssetMenu(fileName = "TutorialSequence", menuName = "Scriptable Objects/TutorialSequence")]
    public class TutorialSequence : ScriptableObject
    {
        [field: SerializeField] public EncounterTutorial[] Tutorials { get; private set; }
    }
}
