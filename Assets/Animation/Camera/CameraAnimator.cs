using UnityEngine;

namespace FoolsBrand
{
    public class CameraAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator cameraAnimator;

        //private void Awake()
        //{
        //    cameraAnimator.ResetTrigger("T_HURT");
        //}
        public void PlayCameraClip(string triggerName)
        {
            cameraAnimator.SetTrigger(triggerName);
            //Debug.Log(triggerName);
        }
    }
}
