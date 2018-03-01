using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FunnyPoker.Models
{
    public enum HandType : Int32
    {
        Invalid,
        HighCard, Pair, TwoPair, ThreeOfAKind, Flush, Straight
        ,FullHouse, FourOfAKind, StraightFlush
    }

    [Serializable]
    public class CardsFigure
    {
        public IList<Card> _cards = new List<Card>();

        public HandType Type { get; set; }
        // Lowest card if straight
        public Card LowestCardStraight { get; private set; }
        public Symbol FlushSymbol { get; set; }

        public CardsFigure()
        {
            Type = HandType.Invalid;
        }

        public CardsFigure(params Card[] cards)
        {
            Type = HandType.Invalid;
            SetCards(cards);
        }

        private void SetCards(Card[] cards)
        {
            if (cards.Length > 5 || cards.Length < 0)
            {
                throw new Exception("Invalid number of cards for cards hand.");
            }
            if (!EvaluateHandType(cards))
            {
                throw new Exception("Card set not valid.");
            }
            for (int i = 0; i < cards.Length; i++)
            {
                _cards.Add(cards[i]);
            }
        }

        // Returns false if declared cards are invalid
        private bool EvaluateHandType(IList<Card> cards)
        {
            var groupedBySymbol = cards.GroupBy(x => x.Symbol)
                                      .Select(g => g.First())
                                      .ToList();

            var groupedByVals = cards.GroupBy(x => x.Value)
                                     .Select(g => g.First())
                                     .ToList();

            var numOfDiffSymbs = groupedBySymbol.Count;
            var numOfDiffVals = groupedByVals.Count;

            bool isFlush = false;
            bool isStraight = false;

            if (cards.Count == 1)
            {
                Type = HandType.HighCard;
                return true;
            }
            else if (cards.Count == 2) // Pair
            {
                if (numOfDiffSymbs == 2 && numOfDiffVals == 1)
                {
                    Type = HandType.Pair;
                    return true;
                }
                return false;
            }
            else if (cards.Count == 3) // Three of a kind
            {
                if (numOfDiffSymbs == 3 && numOfDiffVals == 1)
                {
                    Type = HandType.ThreeOfAKind;
                    return true;
                }
                return false;
            }
            else if (cards.Count == 4)
            {

                if ((numOfDiffSymbs == 2 || numOfDiffSymbs == 3) && numOfDiffVals == 2) // Two Pair
                {
                    Type = HandType.TwoPair;
                    return true;
                }
                else if (numOfDiffSymbs == 4 && numOfDiffVals == 1) // Four of kind
                {
                    Type = HandType.FourOfAKind;
                    return true;
                }
                return false;
            }
            else if (cards.Count == 5)
            {

                if ((numOfDiffSymbs == 3 || numOfDiffSymbs == 4) && numOfDiffVals == 2) // FullHouse
                {
                    Type = HandType.FullHouse;
                    return true;
                }
                if (numOfDiffVals == 5 && numOfDiffSymbs == 1) // Flush
                {
                    isFlush = true;
                }
                if (IsStraight(cards)) // Straight
                {
                    isStraight = true;
                }
            }

            // Check for StraightFlush (Poker)
            if (isFlush && isStraight)
            {
                Type = HandType.StraightFlush;
                return true;
            }
            else if (isFlush)
            {
                Type = HandType.Flush;
                return true;
            }
            else if (isStraight)
            {
                Type = HandType.Straight;
                return true;
            }
            return false;
        }

        private bool IsStraight(IList<Card> cards)
        {
            LowestCardStraight = new Card(Symbol.Clubs, Value.Invalid);
            // Look for ace first
            var tempCard = (from c in cards
                            where c.Value == Value.Ace
                            select c).FirstOrDefault();

            // Is Ace case
            if (tempCard != null)
            {
                if (AreFiveOfSameKind(cards, Value.Two))
                {
                    LowestCardStraight = tempCard;
                    return true;
                }
                else
                {
                    if(AreFiveOfSameKind(cards,Value.Ten))
                    {
                        LowestCardStraight = (from c in cards
                                      where c.Value == Value.Ten
                                      select c).FirstOrDefault();
                        return true;
                    }
                    return false;
                }
            } // No Ace Case
            else
            {
                var val = Value.Two;
                while (LowestCardStraight.Value == Value.Invalid)
                {
                    tempCard = (from c in cards
                                where c.Value == val
                                select c).FirstOrDefault();

                    if (tempCard != null)
                    {
                        LowestCardStraight = tempCard;

                        if (AreFiveOfSameKind(cards, val))
                        {
                            return true;
                        }
                        return false;
                    }

                    if (val == Value.Ten) { return false; }
                    val++;
                }
                return false;
            }
        }

        // Look for 4 cards starting from the lowest value 
        private bool AreFiveOfSameKind(IList<Card> cards, Value startingVal)
        {
            int numOfProperCards = 0;
            Card tempCard = new Card(Symbol.Clubs, Value.Invalid);
            for (int i = 0; i < 4; i++, startingVal++)
            {
                tempCard = (from c in cards
                            where c.Value == startingVal
                            select c).FirstOrDefault();

                if (tempCard != null)
                {
                    numOfProperCards++;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder();
            return null;
        }
    }
}