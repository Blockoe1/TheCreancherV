using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "TempModifyAttackEffect", menuName = "Scriptable Objects/Effects/Temp Modify Attack")]
    public class TempModifyAttackEffect : Effect
    {
        [SerializeField] private int attackModifier;
        [SerializeField] private ParticleSystem vfx;

        IEffectable Source;

        private  ParticleSystem vfxInstance;

        public TempModifyAttackEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            TempModifyAttackEffect copy = new TempModifyAttackEffect(this);
            copy.attackModifier = attackModifier;
            copy.vfx = vfx;
            return copy;
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            //Debug.Log("Attack modifier added");
            Source = effectSource;
            if (vfx != null)
            {
                vfxInstance = GameObject.Instantiate(vfx, effectSource.GetEffectTransform());
            }
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            //Debug.Log("Attack modifier removed");
            if (vfxInstance != null)
            {
                GameObject.Destroy(vfxInstance);
            }
        }

        public override int ModifyAttack(int dealtDamage)
        {
            markRemove = true;
            return dealtDamage + attackModifier;
        }
    }
}