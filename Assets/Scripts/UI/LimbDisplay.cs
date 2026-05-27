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

        /// <summary>
        /// Sets the limb that this display shows info for.
        /// </summary>
        /// <param name="limb"></param>
        public void SetLimb(Limb limb)
        {
            transform.position = Camera.main.WorldToScreenPoint(limb.gameObject.transform.position);
            nameText.text = limb.LimbName;
            defenseText.text = "Defense: " + limb.Defense.ToString();
            multiplierText.text = "Multiplier: " + limb.Multiplier.ToString();
        }
    }
}
