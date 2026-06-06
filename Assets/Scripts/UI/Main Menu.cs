using FoolsBrand;
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
    }
    public void StartGame()
    {
        RunManager.BeginNewGame(DiceDatabaseSetup.Instance.StartingDice, startingEncounter);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
