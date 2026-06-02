using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class InfoBox : MonoBehaviour
    {
        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Mouse.current.position.ReadValue(),
                transform.parent.GetComponent<Canvas>().worldCamera, out Vector2 pos);
            transform.position = transform.parent.TransformPoint(pos);
        }
    }
}
