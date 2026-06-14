/*****************************************************************************
// File Name : BrightnessSlider.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Controls the game brightness as set by PostProcessing via a slider.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

namespace FoolsBrand.UI
{
    public class BrightnessSlider : SliderSetting
    {
        private const float DEFAULT_BRIGHTNESS = 0f;

        [SerializeField] private VolumeProfile mainVolumeProfile;

        private ColorAdjustments colorAdjustments;

        protected override string PlayerPrefsKey => "Brightness";

        /// <summary>
        /// Load the current brightness from PlayerPrefs;
        /// </summary>
        public override void Init()
        {
            mainVolumeProfile.TryGet(out colorAdjustments);

            float brightness = PlayerPrefs.GetFloat(PlayerPrefsKey, DEFAULT_BRIGHTNESS);
            SetSettingValue(brightness);
            slider.SetValueWithoutNotify(brightness);
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            // Reset brightness in editor as it saves to the asset.
            colorAdjustments.postExposure.value = 0;
        }
#endif

        /// <summary>
        /// Sets the brightness and saves it to PlayerPrefs;
        /// </summary>
        /// <param name="value"></param>
        public override void SetSettingValue(float value)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat(PlayerPrefsKey, value);
        }
    }
}
