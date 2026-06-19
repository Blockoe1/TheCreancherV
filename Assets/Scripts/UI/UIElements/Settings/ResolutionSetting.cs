/*****************************************************************************
// File Name : ResolutionSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Manages a setting that controls the game resolution with a dropdown.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class ResolutionSetting : UISetting
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private static Resolution[] resolutions;

        protected override string PlayerPrefsKey => "Resolution";

        private static Resolution[] Resolutions
        {
            get
            {
                if (resolutions == null)
                {
                    resolutions = Screen.resolutions;

                    // manually filter out duplicates.
                    List<Resolution> resList = new List<Resolution>();
                    foreach (Resolution resolution in resolutions)
                    {
                        if (!resList.Any(x => CompareResolutions(x, resolution)))
                        {
                            resList.Add(resolution);
                        }
                    }
                    resolutions = resList.ToArray();
                }
                return resolutions;
            }
        }

        /// <summary>
        /// Loads all
        /// </summary>
        public override void Init()
        {
            int currentResolution = -1;
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                currentResolution =  PlayerPrefs.GetInt(PlayerPrefsKey);
            }

            if (currentResolution >= Resolutions.Length)
            {
                currentResolution = -1;
            }

            // Load the valid resolutions to the dropdown as options.
            resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>();
            for(int i = 0; i < Resolutions.Length; i++)
            {
                resolutionOptions.Add(Resolutions[i].width + " x " + Resolutions[i].height);

                if (currentResolution == -1 && CompareResolutions(Resolutions[i], Screen.currentResolution))
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
            Resolution resolution = Resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt(PlayerPrefsKey, index);
        }
    }
}
