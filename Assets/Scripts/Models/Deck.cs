using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyPoker.Models
{
    public class Deck 
    {
        public IList<Card> _cards = new List<Card>();

        public Deck()
        {
            Initialize();
        }

        private void Initialize()
        {
            Symbol symbol = Symbol.Clubs;
            for (int i = 0; i < 4; i++)
            {
                Value value = Value.Two;

                if (i == 1) { symbol = Symbol.Hearts; }
                else if (i == 2) { symbol = Symbol.Diamonds; }
                else if (i == 3) { symbol = Symbol.Spades; }

                for (int j = 0; j < 13; j++)
                {
                    _cards.Add(new Card(symbol, value));
                    value++;
                }
            }
        }

        public Card DrawRandomCard()
        {
            var index = UnityEngine.Random.Range(1, _cards.Count);
            var card = _cards[index];
            _cards.Remove(card);
            return card;
        }
    }

}