using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FoolsBrand
{
    [CreateAssetMenu(fileName = "InvertColorsEffect", menuName = "Scriptable Objects/Effects/Invert Colors")]
    public class InvertColorEffect : Effect
    {
        private IEffectable source;
        private InvertColorToggle invertColorToggle;

        public InvertColorEffect(Effect copy) : base(copy) { }

        public override Effect Copy()
        {
            return new InvertColorEffect(this);
        }

        public override void OnEffectAdded(Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            invertColorToggle = appliedObj.GetComponent<InvertColorToggle>();
            if (invertColorToggle != null)
            {
                invertColorToggle.EnableInvert();
            }
        }

        public override void OnEffectRemoved(Combatant combatant, IEffectable effectSource)
        {
            invertColorToggle.DisableInvert();
        }



    }
}
