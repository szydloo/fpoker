using System;
using System.Linq;
using System.Collections.Generic;
using FunnyPoker.Models;
using UnityEngine;

namespace FunnyPoker.Scripts.Controller
{
    public class CheckManager : MonoBehaviour
    {
        public bool DoesCurrentBidExistsInAllCards(List<Card> allGameCards, CardsFigure currentBid)
        {
            IEnumerable<Card> cards1 = new List<Card>();
            IEnumerable<Card> cards2 = new List<Card>();
            var valsToCheck = currentBid._cards.GroupBy(x => x.Value).Select(g => g.First()).ToList();
            
            switch (currentBid.Type)
            {
                case HandType.HighCard:
                    cards1 = from c in allGameCards
                             where c.Value == valsToCheck[0].Value
                             select c;
                    if (cards1.ToList().Count < 1) return false;
                    break;

                case HandType.Pair:
                    cards1 = from c in allGameCards
                             where c.Value == valsToCheck[0].Value
                             select c;
                    if (cards1.ToList().Count < 2) return false;
                    break;

                case HandType.TwoPair: // nope
                    cards1 = from c in allGameCards
                             where c.Value == valsToCheck[0].Value
                             select c;
                    cards2 = from c in allGameCards
                             where c.Value == valsToCheck[1].Value
                             select c;
                    if (cards2.Count() < 2 || cards1.Count() < 2) return false;
                    break;

                case HandType.ThreeOfAKind:
                    cards1 = from c in allGameCards
                             where c.Value == currentBid._cards.First().Value
                             select c;
                    if (cards1.ToList().Count < 3) return false;
                    break;

                case HandType.Flush:
                    cards1 = from c in allGameCards
                            where c.Symbol == currentBid.FlushSymbol
                            select c;
                    if (cards1.ToList().Count < 5) return false;
                    break;

                case HandType.Straight:
                    var lowVal = currentBid.LowestCardStraight.Value;

                    // check if lowest val+1 val+2 val+3 val+4 exists
                    switch (lowVal)
                    {
                        case Value.Ace:
                            var x1 = (from c in allGameCards
                                     where c.Value == Value.Two
                                     select c).Count();
                            var x2 = (from c in allGameCards
                                     where c.Value == Value.Three
                                     select c).Count();
                            var x3 = (from c in allGameCards
                                     where c.Value == Value.Four
                                     select c).Count();
                            var x4 = (from c in allGameCards
                                      where c.Value == Value.Five
                                      select c).Count();
                            if (x1 < 1 || x2 < 1 || x3 < 1 || x4 < 1) return false;
                            break;
                        default:
                            var x1a = (from c in allGameCards
                                       where c.Value == lowVal + 1
                                       select c).Count();
                            var x2a = (from c in allGameCards
                                       where c.Value == lowVal + 2
                                      select c).Count();
                            var x3a = (from c in allGameCards
                                       where c.Value == lowVal + 3
                                       select c).Count();
                            var x4a = (from c in allGameCards
                                       where c.Value == lowVal + 4
                                       select c).Count();
                            if (x1a < 1 || x2a < 1 || x3a < 1 || x4a < 1) return false;
                            break;
                    }
                    break;

                case HandType.FullHouse:
                    var set1 = from c in currentBid._cards
                              where c.Value == valsToCheck[0].Value
                              select c;

                    var set2 = from c in currentBid._cards
                                where c.Value == valsToCheck[1].Value
                                select c;

                    Card maxCard;
                    Card minCard;

                    if (set1.Count() < set2.Count())
                    {
                        maxCard = set2.First();
                        minCard = set1.First();
                    }
                    else
                    {
                        maxCard = set1.First();
                        minCard = set2.First();
                    }

                    cards1 = from c in allGameCards
                             where c.Value == maxCard.Value
                             select c;
                    cards2 = from c in allGameCards
                             where c.Value == minCard.Value
                             select c;
                    if (cards2.Count() < 2 || cards1.Count() < 3) return false;
                    break;

                case HandType.FourOfAKind:
                    cards1 = from c in allGameCards
                             where c.Value == valsToCheck[0].Value
                             select c;
                    if (cards1.ToList().Count < 4) return false;
                    break;

                case HandType.StraightFlush: // TODO
                    
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}