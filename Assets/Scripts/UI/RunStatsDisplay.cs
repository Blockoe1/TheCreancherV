/*****************************************************************************
// File Name : RunStatsDisplay.cs
// Author : Arcadia Koederitz
// Creation Date : 6/28/2026
// Last Modified : 6/28/2026
//
// Brief Description : Displays run stats on UI.
*****************************************************************************/
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

namespace FoolsBrand.UI
{
    public class RunStatsDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] replaceTexts;
        [Header("Tags")]
        [SerializeField] private string enemyNameTag = "<enemyName>";
        [SerializeField] private string enemiesKilledTag = "<enemiesKilled>";
        [SerializeField] private string totalDiceAmountTag = "<totalDiceAmount>";
        [SerializeField] private string favoredDiceTag = "<favoredDice>";
        private void Awake()
        {
            if (RunManager.RunStats == null) { return; }

            foreach(var replaceText in  replaceTexts)
            {
                string statsString = replaceText.text;
                statsString = statsString.Replace(enemyNameTag, RunManager.RunStats.LastEnemy);
                statsString = statsString.Replace(enemiesKilledTag, RunManager.RunStats.EnemiesKilled.ToString());
                statsString = statsString.Replace(totalDiceAmountTag, RunManager.RunStats.TotalDice.ToString());
                statsString = statsString.Replace(favoredDiceTag, RunManager.RunStats.FavoredDice);
                replaceText.text = statsString;
            }
        }
    }
}
