using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoolsBrand
{
    public class DieSelectionInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InfoBox _infoBox;
        [SerializeField, TextArea] private string suffix;

        private IDiceInfo diceInfo;

        public void SetupInfo(IDiceInfo diceInfo)
        {
            this.diceInfo = diceInfo;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _infoBox.SetDisplayDice(diceInfo, suffix);
            //_infoBox.SetActive(true);
            //_infoBoxName.text = dieName;
            //_infoBoxDescription.text = dieDescription;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //_infoBox.SetActive(false);
            if (_infoBox != null)
            {
                _infoBox.SetDisplayDice(null);
            }
        }
    }
}
