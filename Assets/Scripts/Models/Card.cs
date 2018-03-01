using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FunnyPoker.Models
{
    public enum Symbol { Hearts, Diamonds, Clubs, Spades }

    public enum Value { Invalid, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    // Serialization via string
    public class Card
    {
        public Symbol Symbol { get; set; }
        public Value Value { get; set; }

        public Card(Symbol symbol, Value value)
        {
            Symbol = symbol;
            Value = value;
        }

        public override string ToString()
        {
            StringBuilder strCard = new StringBuilder("--");
            switch (Value)
            {
                case Value.Two:
                    strCard[0] = '2';
                    break;
                case Value.Three:
                    strCard[0] = '3';
                    break;
                case Value.Four:
                    strCard[0] = '4';
                    break;
                case Value.Five:
                    strCard[0] = '5';
                    break;
                case Value.Six:
                    strCard[0] = '6';
                    break;
                case Value.Seven:
                    strCard[0] = '7';
                    break;
                case Value.Eight:
                    strCard[0] = '8';
                    break;
                case Value.Nine:
                    strCard[0] = '9';
                    break;
                case Value.Ten:
                    strCard[0] = 'X';
                    break;
                case Value.Jack:
                    strCard[0] = 'J';
                    break;
                case Value.Queen:
                    strCard[0] = 'Q';
                    break;
                case Value.King:
                    strCard[0] = 'K';
                    break;
                case Value.Ace:
                    strCard[0] = 'A';
                    break;
                default:
                    break;
            }
            switch (Symbol)
            {
                case Symbol.Spades:
                    strCard[1] = 'S';
                    break;
                case Symbol.Diamonds:
                    strCard[1] = 'D';
                    break;
                case Symbol.Hearts:
                    strCard[1] = 'H';
                    break;
                case Symbol.Clubs:
                    strCard[1] = 'C';
                    break;
            }
            return strCard.ToString();
        }
    }
}
