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

        panel.pivot = new Vector2(
            pos.x > canvas.pixelRect.width / 2 - panel.rect.width ? 1 : 0,
            pos.y < -canvas.pixelRect.height / 2 + panel.rect.height ? 0 : 1);

        transform.position = canvas.transform.TransformPoint(pos);
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
