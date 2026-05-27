/*****************************************************************************
// File Name : LimbUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Manages HUD UI for visualizing and targeting enemy limbs.
*****************************************************************************/
using FoolsBrand.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class LimbUIManager : Manager
    {
        [SerializeField] private LimbDisplay limbDisplayPrefab;

        private readonly List<LimbDisplay> limbDisplays = new List<LimbDisplay>();

        private EnemyManager enemyManager;

        public static event System.Action<int, int> OnLimbSelectedEvent;

        public override void Init(GameManager gm)
        {
            enemyManager = gm.EnemyManager;
        }

        /// <summary>
        /// Loads the limb displays for a given enemy.
        /// </summary>
        /// <param name="toDisplay">The enemy to display limb info for.</param>
        public void SetDisplays(Enemy toDisplay)
        {
            for(int i = 0; i < toDisplay.Limbs.Count; i++)
            {
                LimbDisplay display = GetDisplay(i);
                display.SetLimb(toDisplay.Limbs[i]);
            }
        }

        /// <summary>
        /// Shows the targeting buttons for the enemy's limbs.
        /// </summary>
        public void ToggleTargeting(bool enabled)
        {
            foreach(LimbDisplay display in limbDisplays)
            {
                display.ToggleTargetingButton(enabled);
            }
        }

        /// <summary>
        /// Hides the display entirely.
        /// </summary>
        public void HideDisplays()
        {
            foreach(LimbDisplay display in limbDisplays)
            {
                gameObject.SetActive(false);
            }
        }

        private LimbDisplay GetDisplay(int index)
        {
            while (index >= limbDisplays.Count)
            {
                CreateLimbDisplay();
            }
            return limbDisplays[index];
        }

        private void CreateLimbDisplay()
        {
            int index = limbDisplays.Count;
            LimbDisplay limbDisplay = Instantiate(limbDisplayPrefab, transform);
            limbDisplay.Init(this, index);
            limbDisplays.Add(limbDisplay);
        }

        /// <summary>
        /// Broadcast to a static event that a limb has been selected.
        /// </summary>
        /// <param name="limbIndex"></param>
        public void OnLimbSelected(int limbIndex)
        {
            // First is enemy index, second is limb index.
            OnLimbSelectedEvent?.Invoke(0, limbIndex);
        }
    }
}
