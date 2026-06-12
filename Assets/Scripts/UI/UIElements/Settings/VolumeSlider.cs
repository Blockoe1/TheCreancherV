/*****************************************************************************
// File Name : VolumeSlider.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Controls the volume of a specific FMOD bus via a slider setting.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class VolumeSlider : SliderSetting
    {
        [SerializeField] private string busName;
        [SerializeField] private string playerPrefsKey;

        protected override string PlayerPrefsKey => playerPrefsKey;

        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void SetSettingValue(float value)
        {
            throw new NotImplementedException();
        }
    }
}
