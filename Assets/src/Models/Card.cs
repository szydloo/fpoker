using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyPoker.Models
{
    public enum Symbol { Hearts, Diamonds, Clubs, Spades }

    public enum Value { Invalid, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    public class Card
    {
        public Symbol Symbol { get; set; }
        public Value Value { get; set; }

        public Card(Symbol symbol, Value value)
        {
            Value = value;
            Symbol = symbol;
        }
    }
}
