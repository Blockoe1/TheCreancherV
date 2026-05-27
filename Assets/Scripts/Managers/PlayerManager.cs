using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    /// <summary>
    /// Handles dice, rolling, picking up, etc.
    /// </summary>
    public class PlayerManager : Manager
    {
        [SerializeField] private HealthStruct playerHealth = new();

        public static HealthStruct? PlayerHealth = null;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            PlayerHealth ??= playerHealth;
        }
    }
}
