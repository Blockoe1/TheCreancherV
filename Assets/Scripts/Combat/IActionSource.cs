/*****************************************************************************
// File Name : IActionSource.cs
// Author : Arcadia Koederitz
// Creation Date : 5/30/2026
// Last Modified : 5/30/2026
//
// Brief Description : Interface for all objects that are responsible for performing actions in combat.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public interface IActionSource
    {
        AnimationClip PlayAnimation(string animationName);
    }
}
