using UnityEngine;

namespace FoolsBrand
{
    public class Spectator : MonoBehaviour
    {
        private Animator[] animators;
        void Start()
        {
            animators = GetComponentsInChildren<Animator>();
        }

        public void ExciteSpectators()
        {
            foreach (Animator animator in animators)
            {
                animator.SetTrigger("T_EXCITED");
            }
        }

        public void ShockSpectators()
        {
            foreach (Animator animator in animators)
            {
                animator.SetTrigger("T_SHOCKED");
            }
        }
    }
}
