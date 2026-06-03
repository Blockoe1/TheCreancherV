using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public class ReturnToMainMenuandPauseMenu : MonoBehaviour
    {
        public GameObject pauseScreen;
        public TMP_Text pauseScreenText;
        private bool isGamePaused;
        private InputAction paused;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            isGamePaused = false;
            pauseScreen.SetActive(false);
            paused.performed += Paused_performed;
        
        }

        private void Paused_performed(InputAction.CallbackContext obj)
        {
            isGamePaused = true;
            if (isGamePaused)
            {
                PausedGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
        public void PausedGame()
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
            Time.timeScale = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
