/*****************************************************************************
// File Name : EffectDisplay.cs
// Author : Arcadia Koederitz
// Creation Date : 6/4/2026
// Last Modified : 6/4/2026
//
// Brief Description : Displays information about the effects currently on the player on the HUD.
*****************************************************************************/
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FoolsBrand
{
    public class EffectDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const string POTENCY_TAG = "#potency";
        private const string DURATION_TAG = "#duration";
        private const float X_FLIP_THRESHOLD = 270;

        [SerializeField] private Image effectIcon;
        [SerializeField] private CanvasGroup infoGroup;
        [SerializeField] private Color potencyTextColor;
        [SerializeField] private Color durationTextColor;
        [Header("Text")]
        [SerializeField] private TMP_Text durationText;
        [SerializeField] private TMP_Text potencyText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        private EffectInstance effect;

        public bool IsExpired => effect.IsExpired;

        public void SetEffect(EffectInstance effect)
        {
            this.effect = effect;
            effectIcon.sprite = effect.Effect.Icon;
            durationText.enabled = effect.Effect.HasDuration;
            potencyText.enabled = effect.Effect.UsesPotency;

            // Swap the anchor of the description if the effect is too far to the right.
            RectTransform rTrans = transform as RectTransform;
            SetDescriptionSide(rTrans.anchoredPosition.x > X_FLIP_THRESHOLD);

            Refresh();
            ToggleInfo(false);
        }

        private void SetDescriptionSide(bool isLeft)
        {
            RectTransform descriptionTrans = infoGroup.transform as RectTransform;
            descriptionTrans.anchorMin = new Vector2(isLeft ? 0 : 1, descriptionTrans.anchorMin.y);
            descriptionTrans.anchorMax = new Vector2(isLeft ? 0 : 1, descriptionTrans.anchorMin.y);
            descriptionTrans.pivot = new Vector2(isLeft ? 1 : 0, descriptionTrans.pivot.y);
        }

        /// <summary>
        /// Uopdates the potency and duration values of this effect.
        /// </summary>
        public void Refresh()
        {
            nameText.text = effect.Effect.name;
            durationText.text = effect.Duration.ToString();
            potencyText.text = effect.Potency.ToString();

            descriptionText.text = ParseDescription(effect.Effect.Description, effect.Potency, effect.Duration);
        }

        private string ParseDescription(string descriptionString, int potency, int duration)
        {
            descriptionString = descriptionString.Replace(POTENCY_TAG, $"<color=#{potencyTextColor.ToHexString()}>" + potency.ToString() + "</color>");
            descriptionString = descriptionString.Replace(DURATION_TAG, $"<color=#{durationTextColor.ToHexString()}>" + duration.ToString() + "</color>");
            return descriptionString;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ToggleInfo(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToggleInfo(false);
        }

        private void ToggleInfo(bool showInfo)
        {
            infoGroup.alpha = showInfo ? 1 : 0;
        }
    }
}
