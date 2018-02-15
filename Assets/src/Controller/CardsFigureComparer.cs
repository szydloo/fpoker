using FunnyPoker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FunnyPoker.src.Controller
{
    public class CardsFigureComparer : MonoBehaviour, IComparer<CardsFigure>
    {
        public int Compare(CardsFigure left, CardsFigure right)
        {
            var leftType = left.Type;
            var rightType = right.Type;
            if(leftType != rightType)
            {
                return leftType - rightType;
            } // Since highcard/pair/threeofakind/fourofakind all have same cards it doesn't matter which one
            else if(leftType == HandType.HighCard)
            {
                return left._cards.First().Value - right._cards.First().Value;
            }
            else if (leftType == HandType.Pair)
            {
                return left._cards.First().Value - right._cards.First().Value;
            }
            else if (leftType == HandType.ThreeOfAKind)
            {
                return left._cards.First().Value - right._cards.First().Value;
            }
            else if (leftType == HandType.FourOfAKind)
            {
                return left._cards.First().Value - right._cards.First().Value;
            }
            else if (leftType == HandType.Flush)
            {
                return 0;
            }
            else if (leftType == HandType.Straight) // Determined by lowest card in a straight
            {
                // check if only left lowest is ace
                if(left.LowestCardStraight.Value == Value.Ace && right.LowestCardStraight.Value != Value.Ace)
                {
                    return -1;
                } // check if only right lowest is ace
                else if (right.LowestCardStraight.Value == Value.Ace && left.LowestCardStraight.Value != Value.Ace)
                {
                    return 1;
                }
                else
                {
                    return left.LowestCardStraight.Value - right.LowestCardStraight.Value;
                }
            }
            else if(leftType == HandType.StraightFlush) 
            {
                return left.LowestCardStraight.Value - right.LowestCardStraight.Value;
            }
            else if(leftType == HandType.FullHouse) 
            {
                // left cards
                // group lefts cards by value (should be 2)
                var leftCardVals = left._cards.GroupBy(x => x.Value).Select(g => g.First()).ToList();
                if(leftCardVals.Count() != 2)
                {
                    throw new Exception("Cannot be more than two different types.");

                }
                // determine the value of a card of count 3 in a list
                var leftThreeCardsVal = left._cards.Count(x => x.Value == leftCardVals[0].Value) == 3 ? leftCardVals[0].Value : leftCardVals[1].Value;
                var leftTwoCardsVal = Value.Invalid;
                if(leftThreeCardsVal == leftCardVals[0].Value)
                {
                    leftTwoCardsVal = leftCardVals[1].Value;
                }
                else
                {
                    leftTwoCardsVal = leftCardVals[0].Value;
                }
                // right cards same as left
                var rightCardVals = right._cards.GroupBy(x => x.Value).Select(g => g.First()).ToList();
                if (rightCardVals.Count() != 2)
                {
                    throw new Exception("Cannot be more than two different types.");
                }
                var rightThreeCardsVal = right._cards.Count(x => x.Value == rightCardVals[0].Value) == 3 ? rightCardVals[0].Value : rightCardVals[1].Value;
                var rightTwoCards = Value.Invalid;
                if (rightThreeCardsVal == leftCardVals[0].Value)
                {
                    rightTwoCards = rightCardVals[1].Value;
                }
                else
                {
                    rightTwoCards = rightCardVals[0].Value;
                }
                // First check which value of cards of 3 in a right/left set is higher
                if(rightThreeCardsVal != leftThreeCardsVal)
                {
                    return leftThreeCardsVal - rightThreeCardsVal;
                }
                else // if 3s are equal check value of 2 in a right/left set
                {
                    return leftTwoCardsVal - leftTwoCardsVal;
                }

            }
            else if(leftType == HandType.TwoPair)
            {
                // Left
                var leftPairsVal = left._cards.GroupBy(x => x.Value).Select(g => g.First()).ToList();
                if (leftPairsVal.Count != 2)
                {
                    throw new Exception("Cannot be more than two different types.");
                }
                Value leftMax, leftMin;
                if (leftPairsVal[0].Value > leftPairsVal[1].Value)
                {
                    leftMax = leftPairsVal[0].Value;
                    leftMin = leftPairsVal[1].Value;
                }
                else
                {
                    leftMax = leftPairsVal[1].Value;
                    leftMin = leftPairsVal[0].Value;
                }

                //Right
                var rightPairsVal = right._cards.GroupBy(x => x.Value).Select(g => g.First()).ToList();
                if (rightPairsVal.Count != 2)
                {
                    throw new Exception("Cannot be more than two different types.");
                }
                Value rightMax, rightMin;
                if (rightPairsVal[0].Value > rightPairsVal[1].Value)
                {
                    rightMax = rightPairsVal[0].Value;
                    rightMin = rightPairsVal[1].Value;
                }
                else
                {
                    rightMax = rightPairsVal[1].Value;
                    rightMin = rightPairsVal[0].Value;
                }
                if(rightMax != leftMax)
                {
                    return leftMax - rightMax;
                }
                else
                {
                    return leftMin - rightMin;
                }
            }

            throw new Exception("Error comparing card figures. " + left.Type + " " + right.Type);
        }
    }
}
