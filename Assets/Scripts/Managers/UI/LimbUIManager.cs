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

        public override void Init(GameManager gm)
        {
            enemyManager = gm.EnemyManager;
        }


    }
}
