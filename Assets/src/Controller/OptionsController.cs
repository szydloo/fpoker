using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyPoker.src.Controller
{
    public class OptionsController : MonoBehaviour
    {
        private Dropdown drpNumOfPlayers;
        private Dropdown drpLoseCond;
        private LevelManager levelManager;

        private readonly string NUM_OF_PLAYERS_DROPDOWN_NAME = "drpNumOfPlayers";
        private readonly string LOSE_COND_DROPDOWN_NAME = "drpLoseCond";

        // Use this for initialization
        private void Start()
        {
            drpNumOfPlayers = GameObject.Find(NUM_OF_PLAYERS_DROPDOWN_NAME).GetComponent<Dropdown>();
            drpLoseCond = GameObject.Find(LOSE_COND_DROPDOWN_NAME).GetComponent<Dropdown>();
            levelManager = FindObjectOfType<LevelManager>();
            if(drpNumOfPlayers == null)
            {
                Debug.LogError("Can't find number of players dropdown.");
            }
            if (drpLoseCond == null)
            {
                Debug.LogError("Can't find lose condition dropdown.");
            }
            if(levelManager == null)
            {
                Debug.LogError("Can't find level manager.");
            }

            drpNumOfPlayers.value = PlayerPrefsManager.GetNumberOfPlayers() - 2;
            drpLoseCond.value = PlayerPrefsManager.GetLoseCondition() - 3;
        }

        public void SaveAndQuit()
        {
            PlayerPrefsManager.SetNumberOfPlayers(drpNumOfPlayers.value + 2);
            PlayerPrefsManager.SetLoseCondition(drpLoseCond.value + 3);
            levelManager.LoadLevel("00_MainMenu");
        }
    }
}
