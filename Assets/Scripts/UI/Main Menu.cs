using FoolsBrand;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _diceDatabaseReference;
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
        RunManager.BeginNewGame(DiceDatabaseSetup.Instance.StartingDice);
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
