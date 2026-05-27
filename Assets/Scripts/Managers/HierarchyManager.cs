/*****************************************************************************
// File Name : HierarchyManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/27/2026
// Last Modified : 5/27/2026
//
// Brief Description : Abstract class for managers that manage other managers.
*****************************************************************************/
using System;
using UnityEngine;

namespace FoolsBrand
{
    public abstract class HierarchyManager : Manager
    {
        [SerializeField] private Manager[] managers;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            foreach (Manager manager in managers)
            {
                manager.Init(gm, this);
            }
        }

        public override void GameStart()
        {
            foreach (Manager manager in managers)
            {
                manager.GameStart();
            }
        }

        /// <summary>
        /// Gets a manager object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetManager<T>() where T : Manager
        {
            return (T)Array.Find(managers, x => x is T);
        }
    }
}
