using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "TempModifyAttackEffect", menuName = "Scriptable Objects/Effects/Temp Modify Attack")]
    public class TempModifyAttackEffect : Effect
    {
        [SerializeField] private int attackModifier;
        [SerializeField] private ParticleSystem vfx;

        IEffectable Source;

        public override void OnEffectAdded(EffectInstance instance, Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            //Debug.Log("Attack modifier added");
            Source = effectSource;
            if (vfx != null)
            {
                vfxInstance = GameObject.Instantiate(vfx, effectSource.GetEffectTransform());
            }
        }

        public override void OnEffectRemoved(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            //Debug.Log("Attack modifier removed");
            if (vfxInstance != null)
            {
                GameObject.Destroy(vfxInstance);
            }
        }

        public override int ModifyAttack(EffectInstance instance, int dealtDamage)
        {
            markRemove = true;
            return dealtDamage + attackModifier;
        }
    }
}