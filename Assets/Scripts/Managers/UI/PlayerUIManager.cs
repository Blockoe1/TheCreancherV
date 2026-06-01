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

        private PlayerCombatant player;
        private DamageNumberManager dnm;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            player = gm.GetManager<PlayerManager>().Player;
            playerHealthBar.SetTargetHealth(player.Health);
            dnm = parentManager.GetManager<DamageNumberManager>();

            dnm.RegisterDamageNumber(player.Health, player.transform);
        }

        public override void Deinit()
        {
            dnm.UnregisterDamageNumber(player.Health);
        }
    }
}
