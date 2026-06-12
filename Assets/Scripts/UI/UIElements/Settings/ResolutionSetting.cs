/*****************************************************************************
// File Name : ResolutionSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Manages a setting that controls the game resolution with a dropdown.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    public class ResolutionSetting : UISetting
    {
        protected override string PlayerPrefsKey => "Resolution";

        public override void Init()
        {
            
        }

        public void SetResolution(int index)
        {

        }
    }
}
