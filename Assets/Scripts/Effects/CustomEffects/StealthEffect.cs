using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "DistractionEffect", menuName = "Scriptable Objects/Effects/Distraction Effect")]
    public class DistractionEffect : Effect
    {
        [SerializeField] private float dodgeChance;

        public override int ModifyAttack(EffectInstance instance, int dealtDamage)
        {
            Random.Range(0f, 100f);
            if (Random.value < dodgeChance)
            {
                return 0;
            }
            return dealtDamage;
        }
    }
}