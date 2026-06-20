using UnityEngine;

namespace FoolsBrand
{
    public class CameraAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator cameraAnimator;

        public void PlayCameraClip(string triggerName)
        {
            cameraAnimator.SetTrigger(triggerName);
            Debug.Log(triggerName);
        }
    }
}
