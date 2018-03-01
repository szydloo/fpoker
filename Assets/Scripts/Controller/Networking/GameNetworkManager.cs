using FunnyPoker.Models;
using FunnyPoker.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyPoker.Scripts.Networking
{
    public class GameNetworkManager
    {
        private static GameNetworkManager instance = new GameNetworkManager();
        public static GameNetworkManager Instance
        {
            get
            {
                return instance;
            }
           
        }

        public static Deck deck = new Deck();

        private int clientId = 1;
        private int currentPlayerId = 1;

        public int ClientId
        {
            get
            {
                return clientId;
            }

            set
            {
                clientId = value;
            }
        }

        public int CurrentPlayerId
        {
            get
            {
                return currentPlayerId;
            }

            set
            {
                currentPlayerId = value;
            }
        }

        public void IncrementCurrentPlayerIdNum()
        {
            if(currentPlayerId == PlayerPrefsManager.GetNumberOfPlayers())
            {
                currentPlayerId = 1;
            }
            else
            {
                currentPlayerId++;
            }
        }

        protected GameNetworkManager()
        {

        }
        
    } 
}
