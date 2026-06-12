using FoolsBrand.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private MenuToggle pauseScreen;
        [SerializeField] private TMP_Text pauseScreenText;
        [SerializeField] private InputAction paused;

        [SerializeField] private UnityEvent<bool> PauseToggledEvent;

        private bool disablePause;

        private static bool isGamePaused;

        public static bool IsGamePaused
        {
            get => isGamePaused;
            set
            {
                isGamePaused = value;
                //Time.timeScale = isGamePaused ? 0f : 1f;
            }
        }

        void Awake()
        {
            paused.Enable();
            paused.performed += Paused_performed;
            TogglePause(false);
        }

        private void OnDestroy()
        {
            paused.performed -= Paused_performed;
        }

        private void Paused_performed(InputAction.CallbackContext obj)
        {
            if (disablePause) { return; }
            TogglePause(!IsGamePaused);
        }

        /// <summary>
        /// Toggles the pause menu,
        /// </summary>
        /// <param name="isPaused"></param>
        public void TogglePause(bool isPaused)
        {
            pauseScreen.ToggleMenu(isPaused);
            IsGamePaused = isPaused;
            PauseToggledEvent?.Invoke(isPaused);
        }

        #region Buttons
        public void ReturnToMainMenu()
        {
            TogglePause(false);
            RunManager.CombatLose();
        }
        #endregion
    }
}
