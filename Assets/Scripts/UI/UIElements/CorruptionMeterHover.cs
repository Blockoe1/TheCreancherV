using UnityEngine;
using UnityEngine.EventSystems;

public class CorruptionBarHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField]
    private string description =
        "Corruption increases the value of all dice by <b>2</b>.\n" +
        "If more than half of your dice are corrupted, you die.";

    [SerializeField] private CorruptionInfo popup;

    private void Awake()
    {
        if (popup == null)
        {
            popup = GetComponentInParent<CorruptionInfo>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popup == null)
        {
            Debug.LogError("CorruptionBarHover: popup reference is null.");
            return;
        }

        popup.Show("Corruption<sprite name=\"Corruption\">", description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (popup == null)
        {
            Debug.LogError("CorruptionBarHover: popup reference is null.");
            return;
        }

        popup.Hide();
    }
}
