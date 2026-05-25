using UnityEngine;

/// <summary>
/// Master game manager. Manages the individual managers
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceBag;
    /// <summary>
    /// Initialize the other managers
    /// </summary>
    private void Start()
    {
        diceBag.Init();
    }
}
