using System;
using System.Collections.Generic;
using FunnyPoker.Models;
using UnityEngine;

namespace FunnyPoker.Scripts.Controller
{
    public class CardDealer : MonoBehaviour
    {
        private Sprite[] sprites;
        private string cardPosName = "CardPos";
        private int reversedCardImgNum = 15;
        private int aceDiamondsNum = 0;
        private int aceHeartsNum = 16;
        private int aceClubsNum = 32;
        private int aceSpadesNum = 48;

        private void Start()
        {
            sprites = Resources.LoadAll<Sprite>("Images\\Cards\\cards");
        }

        private int GetImgIdForCard(Card card)
        {
            int num = default(int);
            switch (card.Symbol)
            {
                case Symbol.Diamonds:
                    num = aceDiamondsNum;
                    break;
                case Symbol.Hearts:
                    num = aceHeartsNum;
                    break;
                case Symbol.Spades:
                    num = aceSpadesNum;
                    break;
                case Symbol.Clubs:
                    num = aceClubsNum;
                    break;
                default:
                    Debug.Log("Invalid symbol.");
                    num = 0;
                    break;
            }
            switch (card.Value)
            {
                case Value.Ace:
                    break;
                default:
                    num += (int)card.Value;
                    break;
            }
            return num;
        }

        public void DealProperNumberOfReversedCards(GameObject player, int numOfCards)
        {
            for (int i = 1; i <= numOfCards; i++)
            {
                var cardPos = player.transform.Find("Cards").transform.Find("CardPos" + i).gameObject;

                var cardSprite = sprites[reversedCardImgNum];

                cardPos.GetComponent<SpriteRenderer>().sprite = cardSprite;
            }
        }

        public void DealProperNumberOfCards(GameObject player, int numOfCards)
        {
            var cards = player.GetComponent<Player>().CurrentCards;
            int i = 1;
            foreach (var c in cards)
            {
                var cardPos = player.transform.Find("Cards").transform.Find("CardPos" + i).gameObject;

                var cardSprite = sprites[GetImgIdForCard(c)];

                cardPos.GetComponent<SpriteRenderer>().sprite = cardSprite;
                i++;
            }
        }


        #region singleplayerCommented
        //private List<GameObject> _players;
        //private Deck _gameDeck;
        //private Sprite[] sprites;
        //private string cardPosName = "CardPos";
        //private int reversedCardImgNum = 15;
        //private int aceDiamondsNum = 0;
        //private int aceHeartsNum = 16;
        //private int aceClubsNum = 32;
        //private int aceSpadesNum = 48;

        //private void Start()
        //{
        //    sprites = Resources.LoadAll<Sprite>("Images\\Cards\\cards");
        //    _gameDeck = new Deck();

        //}

        //public void DealFirstCard(List<GameObject> players)
        //{
        //    _players = players;

        //    foreach (var pObj in _players)
        //    {
        //        var p = pObj.GetComponent<Player>();
        //        var card = _gameDeck.DrawRandomCard();
        //        p.CurrentCards.Add(card);

        //        var cardPosObj = pObj.transform.Find("Cards").Find(cardPosName + 1);

        //        var cardSprite = sprites[GetImgIdForCard(card)];

        //        cardPosObj.gameObject.GetComponent<SpriteRenderer>().sprite = cardSprite;
        //    }
        //}

        //public void RevealCardsForPlayer(int currPlayerId)
        //{
        //    var pObj = _players[currPlayerId - 1];
        //    var player = pObj.GetComponent<Player>();
        //    int i = 1;
        //    foreach (var c in player.CurrentCards)
        //    {
        //        var imgId = GetImgIdForCard(c);
        //        var cardPosObj = pObj.transform.Find("Cards").Find(cardPosName + i);
        //        cardPosObj.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[imgId];
        //    }
        //}

        //public void HideCardsForPlayer(int currPlayerId)
        //{
        //    var pObj = _players[currPlayerId - 1];
        //    var player = pObj.GetComponent<Player>();
        //    int i = 1;
        //    foreach (var c in player.CurrentCards)
        //    {
        //        var cardPosObj = pObj.transform.Find("Cards").Find(cardPosName + i);
        //        cardPosObj.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[reversedCardImgNum];
        //    }
        //}

        //public void RevealCardsForAllPlayers()
        //{
        //    var numOfPlayers = PlayerPrefsManager.GetNumberOfPlayers();
        //    for(int i = 1; i <= numOfPlayers; i++)
        //    {
        //        RevealCardsForPlayer(i);
        //    }
        //}



        //public void DealProperNumOfCardsForAllPlayers()
        //{
        //    // Destroy all other cards
        //    // refil deck
        //    _gameDeck = new Deck();

        //    foreach (var pObj in _players)
        //    {

        //        var p = pObj.GetComponent<Player>();
        //        p.CurrentCards.Clear();
        //        for (int i = 0; i < p.NumOfCards; i++)
        //        {
        //            int j = i + 1;
        //            var card = _gameDeck.DrawRandomCard();
        //            var cardImgId = GetImgIdForCard(card);
        //            p.CurrentCards.Add(card);
        //            var cardPosObj = pObj.transform.Find("Cards").Find(cardPosName + j);
        //            cardPosObj.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[cardImgId];
        //        }
        //    }
        //} 
        #endregion
    }
}