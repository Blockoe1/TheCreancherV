using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorruptionInfo : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private Vector2 infoPadding;

    private bool isVisible;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isVisible) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Mouse.current.position.ReadValue(),
            canvas.worldCamera,
            out Vector2 pos);
        pos = canvas.transform.TransformPoint(pos);

        Vector2 canvasDimensions = canvas.renderMode == RenderMode.ScreenSpaceOverlay ?
                new Vector2(canvas.pixelRect.width, canvas.pixelRect.height) :
                new Vector2(canvas.pixelRect.width / 2, canvas.pixelRect.height / 2);
        Rect trueRect = new Rect(panel.rect.min, panel.rect.size * canvas.scaleFactor);
        panel.pivot = new Vector2(pos.x > canvasDimensions.x - (trueRect.width + infoPadding.x) ? 1 : 0,
            pos.y > canvasDimensions.y - (trueRect.height + infoPadding.y) ? 1 : 0);

        transform.position = pos;
    }

    public void Show(string title, string description)
    {
        titleText.text = title;
        descText.text = description;

        isVisible = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }
}
