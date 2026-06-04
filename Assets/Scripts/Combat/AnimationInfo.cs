/*****************************************************************************
// File Name : AnimationInfo.cs
// Author : Arcadia Koederitz
// Creation Date : 6/4/2026
// Last Modified : 6/4/2026
//
// Brief Description : Package of clip and animator state info from player a combatant animation.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public class AnimationInfo
    {
        public AnimationClip Clip { get; private set; }
        public AnimatorStateInfo StateInfo { get; private set; }

        public AnimationInfo(AnimationClip clip, AnimatorStateInfo stateInfo)
        {
            Clip = clip;
            StateInfo = stateInfo;
        }
    }
}
