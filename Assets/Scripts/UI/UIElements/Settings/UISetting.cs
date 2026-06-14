/*****************************************************************************
// File Name : ResolutionSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Abstract class for all settings managed by the UI.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    public abstract class UISetting : MonoBehaviour
    {
        protected abstract string PlayerPrefsKey { get; }

        public abstract void Init();
    }
}
