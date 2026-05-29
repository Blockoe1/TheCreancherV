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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public class LimbDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button targetingButton;
        [SerializeField] private CanvasGroup infoGroup;
        [SerializeField] private RectTransform mainInfoTransform;
        [SerializeField] private HealthBar healthBar;
        [Header("Multiplier")]
        [SerializeField] private LineUI multiplierLine;
        [SerializeField] private Transform multiplierTransform;
        [Header("Info Fields")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text defenseText;
        [SerializeField] private TMP_Text multiplierText;

        private LimbUIManager manager;
        private int index;

        private Limb currentLimb;
        private Transform bodyDisplay;

        public void Init(LimbUIManager manager, int index, Transform bodyDisplay)
        {
            this.manager = manager;
            this.index = index;
            this.bodyDisplay = bodyDisplay;

            infoGroup.alpha = 0;
            targetingButton.interactable = false;
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
            targetingButton.interactable = enabled;
        }

        private void HideDisplay()
        {
            gameObject.SetActive(false);
        }

        public void RefreshDisplay()
        {
            gameObject.SetActive(!currentLimb.IsDead);
            transform.position = Camera.main.WorldToScreenPoint(currentLimb.gameObject.transform.position);
            SetAlignment(CheckScreenSide());
            if (nameText != null)
            {
                nameText.text = currentLimb.LimbName;
            }
            if (defenseText != null)
            {
                defenseText.text = currentLimb.Defense.ToString();
            }
            if (multiplierText != null)
            {
                multiplierText.text = currentLimb.Multiplier.ToString() + "x";
            }

            // Set the multiplier line.
            if (multiplierLine != null)
            {
                multiplierLine.SetPoints(transform.position, bodyDisplay.position);
                if (multiplierTransform != null)
                {
                    multiplierTransform.position = multiplierLine.transform.position;
                }
            }
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
                if (healthBar != null)
                {
                    healthBar.SetTargetHealth(currentLimb.Health);
                }
                RefreshDisplay();
            }
            else
            {
                if (healthBar != null)
                {
                    healthBar.SetTargetHealth(null);
                }
                HideDisplay();
            }
        }

        /// <summary>
        /// Checks which side of the screen the display is on.
        /// </summary>
        /// <returns>True if the display is on the right side.</returns>
        public bool CheckScreenSide()
        {
            return transform.position.x >= Screen.width / 2;
        }

        /// <summary>
        /// Sets the alignment of the display so that the info never overlaps with the multiplier line.
        /// </summary>
        /// <param name="isRightAlign"></param>
        public void SetAlignment(bool isRightAlign)
        {
            if (nameText != null)
            {
                nameText.alignment = isRightAlign ? TextAlignmentOptions.Left : TextAlignmentOptions.Right;
            }

            if (mainInfoTransform != null)
            {
                mainInfoTransform.anchorMin = new Vector2(isRightAlign ? 1 : 0, mainInfoTransform.anchorMin.y);
                mainInfoTransform.anchorMax = new Vector2(isRightAlign ? 1 : 0, mainInfoTransform.anchorMax.y);
                mainInfoTransform.pivot = new Vector2(isRightAlign ? 0 : 1, mainInfoTransform.pivot.y);
            }
        }

        #region Info Showing
        /// <summary>
        /// Only show the information for this limb if the mouse is over it.
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnPointerEnter(PointerEventData eventData)
        {
            infoGroup.alpha = 1;   
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            infoGroup.alpha = 0;
        }
        #endregion
    }
}
