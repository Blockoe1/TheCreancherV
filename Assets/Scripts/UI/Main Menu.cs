using FoolsBrand;
using FoolsBrand.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _diceDatabaseReference;
    [SerializeField] private int startingEncounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DiceDatabaseSetup.Instance == null)
        {
            Instantiate(_diceDatabaseReference);
        }

        AudioManager.Instance.SetMusic(MusicType.MainMenu);
    }
    public void StartGame()
    {
        RunManager.BeginNewGame(DiceDatabaseSetup.Instance.StartingDice, startingEncounter);
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credits");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
