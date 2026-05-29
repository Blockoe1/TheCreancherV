using UnityEngine;
using System.Collections.Generic;

namespace FoolsBrand
{
    public class DiceDatabaseSetup : MonoBehaviour
    {
        public static DiceDatabaseSetup Instance;

        [SerializeField] private List<GameObject> _dicePrefabs;
        [SerializeField] private List<string> _startingDice;

        public List<string> StartingDice { get => _startingDice; set => _startingDice = value; }

        void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            SetupDiceDictionary();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void QuickSetupInstance()
        {
            SetupDiceDictionary();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetupDiceDictionary()
        {
            DiceDatabase.SetupDiceDict(_dicePrefabs);
        }
    }
}
