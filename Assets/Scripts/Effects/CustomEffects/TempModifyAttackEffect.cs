using UnityEngine;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "TempModifyAttackEffect", menuName = "Scriptable Objects/Effects/Temp Modify Attack")]
    public class TempModifyAttackEffect : Effect
    {
        [SerializeField] private int modifierMultiplier;

        IEffectable Source;

        public override void OnEffectAdded(EffectInstance instance, Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            //Debug.Log("Attack modifier added");
            Source = effectSource;
        }

        public override int ModifyAttack(EffectInstance instance, int dealtDamage)
        {
            instance.MarkRemove = true;
            return dealtDamage + (instance.Potency * modifierMultiplier);
        }
    }
}