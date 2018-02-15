﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FunnyPoker.src.Controller
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        private const string LOSE_CONDITION_KEY = "lose_condition";
        private const string NUMBER_OF_PLAYERS_KEY = "number_of_players";

        public static void SetLoseCondition(int numOfCards)
        {
            if (numOfCards >= 3 && numOfCards <= 8)
            {
                PlayerPrefs.SetInt(LOSE_CONDITION_KEY, numOfCards);
            }
            else
            {
                Debug.Log("Num of cards out of range");
            }
        }

        public static int GetLoseCondition()
        {
            return PlayerPrefs.GetInt(LOSE_CONDITION_KEY);
        }

        public static void SetNumberOfPlayers(int numOfPlayers)
        {
            if(numOfPlayers >= 2 && numOfPlayers <= 4)
            {
                PlayerPrefs.SetInt(NUMBER_OF_PLAYERS_KEY, numOfPlayers);
            }
            else
            {
                Debug.Log("Invalid num of players");
            }
        }

        public static int GetNumberOfPlayers()
        {
            return PlayerPrefs.GetInt(NUMBER_OF_PLAYERS_KEY);
        }
    }
}
