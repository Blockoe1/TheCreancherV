/*****************************************************************************
// File Name : VolumeSlider.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Controls the volume of a specific FMOD bus via a slider setting.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class VolumeSlider : SliderSetting
    {
        private const float DEFAULT_VOLUME = 1f;

        [SerializeField, Tooltip("Set to blank for the master bus.")] private string busName;
        [SerializeField] private string playerPrefsKey;

        private Bus controlledBus;

        protected override string PlayerPrefsKey => playerPrefsKey;

        /// <summary>
        /// Load the appropriate volume setting from PlayerPrefs
        /// </summary>
        public override void Init()
        {
            try
            {
                controlledBus = RuntimeManager.GetBus("bus:/" + busName);
            }
            catch(BusNotFoundException)
            {
                Debug.LogWarning($"No bus called {busName} exists.  Disabling {name}");
                slider.interactable = false;
                return;
            }

            float volume = PlayerPrefs.GetFloat(PlayerPrefsKey, DEFAULT_VOLUME) * slider.maxValue;
            SetSettingValue(volume);
            slider.SetValueWithoutNotify(volume);
        }

        /// <summary>
        /// Sets the volume setting and saves it to PlayerPrefs;
        /// </summary>
        /// <param name="value"></param>
        public override void SetSettingValue(float value)
        {
            // Automatically normalize the value from the slider as FMOD busses use volume from 0-1;
            float volume = value / slider.maxValue;

            controlledBus.setVolume(volume);
            PlayerPrefs.SetFloat(PlayerPrefsKey, volume);
        }
    }
}
