using FoolsBrand.UI;
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

        public static bool HideTooltips { get; set; }

        //private void Awake()
        //{
        //    UpdateBoxPosition();
        //    Debug.LogError($"Screen: {Screen.width}.  Position: {transform.position}.  Rect:{panel.rect.width}");
        //}

        void Update()
        {
            UpdateBoxPosition();
        }

        private void UpdateBoxPosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Mouse.current.position.ReadValue(),
                _myCanvas.worldCamera, out Vector2 pos);
            pos = _myCanvas.transform.TransformPoint(pos);

            // This needs to be here for some reason.
            Debug.Log(null);

            Vector2 canvasDimensions = _myCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                new Vector2(_myCanvas.pixelRect.width, _myCanvas.pixelRect.height) : 
                new Vector2(_myCanvas.pixelRect.width / 2, _myCanvas.pixelRect.height / 2);
            Rect trueRect = new Rect(panel.rect.min, panel.rect.size * _myCanvas.scaleFactor);
            Debug.Log($"Screen: {_myCanvas.pixelRect.width}.  Canvas Dimensions: {canvasDimensions}.  Rect: {trueRect}.  Position: {pos}.  Canvas Type: {_myCanvas.renderMode}");
            panel.pivot = new Vector2(pos.x > canvasDimensions.x - (trueRect.width + infoPadding.x) ? 1 : 0,
                pos.y > canvasDimensions.y - (trueRect.height + infoPadding.y) ? 1 : 0);
            transform.position = pos;
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
                if (HideTooltips)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                    _dieNameText.text = currentDice.DieName;
                    _dieDescText.text = currentDice.DieDescription + suffix;
                }
                currentDice.ShowHoverOutline();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
