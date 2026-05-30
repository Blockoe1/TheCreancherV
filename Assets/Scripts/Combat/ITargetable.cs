using UnityEngine;

namespace FoolsBrand
{
    public interface ITargetable
    {
        int TakeDamage(int damage, Combatant source);
    }
}
