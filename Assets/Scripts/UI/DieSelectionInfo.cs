using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoolsBrand
{
    public class DieSelectionInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _infoBox;
        [SerializeField] private TMP_Text _infoBoxName;
        [SerializeField] private TMP_Text _infoBoxDescription;

        private string dieName;
        private string dieDescription;

        public void SetupInfo(string name, string description)
        {
            dieName = name;
            dieDescription = description;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _infoBox.SetActive(true);
            _infoBoxName.text = dieName;
            _infoBoxDescription.text = dieDescription;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _infoBox.SetActive(false);
        }
    }
}
