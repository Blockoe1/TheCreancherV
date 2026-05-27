/*****************************************************************************
// File Name : UIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Parent manager that controls all child UI scripts.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class UIManager : Manager
    {
        [SerializeField] private LimbUIManager limbUI;
        public override void Init(GameManager gm)
        {
            limbUI.Init(gm);
        }
    }
}
