/*****************************************************************************
// File Name : DamageNumber.cs
// Author : Arcadia Koederitz
// Creation Date : 5/31/2026
// Last Modified : 5/31/2026
//
// Brief Description : Controls UI damage numbers that pop up when a health bar takes damage.
*****************************************************************************/
using CustomAttributes;
using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private Color positiveColor = Color.green;
        [SerializeField] private Color negativeColor = Color.red;
        [Header("Animation")]
        [SerializeField] private float animationDuration;
        [SerializeField] private Vector2 animOffset;
        [SerializeField] private AnimationCurve xPositionCurve;
        [SerializeField] private AnimationCurve yPositionCurve;
        [SerializeField] private AnimationCurve alphaCurve;
        [SerializeField, ShowIfNull] private TMP_Text text;

        private void Reset()
        {
            text = GetComponent<TMP_Text>();
        }

        /// <summary>
        /// Plays this damage number animation.
        /// </summary>
        /// <param name="amount"></param>
        public void Play(int amount, Vector3 position, Action<DamageNumber> finishedCallback = null)
        {
            text.text = amount.ToString();
            text.color = amount > 0 ? positiveColor : negativeColor;
            transform.position = position;
            StartCoroutine(DamageAnimation(amount, position, finishedCallback));
        }

        private IEnumerator DamageAnimation(int amount, Vector3 position, Action<DamageNumber> finishedCallback)
        {
            float timer = 0;
            while(timer < animationDuration)
            {
                float normalizedTime = timer / animationDuration;
                float xOffset = animOffset.x * xPositionCurve.Evaluate(normalizedTime);
                float yOffset = animOffset.y * yPositionCurve.Evaluate(normalizedTime);
                transform.position = position + new Vector3(xOffset, yOffset);

                text.color = SetAlpha(text.color, alphaCurve.Evaluate(normalizedTime));

                timer += Time.deltaTime;
                yield return null;
            }
            finishedCallback?.Invoke(this);
        }

        private static Color SetAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
