/********************************************************************
// File Name : ShellLimb.cs
// Author : Arcadia Koederitz
// Creation Date : 6/5/2026
// Last Modified : 6/5/2026
//
// Brief Description : Custom limb that boosts the defense of the main body while it's alive.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand.Enemies
{
    public class ShellLimb : Limb
    {
        [SerializeField] private int defenseBoost;
        protected override void LimbStart()
        {
            parentEnemy.Defense += defenseBoost;
        }

        protected override void LimbDestroyed()
        {
            parentEnemy.Defense -= defenseBoost;
        }
    }
}
