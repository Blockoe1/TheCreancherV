/*****************************************************************************
// File Name : UIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Parent manager that controls all child UI scripts.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class UIManager : HierarchyManager
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private Canvas gameCanvas;

        internal static Camera GameCamera { get; private set; }
        internal static Canvas GameCanvas { get; private set; }

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            GameCamera = gameCamera;
            GameCanvas = gameCanvas;
            base.Init(gm, parentManager);
        }
    }
}
