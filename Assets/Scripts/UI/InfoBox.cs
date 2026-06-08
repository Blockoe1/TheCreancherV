using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class InfoBox : MonoBehaviour
    {
        [SerializeField] private Canvas _myCanvas;
        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Mouse.current.position.ReadValue(),
                _myCanvas.GetComponent<Canvas>().worldCamera, out Vector2 pos);
            transform.position = _myCanvas.transform.TransformPoint(pos);
        }
    }
}
