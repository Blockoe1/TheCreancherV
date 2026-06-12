/*****************************************************************************
// File Name : SliderSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Abstract class that serves as the basis for all settings controlled by a slider.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public abstract class SliderSetting : UISetting
    {
        [SerializeField] protected Slider slider;

        public abstract void SetSettingValue(float value);
    }
}
