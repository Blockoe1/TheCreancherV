/*****************************************************************************
// File Name : ActionVFX.cs
// Author : Arcadia Koederitz
// Creation Date : 5/31/2026
// Last Modified : 5/31/2026
//
// Brief Description : Data container for pairing a visual particle effect with other data that an action needs to
// play it.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "ActionVFX", menuName = "Scriptable Objects/ActionVFX")]
    public class ActionVFX : ScriptableObject
    {
        [field: SerializeField] public float PreloadTime { get; private set; }
        [field: SerializeField] public GameObject EffectObj {  get; private set; }
    }
}
