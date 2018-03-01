using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyPoker.Models;
using System;
using System.Threading.Tasks;
using System.Text;
using FunnyPoker.Scripts.Networking;
using UnityEngine.UI;

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

        public PlayersCreator PlayersCreator;
        public CardDealer CardDealer;
        public InputManager InputManager;
        public CheckManager CheckManager;
        public PlayersUIManager PlayersUIManager;
        public Text DebugID;
        public GameObject PlayerPrefab;

        private Client client;
        private StringBuilder bid;
        private List<Player> _playersGraphRepresentation;

        // Variables to sync accros players
        public int NumOfCardsToLose { get; set; }
        public int CurrentPlayerId { get; set; }
        public int NumOfPlayers { get; set; }
        public int NumOfStartingCards { get; set; }
        public CardsFigure CurrentBid { get; set; }
        public CardsFigure PreviousBid { get; set; }
        public bool IsCallBidExecuted { get; set; }
        public bool tempFlag { get; set; }

        void Awake()
        {
            instance = this;
            bid = new StringBuilder();
            PreviousBid = new CardsFigure();
            CurrentBid = new CardsFigure();
            _playersGraphRepresentation = new List<Player>();
            CurrentPlayerId = 1;
            client = FindObjectOfType<Client>();
            tempFlag = true;

        }

        private void Start()
        {
            DebugID.text = client.Id.ToString();
            client.Send("CSGM"); // Clients notification to server about started game and 
        }

        public void Update()
        {

            Debug.Log(NumOfPlayers);

        }

        public void SetPlayers()
        {
            for (int i = 1; i <= NumOfPlayers; i++)
            {
                var pos = GameObject.Find("PlayerPos" + i);
                if (pos.transform.childCount != 0) continue;

                if (client.Id == i)
                {
                    client.player = Instantiate(PlayerPrefab);
                    client.player.GetComponent<Player>().Id = i;

                    client.player.transform.SetParent(pos.transform, false);
                    _playersGraphRepresentation.Add(client.player.GetComponent<Player>()); // So we can access other players via indexer Curr
                    
                }
                else
                {
                    if (pos.transform.childCount == 0)
                    {
                        GameObject player = Instantiate(PlayerPrefab);
                        player.transform.SetParent(pos.transform, false);
                        _playersGraphRepresentation.Add(player.GetComponent<Player>());
                    }
                }
            }
            foreach(var p in _playersGraphRepresentation)
            {
                p.NumOfCards = NumOfStartingCards; 
            }
            if(tempFlag)
            {
                EndTurn();
            }
        }


        private void EndTurn()
        {
             client.Send("CENDTURN|" + client.player.GetComponent<Player>().NumOfCards + "|" + client.Id); // Sending msg about endturn with num of current cards and id of client
            // On inc message client will recievie card
            tempFlag = false;
        }

        public void AddCardPicforPlayer(int id, int numOfCards)
        {
            var player = _playersGraphRepresentation[id - 1].gameObject;
            if(id == client.Id)
            {
                CardDealer.DealProperNumberOfCards(player, numOfCards);
            }
            else
            {
                CardDealer.DealProperNumberOfReversedCards(player, numOfCards);
            }
        }

        private void OnCalledBid()
        {
            if (!IsValidPlayersTurn()) return;
            // validation handled on server level
        }

        public void OnCalledCheck()
        {
            if (!IsValidPlayersTurn()) return;

        }

        private bool IsValidPlayersTurn()
        {
            if (client.Id != CurrentPlayerId)
            {
                Debug.Log("It's players " + CurrentPlayerId + " turn, not player " + client.Id);
                return false;
            }
            return true;
        }

        #region singleplayercodePrerework
        //private void Update()
        //{
        //    if (_players.Count > 0)
        //    {
        //        var player = _players[CurrentPlayer - 1];
        //        if (player.GetComponent<Player>().Status == PlayerStatus.Defeated)
        //        {
        //            playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
        //            player.SetActive(false);
        //            IncrementCurrentPlayer();
        //            playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

        //        }
        //    }
        //}


        //private void IncrementCurrentPlayer()
        //{
        //    if(CurrentPlayer == startingNumOfPlayers)
        //    {
        //        CurrentPlayer = 1;
        //    }
        //    else
        //    {
        //        CurrentPlayer++;
        //    }
        //}

        //public void OnCalledBid()
        //{
        //    // Coroutinessss...... 
        //    StartCoroutine(ExecuteCallBidCoroutine());
        //}

        //private IEnumerator ExecuteCallBidCoroutine()
        //{
        //    StartCoroutine(inputManager.OpenInputWindowAndProcessInputData(bid));
        //    do
        //    {
        //        yield return null;
        //    } while (!isCallBidExecuted);
        //    playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
        //    IncrementCurrentPlayer();
        //    playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer - 1);

        //    isCallBidExecuted = false;
        //}

        //public void OnCalledCheck()
        //{

        //    //Check if currentBid okey
        //    if(CurrentBid.Type == HandType.Invalid)
        //    {
        //        return;
        //    }
        //    // reveal all cards
        //    var allGameCards = GetCardsFromEveryPlayer();

        //    // if in all cards currentBid exists
        //    if (checkManager.DoesCurrentBidExistsInAllCards(allGameCards, CurrentBid))
        //    {
        //        var player = _players[CurrentPlayer - 1].GetComponent<Player>();
        //        player.NumOfCards++;

        //        // Check if num of players num of cards exceeds cards to lose
        //        if(player.NumOfCards >= NumOfCardsToLose)
        //        {
        //            player.NumOfCards = 0;
        //            player.Status = PlayerStatus.Defeated;
        //        }
        //    }
        //    else 
        //    {
        //        playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);

        //        DecrementCurrentPlayer();

        //        while (_players[CurrentPlayer - 1].GetComponent<Player>().Status == PlayerStatus.Defeated)
        //        {
        //            playersUIManager.DeactivateIndicatorForPlayer(_players, CurrentPlayer - 1);
        //            DecrementCurrentPlayer();
        //        }

        //        var player = _players[CurrentPlayer - 1].GetComponent<Player>();
        //        player.NumOfCards++;

        //        playersUIManager.ActivateIndicatorForPlayer(_players, CurrentPlayer-1);

        //        // Check if num of players num of cards exceeds cards to lose
        //        if (player.NumOfCards >= NumOfCardsToLose)
        //        {
        //            player.NumOfCards = 0;
        //            player.Status = PlayerStatus.Defeated;
        //        }
        //    }

        //    // deal new cards
        //    cardDealer.DealProperNumOfCardsForAllPlayers();
        //    // reset currbid and previous bid
        //    CurrentBid = new CardsFigure();
        //    PreviousBid = new CardsFigure();
        //}

        //private void DecrementCurrentPlayer()
        //{
        //    if(CurrentPlayer == 1)
        //    {
        //        CurrentPlayer = startingNumOfPlayers;
        //    }
        //    else
        //    {
        //        CurrentPlayer--;
        //    }
        //}

        //private List<Card> GetCardsFromEveryPlayer()
        //{
        //    List<Card> allGameCards = new List<Card>();
        //    foreach (var pObj in _players)
        //    {
        //        var p = pObj.GetComponent<Player>();
        //        if (p.Status == PlayerStatus.Defeated) continue;
        //        foreach (var c in p.CurrentCards)
        //        {
        //            allGameCards.Add(c);
        //        }
        //    }
        //    return allGameCards;
        //}
        #endregion
    }
}