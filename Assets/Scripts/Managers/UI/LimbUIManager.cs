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
using UnityEngine.UI;

namespace FoolsBrand.UI
{
    public class LimbUIManager : Manager
    {
        [SerializeField] private LimbDisplay limbDisplayPrefab;
        [SerializeField] private LimbDisplay bodyDisplayPrefab;

        private readonly List<LimbDisplay> limbDisplays = new List<LimbDisplay>();

        private DamageNumberManager dnm;

        private LimbDisplay bodyDisplay;
        private Enemy currentDisplayedEnemy;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            dnm = parentManager.GetManager<DamageNumberManager>();
            EnemyManager.EnemySpawnEvent += SetDisplays;
        }

        public override void Deinit()
        {
            EnemyManager.EnemySpawnEvent -= SetDisplays;
        }

        /// <summary>
        /// Refresh the position of displays after the canvas scaler updates.
        /// </summary>
        private void Start()
        {
            RefreshDisplays();
        }

        /// <summary>
        /// For the first half second after init, refresh the position of all limb displays so they update after canvas scaling.
        /// </summary>
        /// <returns></returns>
        //private IEnumerator ResetTransformCoroutine()
        //{

        //}

        private void OnRectTransformDimensionsChange()
        {
            //Debug.Log("Dimensions Changed");
            RefreshDisplays();
        }

        /// <summary>
        /// Loads the limb displays for a given enemy.
        /// </summary>
        /// <param name="toDisplay">The enemy to display limb info for.</param>
        public void SetDisplays(Enemy toDisplay)
        {
            if (toDisplay != null)
            {
                toDisplay.OnDeathEvent.RemoveListener(HideDisplays);
            }

            currentDisplayedEnemy = toDisplay;

            if (currentDisplayedEnemy != null)
            {
                toDisplay.OnDeathEvent.AddListener(HideDisplays);

                for (int i = 0; i < toDisplay.Limbs.Count; i++)
                {
                    LimbDisplay display = GetDisplay(i);
                    display.SetLimb(toDisplay.Limbs[i]);

                }
                RefreshDisplays();
            }
            
        }

        public void RefreshDisplays()
        {
            foreach(LimbDisplay display in limbDisplays)
            {
                display.RefreshPosition();
                display.RefreshDisplay();
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
            LimbDisplay limbDisplay = Instantiate(index == 0 ?  bodyDisplayPrefab: limbDisplayPrefab, transform);
            if (index == 0)
            {
                bodyDisplay = limbDisplay;
            }
            limbDisplay.Init(this, dnm, index, bodyDisplay.transform);
            limbDisplays.Add(limbDisplay);
        }

        /// <summary>
        /// Broadcast to a static event that a limb has been selected.
        /// </summary>
        /// <param name="limbIndex"></param>
        public void OnLimbSelected(int limbIndex)
        {
            PlayerInputManager.LimbSelected(limbIndex);
        }
    }
}
