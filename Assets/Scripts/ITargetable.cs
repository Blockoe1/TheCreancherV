using UnityEngine;

namespace FoolsBrand
{
    public interface ITargetable
    {
        void TakeDamage(int damage, Combatant source);
    }
}
