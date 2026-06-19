/*****************************************************************************
// File Name : LimbDisplay.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Base script for displaying info about a limb on the HUD.
*****************************************************************************/
using FoolsBrand.Enemies;
using NaughtyAttributes;
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
        [SerializeField] private Image multiplier;
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private Sprite weakPointIcon;
        [Header("Info Fields")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text defenseText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private TMP_Text limbDescriptionText;

        private LimbUIManager manager;
        private DamageNumberManager dnm;
        private int index;

        private Limb currentLimb;
        private Transform bodyDisplay;

        public void Init(LimbUIManager manager, DamageNumberManager dnm, int index, Transform bodyDisplay)
        {
            this.manager = manager;
            this.dnm = dnm;
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

        private void OnLimbDestroyed()
        {
            gameObject.SetActive(false);
        }

        public void RefreshDisplay()
        {
            gameObject.SetActive(!currentLimb.IsDead);
            
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
            if (limbDescriptionText != null)
            {
                limbDescriptionText.text = currentLimb.Description;
            }

            // Set the multiplier line.
            if (multiplierLine != null)
            {
                multiplierLine.SetPoints(transform.position, bodyDisplay.position);
                if (multiplier != null)
                {
                    multiplier.transform.position = multiplierLine.transform.position;
                    multiplier.sprite = currentLimb.Multiplier > 1 ? weakPointIcon : defaultIcon;
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
                currentLimb.OnDestroyEvent.RemoveListener(OnLimbDestroyed);
                dnm.UnregisterDamageNumber(currentLimb.Health);
            }

            currentLimb = limb;
            
            if (currentLimb != null)
            {
                currentLimb.OnDestroyEvent.AddListener(OnLimbDestroyed);
                if (healthBar != null)
                {
                    healthBar.SetTargetHealth(currentLimb.Health);
                }
                dnm.RegisterDamageNumber(currentLimb.Health, currentLimb.transform);

                RefreshPosition();
                RefreshDisplay();
            }
            else
            {
                if (healthBar != null)
                {
                    healthBar.SetTargetHealth(null);
                }
                OnLimbDestroyed();
            }
        }

        [Button]
        public void RefreshPosition()
        {
            transform.position = UIManager.GameCamera.WorldToScreenPoint(currentLimb.gameObject.transform.position);
            SetAlignment(CheckScreenSide());
        }

        /// <summary>
        /// Checks which side of the screen the display is on.
        /// </summary>
        /// <returns>True if the display is on the right side.</returns>
        public bool CheckScreenSide()
        {
            Debug.Log($"Screen: {UIManager.GameCanvas.pixelRect.width}.  Position: {transform.position}.");
            return transform.position.x >= UIManager.GameCanvas.pixelRect.width / 2;
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
            // Always render the selected limb on top.
            transform.SetAsLastSibling();
            RefreshDisplay();
            infoGroup.alpha = 1;   
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            infoGroup.alpha = 0;
        }
        #endregion
    }
}
