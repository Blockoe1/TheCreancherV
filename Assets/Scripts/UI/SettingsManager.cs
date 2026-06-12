/*****************************************************************************
// File Name : SettingsManager.cs
// Author : Arcadia Koederitz
// Creation Date : 6/10/2026
// Last Modified : 6/10/2026
//
// Brief Description : Controls modifying settings and storing them in PlayerPrefs.
*****************************************************************************/
using FMOD.Studio;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private UISetting[] settings;

        private void Awake()
        {
            foreach(var setting in settings)
            {
                setting.Init();
            }
        }
    }
}
