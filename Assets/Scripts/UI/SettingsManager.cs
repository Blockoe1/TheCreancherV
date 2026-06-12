/*****************************************************************************
// File Name : SettingsManager.cs
// Author : Arcadia Koederitz
// Creation Date : 6/10/2026
// Last Modified : 6/10/2026
//
// Brief Description : Controls modifying settings and storing them in PlayerPrefs.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using FoolsBrand.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public class SettingsManager : MonoBehaviour
    {
        #region PlayerPref Keys
        private static string MASTER_VOLUME_KEY = "MasterVolume";
        private static string MUSIC_VOLUME_KEY = "MusicVolume";
        private static string SFX_VOLUME_KEY = "SFXVolume";
        private static string RESOLUTION_KEY = "Resolution";
        private static string BRIGHTNESS_KEY = "Brightness";
        #endregion

        [SerializeField] private UISetting[] settings;

        [SerializeField] private Dropdown resolutionDropdown;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;

        private ColorAdjustments colorAdjustments;

        private void Awake()
        {
            foreach(var setting in settings)
            {
                setting.Init();
            }
        }

        public void Init()
        {
            

            // Get Busses.
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/SFX Bus");
            sfxBus = RuntimeManager.GetBus("bus:/Music Bus");

            // Pull all saved settings from PlayerPrefs.
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Volume
            float masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
            SetMasterVolume(masterVolume);

        }

        #region Settings functions
        #region Volume
        public void SetMasterVolume(float volume)
        {
            SetVolumeInternal(volume, masterBus, MASTER_VOLUME_KEY);
        }
        public void SetMusicVolume(float volume)
        {
            SetVolumeInternal(volume, musicBus, MUSIC_VOLUME_KEY);
        }

        public void SetSFXVolume(float volume)
        {
            SetVolumeInternal(volume, sfxBus, SFX_VOLUME_KEY);
        }

        private void SetVolumeInternal(float volume, Bus bus, string playerPrefsKey)
        {
            bus.setVolume(volume);

            // Save to PlayerPrefs.
            PlayerPrefs.SetFloat(playerPrefsKey, volume);
        }
        #endregion

        public void SetResolution(int resolution)
        {
            
        }

        public void SetBrightness(float brightness)
        {
            colorAdjustments.postExposure.value = brightness;
        }
        #endregion
    }
}
