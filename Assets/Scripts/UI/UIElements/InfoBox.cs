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

        private IDiceInfo currentDice;

        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Mouse.current.position.ReadValue(),
                _myCanvas.worldCamera, out Vector2 pos);

            panel.pivot = new Vector2(pos.x > _myCanvas.pixelRect.width - panel.rect.width ? 1 : 0, pos.y < -_myCanvas.pixelRect.height + panel.rect.height ? 0 : 1);
            transform.position = _myCanvas.transform.TransformPoint(pos);
        }

        public void SetDisplayDice(IDiceInfo die, string suffix = "")
        {
            if (currentDice != null)
            {
                currentDice.HideHoverOutline();
            }

            currentDice = die;
            if (currentDice != null)
            {
                gameObject.SetActive(true);
                _dieNameText.text = currentDice.DieName;
                _dieDescText.text = currentDice.DieDescription + suffix;
                currentDice.ShowHoverOutline();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
