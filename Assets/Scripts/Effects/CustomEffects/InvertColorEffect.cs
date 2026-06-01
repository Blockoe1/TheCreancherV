using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FoolsBrand
{
    [System.Serializable]
    public class InvertColorEffect : Effect
    {
        private IEffectable source;

        public InvertColorEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            return new InvertColorEffect(this);
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            InvertColorToggle.EnableInvert();
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            InvertColorToggle.DisableInvert();
        }



    }
}
