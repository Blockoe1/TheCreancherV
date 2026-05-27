/*****************************************************************************
// File Name : Manager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description :  Base class for all managers.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public abstract class Manager : MonoBehaviour
    {
        public virtual void Init(GameManager gm, HierarchyManager parentManager) { }

        public virtual void GameStart() { }
    }
}
