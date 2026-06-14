/*****************************************************************************
// File Name : FullscreenSetting.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Controls the game's fullscreen status.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public class FullscreenSetting : UISetting
    {
        [SerializeField] private Toggle toggle;
        protected override string PlayerPrefsKey => "Fullscreen";

        /// <summary>
        /// Loads the fullscreen toggle from PlayerPrefs and sets it.
        /// </summary>
        public override void Init()
        {
            bool isFullscreen = PlayerPrefs.GetInt(PlayerPrefsKey, 1) > 0;
            ToggleFullscreen(isFullscreen);
            toggle.SetIsOnWithoutNotify(isFullscreen);
        }

        public void ToggleFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt(PlayerPrefsKey, isFullscreen ? 1 : 0);
        }
    }
}
