/*****************************************************************************
// File Name : IEffectable.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : interface for any object that can have effects applied to it.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public interface IEffectable
    {
        void ApplyEffect(Effect toApply);
        void RemoveEffect(string className);
        void FlushEffects();
    }
}
