using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyPoker.Models;
using System;
using System.Threading.Tasks;
using System.Text;
using FunnyPoker.Scripts.Networking;

namespace FunnyPoker.Scripts.Controller
{
    public class Game : MonoBehaviour
    {
        private static Game instance;
        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        private List<GameObject> _players = new List<GameObject>();

        public PlayersCreator playersCreator;
        public CardDealer cardDealer;
        public InputManager inputManager;
        public CheckManager checkManager;
        public PlayersUIManager playersUIManager;

        private Client client;
        private StringBuilder bid;

        public bool isCallBidExecuted { get; set; }
        public int startingNumOfPlayers { get; private set; }
        public int NumOfCardsToLose { get; private set; }
        public int CurrentPlayer { get; set; }
        public CardsFigure CurrentBid { get; set; }
        public CardsFigure PreviousBid { get; set; }
        

        void Awake()
        {
            instance = this;
            bid = new StringBuilder();
            PreviousBid = new CardsFigure();
            CurrentBid = new CardsFigure();
            client = FindObjectOfType<Client>();
            
            //playersCreator = FindObjectOfType<PlayersCreator>();
            //cardDealer = FindObjectOfType<CardDealer>();
            //inputManager = FindObjectOfType<InputManager>();
            //checkManager = FindObjectOfType<CheckManager>();
            //playersUIManager = FindObjectOfType<PlayersUIManager>();

            startingNumOfPlayers = PlayerPrefsManager.GetNumberOfPlayers();
            NumOfCardsToLose = PlayerPrefsManager.GetLoseCondition();

            // Single player
            //playersCreator.CreatePlayers(_players); 
            CurrentPlayer = 1;

            // Multi Player
            // Based on connected players
            // GetTheir names
        }

        private void Update()
        {
            if (_players.Count > 0)
            {
                var player = _players[CurrentPlayer - 1];
                if (player.GetComponent<Player>().Status == PlayerStatus.Defeated)
                {
                    playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
                    player.SetActive(false);
                    IncrementCurrentPlayer();
                    playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

                }
            }
        }


        private void IncrementCurrentPlayer()
        {
            if(CurrentPlayer == startingNumOfPlayers)
            {
                CurrentPlayer = 1;
            }
            else
            {
                CurrentPlayer++;
            }
        }

        public void OnCalledBid()
        {
            // Coroutinessss...... 
            StartCoroutine(ExecuteCallBidCoroutine());
        }

        private IEnumerator ExecuteCallBidCoroutine()
        {
            StartCoroutine(inputManager.OpenInputWindowAndProcessInputData(bid));
            do
            {
                yield return null;
            } while (!isCallBidExecuted);
            playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
            IncrementCurrentPlayer();
            playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

            isCallBidExecuted = false;
        }

        public void OnCalledCheck()
        {

            //Check if currentBid okey
            if(CurrentBid.Type == HandType.Invalid)
            {
                return;
            }
            // reveal all cards
            var allGameCards = GetCardsFromEveryPlayer();

            // if in all cards currentBid exists
            if (checkManager.DoesCurrentBidExistsInAllCards(allGameCards, CurrentBid))
            {
                var player = _players[CurrentPlayer - 1].GetComponent<Player>();
                player.NumOfCards++;

                // Check if num of players num of cards exceeds cards to lose
                if(player.NumOfCards >= NumOfCardsToLose)
                {
                    player.NumOfCards = 0;
                    player.Status = PlayerStatus.Defeated;
                }
            }
            else 
            {
                playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);

                DecrementCurrentPlayer();

                while (_players[CurrentPlayer - 1].GetComponent<Player>().Status == PlayerStatus.Defeated)
                {
                    playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
                    DecrementCurrentPlayer();
                }

                var player = _players[CurrentPlayer - 1].GetComponent<Player>();
                player.NumOfCards++;

                playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer-1);

                // Check if num of players num of cards exceeds cards to lose
                if (player.NumOfCards >= NumOfCardsToLose)
                {
                    player.NumOfCards = 0;
                    player.Status = PlayerStatus.Defeated;
                }
            }

            // deal new cards
            cardDealer.DealProperNumOfCardsForAllPlayers();
            // reset currbid and previous bid
            CurrentBid = new CardsFigure();
            PreviousBid = new CardsFigure();
        }

        private void DecrementCurrentPlayer()
        {
            if(CurrentPlayer == 1)
            {
                CurrentPlayer = startingNumOfPlayers;
            }
            else
            {
                CurrentPlayer--;
            }
        }

        private List<Card> GetCardsFromEveryPlayer()
        {
            List<Card> allGameCards = new List<Card>();
            foreach (var pObj in _players)
            {
                var p = pObj.GetComponent<Player>();
                if (p.Status == PlayerStatus.Defeated) continue;
                foreach (var c in p.CurrentCards)
                {
                    allGameCards.Add(c);
                }
            }
            return allGameCards;
        }

   
    }
}