using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoolsBrand
{
    public class MainMenuReturnNotForPauseMenu : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
