using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyPoker.Models;
using System;
using System.Threading.Tasks;
using System.Text;

namespace FunnyPoker.src.Controller
{
    public class Game : MonoBehaviour
    {
        private List<GameObject> _players= new List<GameObject>();
        private PlayersCreator _playerCreator;
        private CardDealer _cardDealer;
        private InputManager _inputManager;
        private StringBuilder _bid;
        private CheckManager _checkManager;
        private PlayersUIManager _playersUIManager;

        public bool IsCallBidExecuted { get; set; }
        public int StartingNumOfPlayers { get; private set; }
        public int NumOfCardsToLose { get; private set; }
        public int CurrentPlayer { get; set; }
        public CardsFigure CurrentBid { get; set; }
        public CardsFigure PreviousBid { get; set; }
        

        void Awake()
        {
            _bid = new StringBuilder();
            PreviousBid = new CardsFigure();
            CurrentBid = new CardsFigure();

            _playerCreator = FindObjectOfType<PlayersCreator>();
            _cardDealer = FindObjectOfType<CardDealer>();
            _inputManager = FindObjectOfType<InputManager>();
            _checkManager = FindObjectOfType<CheckManager>();
            _playersUIManager = FindObjectOfType<PlayersUIManager>();

            StartingNumOfPlayers = PlayerPrefsManager.GetNumberOfPlayers();
            NumOfCardsToLose = PlayerPrefsManager.GetLoseCondition();

            _playerCreator.CreatePlayers(_players);
            CurrentPlayer = 1;
        }

        private void Update()
        {
            if (_players.Count > 0)
            {
                var player = _players[CurrentPlayer - 1];
                if (player.GetComponent<Player>().Status == PlayerStatus.Defeated)
                {
                    _playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
                    player.SetActive(false);
                    IncrementCurrentPlayer();
                    _playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

                }
            }
        }

        private void IncrementCurrentPlayer()
        {
            if(CurrentPlayer == StartingNumOfPlayers)
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
            StartCoroutine(_inputManager.OpenInputWindowAndProcessInputData(_bid));
            do
            {
                yield return null;
            } while (!IsCallBidExecuted);
            _playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
            IncrementCurrentPlayer();
            _playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

            IsCallBidExecuted = false;
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
            if (_checkManager.DoesCurrentBidExistsInAllCards(allGameCards, CurrentBid))
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
                _playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);

                DecrementCurrentPlayer();

                while (_players[CurrentPlayer - 1].GetComponent<Player>().Status == PlayerStatus.Defeated)
                {
                    _playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
                    DecrementCurrentPlayer();
                }

                var player = _players[CurrentPlayer - 1].GetComponent<Player>();
                player.NumOfCards++;

                _playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer-1);

                // Check if num of players num of cards exceeds cards to lose
                if (player.NumOfCards >= NumOfCardsToLose)
                {
                    player.NumOfCards = 0;
                    player.Status = PlayerStatus.Defeated;
                }
            }

            // deal new cards
            _cardDealer.DealProperNumOfCardsForAllPlayers();
            // reset currbid and previous bid
            CurrentBid = new CardsFigure();
            PreviousBid = new CardsFigure();
        }

        private void DecrementCurrentPlayer()
        {
            if(CurrentPlayer == 1)
            {
                CurrentPlayer = StartingNumOfPlayers;
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