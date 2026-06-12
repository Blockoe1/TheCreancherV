using UnityEngine;

namespace FoolsBrand
{
    public class ReserveDiceUpdater : MonoBehaviour
    {
        [SerializeField] private DieSelectionInfo reserveSelectionInfo;
        private void Awake()
        {
            DiceManager.DiceReservedEvent += HandleDiceReserved;
            DiceManager.DiceChangedEvent += HandleDiceChanged;
        }

        private void OnDestroy()
        {
            DiceManager.DiceReservedEvent -= HandleDiceReserved;
            DiceManager.DiceChangedEvent -= HandleDiceChanged;
        }

        private void HandleDiceReserved(DieBase reservedDice)
        {
            if (reserveSelectionInfo != null)
            {
                reserveSelectionInfo.SetupInfo(reservedDice);
            }
        }

        private void HandleDiceChanged(int diceNum, DieBase changedDice, bool wasAdded)
        {
            if (!wasAdded && changedDice == (object)reserveSelectionInfo.DiceInfo)
            {
                reserveSelectionInfo.SetupInfo(null);
            }
        }
    }
}
