/*****************************************************************************
// File Name : LimbDisplay.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Base script for displaying info about a limb on the HUD.
*****************************************************************************/
using FoolsBrand.Enemies;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class LimbDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject targetingButton;
        [Header("Info Fields")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text defenseText;
        [SerializeField] private TMP_Text multiplierText;

        private LimbUIManager manager;
        private int index;

        private Limb currentLimb;

        public void Init(LimbUIManager manager, int index)
        {
            this.manager = manager;
            this.index = index;
        }

        /// <summary>
        /// Button Callback.
        /// </summary>
        public void SelectLimb()
        {
            manager.OnLimbSelected(index);
        }

        public void ToggleTargetingButton(bool enabled)
        {
            targetingButton.SetActive(enabled);
        }

        private void HideDisplay()
        {
            gameObject.SetActive(false);
        }

        public void RefreshDisplay()
        {
            gameObject.SetActive(!currentLimb.IsDead);
            transform.position = Camera.main.WorldToScreenPoint(currentLimb.gameObject.transform.position);
            nameText.text = currentLimb.LimbName;
            defenseText.text = "Defense: " + currentLimb.Defense.ToString();
            multiplierText.text = "Multiplier: " + currentLimb.Multiplier.ToString();
        }

        /// <summary>
        /// Sets the limb that this display shows info for.
        /// </summary>
        /// <param name="limb"></param>
        public void SetLimb(Limb limb)
        {
            if (currentLimb != null)
            {
                // Clean up the last limb.
                currentLimb.OnDestroyEvent.RemoveListener(HideDisplay);
            }

            currentLimb = limb;

            if (currentLimb != null)
            {
                currentLimb.OnDestroyEvent.AddListener(HideDisplay);

                RefreshDisplay();
            }
            else
            {
                HideDisplay();
            }
        }
    }
}
