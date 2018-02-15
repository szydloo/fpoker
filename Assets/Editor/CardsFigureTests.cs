using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using FunnyPoker.Models;

[TestFixture]
public class CardsFigureTests
{
    public Card Card1 { get; set; }
    public Card Card2 { get; set; }
    public Card Card3 { get; set; }
    public Card Card4 { get; set; }
    public Card Card5 { get; set; }

    [Test]
    public void T01_declaring_pair_should_give_handtype_pair()
    {
        CardsFigure cardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Diamonds, Value.Ace));
        Assert.AreEqual(cardsFigure.Type, HandType.Pair);
    }

    [Test]
    public void T02_declaring_threeofkind_should_give_handtype_threeofkind()
    {
        Card1 = new Card(Symbol.Clubs, Value.Five);
        Card3 = new Card(Symbol.Diamonds, Value.Five);
        Card2 = new Card(Symbol.Spades, Value.Five);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3);

        Assert.AreEqual(cardsFigure.Type, HandType.ThreeOfAKind);

    }

    [Test]
    public void T03_declaring_fourofkind_should_give_handtype_fourofkind()
    {
        Card1 = new Card(Symbol.Clubs, Value.Five);
        Card3 = new Card(Symbol.Diamonds, Value.Five);
        Card2 = new Card(Symbol.Spades, Value.Five);
        Card4 = new Card(Symbol.Hearts, Value.Five);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4);

        Assert.AreEqual(cardsFigure.Type, HandType.FourOfAKind);
    }

    [Test]
    public void T04_declaring_twopairs_should_give_handtype_twopairs()
    {
        Card1 = new Card(Symbol.Clubs, Value.Five);
        Card3 = new Card(Symbol.Diamonds, Value.Five);
        Card2 = new Card(Symbol.Spades, Value.Eight);
        Card4 = new Card(Symbol.Clubs, Value.Eight);


        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4);

        Assert.AreEqual(cardsFigure.Type, HandType.TwoPair);
    }


    [Test]
    public void T05_declaring_flush_should_give_handtype_flush()
    {
        Card1 = new Card(Symbol.Clubs, Value.Five);
        Card3 = new Card(Symbol.Clubs, Value.Ten);
        Card2 = new Card(Symbol.Clubs, Value.Jack);
        Card4 = new Card(Symbol.Clubs, Value.Eight);
        Card5 = new Card(Symbol.Clubs, Value.Two);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4, Card5);

        Assert.AreEqual(cardsFigure.Type, HandType.Flush);
    }

    [Test]
    public void T06_declaring_fullhouse_should_give_handtype_fullhouse()
    {
        Card1 = new Card(Symbol.Clubs, Value.Five);
        Card3 = new Card(Symbol.Hearts, Value.Five);
        Card2 = new Card(Symbol.Diamonds, Value.Five);
        Card4 = new Card(Symbol.Clubs, Value.Two);
        Card5 = new Card(Symbol.Diamonds, Value.Two);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4, Card5);

        Assert.AreEqual(cardsFigure.Type, HandType.FullHouse);
    }

    [Test]
    public void T07_declaring_straight_should_give_handtype_straight()
    {
        Card1 = new Card(Symbol.Clubs, Value.Four);
        Card3 = new Card(Symbol.Hearts, Value.Five);
        Card2 = new Card(Symbol.Diamonds, Value.Six);
        Card4 = new Card(Symbol.Clubs, Value.Seven);
        Card5 = new Card(Symbol.Diamonds, Value.Eight);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4, Card5);

        Assert.AreEqual(cardsFigure.Type, HandType.Straight);
    }

    [Test]
    public void T08_declaring_straightfromAce_should_give_handtype_straight()
    {
        Card1 = new Card(Symbol.Clubs, Value.Ace);
        Card3 = new Card(Symbol.Hearts, Value.Two);
        Card2 = new Card(Symbol.Diamonds, Value.Three);
        Card4 = new Card(Symbol.Clubs, Value.Five);
        Card5 = new Card(Symbol.Diamonds, Value.Four);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4, Card5);

        Assert.AreEqual(cardsFigure.Type, HandType.Straight);
    }

    [Test]
    public void T09_declaring_straightflush_should_give_handtype_straighhtflush()
    {
        Card1 = new Card(Symbol.Clubs, Value.Ace);
        Card3 = new Card(Symbol.Clubs, Value.Two);
        Card2 = new Card(Symbol.Clubs, Value.Three);
        Card4 = new Card(Symbol.Clubs, Value.Five);
        Card5 = new Card(Symbol.Clubs, Value.Four);

        CardsFigure cardsFigure = new CardsFigure(Card1, Card2, Card3, Card4, Card5);

        Assert.AreEqual(cardsFigure.Type, HandType.StraightFlush);
    }
    [Test]
    public void T10_declaring_straightflushFromTen_should_give_handtype_straightFlush()
    {
        CardsFigure card = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Clubs, Value.Ten),
            new Card(Symbol.Clubs, Value.King), new Card(Symbol.Clubs, Value.Queen), new Card(Symbol.Clubs, Value.Jack));

        Assert.AreEqual(card.Type, HandType.StraightFlush);

    }

}
