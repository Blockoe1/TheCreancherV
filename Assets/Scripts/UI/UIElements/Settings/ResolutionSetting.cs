/*****************************************************************************
// File Name : ResolutionSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Manages a setting that controls the game resolution with a dropdown.
*****************************************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class ResolutionSetting : UISetting
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private Resolution[] resolutions;

        protected override string PlayerPrefsKey => "Resolution";

        /// <summary>
        /// Loads all
        /// </summary>
        public override void Init()
        {
            resolutions = Screen.resolutions;

            int currentResolution = -1;
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                currentResolution =  PlayerPrefs.GetInt(PlayerPrefsKey);
            }

            if (currentResolution >= resolutions.Length)
            {
                currentResolution = -1;
            }

            // Load the valid resolutions to the dropdown as options.
            resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>();
            for(int i = 0; i < resolutions.Length; i++)
            {
                resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);

                if (currentResolution == -1 && CompareResolutions(resolutions[i], Screen.currentResolution))
                {
                    currentResolution = i;
                }
            }
            resolutionDropdown.AddOptions(resolutionOptions);

            SetResolution(currentResolution);
            resolutionDropdown.SetValueWithoutNotify(currentResolution);
            resolutionDropdown.RefreshShownValue();
        }

        private static bool CompareResolutions(Resolution res1, Resolution res2)
        {
            return res1.width == res2.width && res1.height == res2.height;
        }

        public void SetResolution(int index)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt(PlayerPrefsKey, index);
        }
    }
}
