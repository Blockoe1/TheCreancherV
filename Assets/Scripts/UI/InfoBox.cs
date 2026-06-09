using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class InfoBox : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private Canvas _myCanvas;
        [SerializeField] private TMP_Text _dieNameText;
        [SerializeField] private TMP_Text _dieDescText;
        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Mouse.current.position.ReadValue(),
                _myCanvas.worldCamera, out Vector2 pos);

            panel.pivot = new Vector2(pos.x > _myCanvas.pixelRect.width / 2 - panel.rect.width ? 1 : 0, pos.y < -_myCanvas.pixelRect.height / 2 + panel.rect.height ? 0 : 1);
            transform.position = _myCanvas.transform.TransformPoint(pos);
        }

        public void SetDisplayDice(IDiceInfo die, bool canReserve = false)
        {
            if (die != null)
            {
                gameObject.SetActive(true);
                _dieNameText.text = die.DieName;
                _dieDescText.text = die.DieDescription +
                    (canReserve && !die.IsReserved && die is DieBase ? "\n\n<i>Click to reserve.</i>" : "");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
