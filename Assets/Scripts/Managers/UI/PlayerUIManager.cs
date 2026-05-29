/*****************************************************************************
// File Name : PlayerUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/28/2026
// Last Modified : 5/28/2026
//
// Brief Description : Manages all player UI such as health bars.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    public class PlayerUIManager : Manager
    {
        [SerializeField] private HealthBar playerHealthBar;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            playerHealthBar.SetTargetHealth(gm.GetManager<PlayerManager>().Player.Health);
        }
    }
}
