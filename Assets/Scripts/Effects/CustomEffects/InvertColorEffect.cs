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
        public bool isInverted;

        public override bool UsesPotency => false;


        public override void OnEffectAdded(EffectInstance instance, Combatant combatant, IEffectable effectSource, GameObject appliedObj)
        {
            invertColorToggle = appliedObj.GetComponent<InvertColorToggle>();
            if (invertColorToggle != null)
            {
                invertColorToggle.EnableInvert();
                isInverted = true;
            }
        }

        public override void OnEffectRemoved(EffectInstance instance, Combatant combatant, IEffectable effectSource)
        {
            invertColorToggle.DisableInvert();
            isInverted = false;
        }
    }
}
