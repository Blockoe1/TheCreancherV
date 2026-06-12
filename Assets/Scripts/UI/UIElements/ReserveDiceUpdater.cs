using UnityEngine;

namespace FoolsBrand
{
    public class ReserveDiceUpdater : MonoBehaviour
    {
        [SerializeField] private DieSelectionInfo reserveSelectionInfo;
        private void Awake()
        {
            DiceManager.DiceReservedEvent += HandleDiceReserved;
        }

        private void OnDestroy()
        {
            DiceManager.DiceReservedEvent -= HandleDiceReserved;
        }

        private void HandleDiceReserved(DieBase reservedDice)
        {
            if (reserveSelectionInfo != null)
            {
                reserveSelectionInfo.SetupInfo(reservedDice);
            }
        }
    }
}
