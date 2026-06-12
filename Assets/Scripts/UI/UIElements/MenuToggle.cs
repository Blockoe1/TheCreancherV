/*****************************************************************************
// File Name : MenuToggle.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Uses a CanvasGroup to toggle a menu,
*****************************************************************************/
using CustomAttributes;
using UnityEngine;

namespace FoolsBrand.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuToggle : MonoBehaviour
    {
        [SerializeField, ShowIfNull] private CanvasGroup canvasGroup;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ToggleMenu(bool isEnabled)
        {
            canvasGroup.blocksRaycasts = isEnabled;
            canvasGroup.interactable = isEnabled;
            canvasGroup.alpha = isEnabled ? 1 : 0;
        }
    }
}
