/*****************************************************************************
// File Name : ICombatantInitialized.cs
// Author : Arcadia Koederitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description :Interface for any component that is initialized by a combatant.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public interface ICombatantInitialized
    {
        void Init(Combatant combatant);
        void Deinit() { }
    }
}
