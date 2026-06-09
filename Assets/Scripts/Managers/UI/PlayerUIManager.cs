/*****************************************************************************
// File Name : PlayerUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/28/2026
// Last Modified : 5/28/2026
//
// Brief Description : Manages all player UI such as health bars.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class PlayerUIManager : Manager
    {
        [SerializeField] private HealthBar playerHealthBar;
        [SerializeField] private CorruptionMeter corruptionMeter;
        [SerializeField] private ObjectPool<EffectDisplay> effectDisplayPool;

        private PlayerCombatant player;
        private DamageNumberManager dnm;

        private readonly List<EffectDisplay> currentEffectDisplays = new();

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            player = gm.GetManager<PlayerManager>().Player;
            playerHealthBar.SetTargetHealth(player.Health);
            dnm = parentManager.GetManager<DamageNumberManager>();

            corruptionMeter.Init();

            dnm.RegisterDamageNumber(player.Health, player.DamageNumberPoint);

            player.EffectAppliedEvent += AddNewEffectDisplay;
            player.PlayerActEvent += UpdateDisplays;
        }

        public override void Deinit()
        {
            dnm.UnregisterDamageNumber(player.Health);

            corruptionMeter.Deinit();

            player.EffectAppliedEvent -= AddNewEffectDisplay;
            player.PlayerActEvent -= UpdateDisplays;
        }

        private void UpdateDisplays()
        {
            for (int i = 0; i < currentEffectDisplays.Count; i++)
            {
                if (currentEffectDisplays[i].IsExpired)
                {
                    effectDisplayPool.ReturnObject(currentEffectDisplays[i]);
                    currentEffectDisplays.RemoveAt(i);
                    i--;
                    continue;
                }

                currentEffectDisplays[i].Refresh();
            }
        }

        private void AddNewEffectDisplay(EffectInstance effect)
        {
            EffectDisplay display = effectDisplayPool.GetObject();
            currentEffectDisplays.Add(display);
            display.SetEffect(effect);
        }
    }
}
