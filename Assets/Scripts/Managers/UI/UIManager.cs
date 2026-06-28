/*****************************************************************************
// File Name : UIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Parent manager that controls all child UI scripts.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.UI
{
    public class UIManager : HierarchyManager
    {
        [SerializeField] private CanvasGroup[] masterUiGroups;
        [SerializeField] private Camera gameCamera;
        [SerializeField] private Canvas gameCanvas;

        internal static Camera GameCamera { get; private set; }
        internal static Canvas GameCanvas { get; private set; }

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            GameCamera = gameCamera;
            GameCanvas = gameCanvas;
            if (masterUiGroups != null)
            {
                foreach(var group in masterUiGroups)
                {
                    if (group == null) { continue; }
                    group.alpha = 1.0f;
                }
            }
            base.Init(gm, parentManager);

            gm.GetManager<PlayerManager>().Player.OnDeathEvent.AddListener(HideUIOnDeath);
        }

        private void HideUIOnDeath()
        {
            if (masterUiGroups != null)
            {
                foreach (var group in masterUiGroups)
                {
                    if (group == null) { continue; }
                    group.alpha = 0;
                }
            }
        }
    }
}
