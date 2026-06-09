using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public class MainMenuReturnNotForPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;
        void Start()
        {
            _winScreen.SetActive(RunManager.Win);
            _loseScreen.SetActive(!RunManager.Win);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
