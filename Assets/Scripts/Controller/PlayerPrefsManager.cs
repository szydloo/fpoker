using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FunnyPoker.Scripts.Controller
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        private const string LOSE_CONDITION_KEY = "lose_condition";
        private const string NUMBER_OF_PLAYERS_KEY = "number_of_players";
        private const string NUMBER_OF_STARTING_CARDS = "number_of_starting_cards";

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

        public static void SetNumberOfStartingCards(int numOfStartingCards)
        {
            if (numOfStartingCards >= 1 && numOfStartingCards <= 4 && numOfStartingCards < GetLoseCondition())
            {
                PlayerPrefs.SetInt(NUMBER_OF_STARTING_CARDS, numOfStartingCards);
            }
            else
            {
                Debug.Log("Invalid num of starting cards");
            }
        }

        public static int GetNumberOfStartingCards()
        {
            return PlayerPrefs.GetInt(NUMBER_OF_STARTING_CARDS);
        }
    }
}
