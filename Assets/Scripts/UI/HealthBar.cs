/*****************************************************************************
// File Name : HealthBar.cs
// Author : Arcadia Koederitz
// Creation Date : 5/28/2026
// Last Modified : 5/28/2026
//
// Brief Description : Visualizes a certain HealthData object on the UI.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform barRect;
        private HealthData referenceHealth;

        public void SetTargetHealth(HealthData healthData)
        {
            if (referenceHealth != null)
            {
                referenceHealth.HealthChangedEvent -= UpdateHealthBar;
            }

            referenceHealth = healthData;

            if(referenceHealth != null )
            {
                referenceHealth.HealthChangedEvent += UpdateHealthBar;
                UpdateHealthBar(referenceHealth.Value);
            }
        }

        /// <summary>
        /// Updates the health bar to reflect changes to the health data.
        /// </summary>
        /// <param name="health"></param>
        private void UpdateHealthBar(int health)
        {
            // Add tweening later.
            barRect.anchorMax = new Vector2(referenceHealth.HealthProportion, barRect.anchorMax.y);
        }
    }
}
