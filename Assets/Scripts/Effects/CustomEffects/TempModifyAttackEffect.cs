using UnityEngine;

namespace FoolsBrand
{
    [System.Serializable]
    public class TempModifyAttackEffect : Effect
    {
        [SerializeField] private int attackModifier;

        IEffectable Source;

        private GameObject effectInstance;

        public TempModifyAttackEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            TempModifyAttackEffect copy = new TempModifyAttackEffect(this);
            return copy;
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            Source = effectSource;
            Debug.Log(effectInstance);
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            Debug.Log("Attack modifier removed");
            GameObject.Destroy(effectInstance);
        }

        public override int ModifyAttack(int dealtDamage)
        {
            Source.RemoveEffect(this.GetType().Name);
            return dealtDamage + attackModifier;
        }
    }
}