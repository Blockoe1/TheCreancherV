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
        [SerializeField] private Vector2 infoPadding;

        private IDiceInfo currentDice;

        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Mouse.current.position.ReadValue(),
                _myCanvas.worldCamera, out Vector2 pos);
            panel.pivot = new Vector2(pos.x > _myCanvas.pixelRect.width / 2 - (panel.rect.width + infoPadding.x) ? 1 : 0, 
                pos.y > _myCanvas.pixelRect.height / 2 - (panel.rect.height + infoPadding.y) ? 1 : 0);
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
