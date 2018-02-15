using System;
using NUnit.Framework;
using FunnyPoker.src.Controller;
using FunnyPoker.Models;
using System.Collections.Generic;


public class CardsFigureComparerTest
{
    private CardsFigureComparer _cardsFigureComparer = new CardsFigureComparer();

    public CardsFigure firstCardsFigure { get; set; }
    public CardsFigure secCardsFigure { get; set; }
    public CardsFigure thirdCardsFigure { get; set; }


    [Test]
    public void T01_comparing_hgihcard_with_pair_should_mark_pair_higher()
    {
        firstCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace));
        secCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Diamonds, Value.Ace));
        
        var max = (_cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure)) > 0 ? firstCardsFigure : secCardsFigure;

        Assert.AreEqual(max, secCardsFigure);
    }

    [Test]
    public void T02_comparing_highcard_with_straightflush_should_mark_straightflush_higher()
    {
        firstCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace));
        secCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Clubs, Value.Ten),
            new Card(Symbol.Clubs, Value.King), new Card(Symbol.Clubs, Value.Queen), new Card(Symbol.Clubs, Value.Jack));

        Assert.AreEqual(secCardsFigure.Type, HandType.StraightFlush);

        var max = (_cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure)) > 0 ? firstCardsFigure : secCardsFigure;

        Assert.AreEqual(max, secCardsFigure);
    }

    [Test]
    public void T03_comparing_straightFromAce_with_straightFromTen_should_mark_straightFromTen_higher()
    {
        firstCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Diamonds, Value.Two),
            new Card(Symbol.Hearts, Value.Three), new Card(Symbol.Clubs, Value.Four), new Card(Symbol.Clubs, Value.Five));
        secCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ten), new Card(Symbol.Hearts, Value.Jack),
            new Card(Symbol.Diamonds, Value.King), new Card(Symbol.Clubs, Value.Queen), new Card(Symbol.Spades, Value.Ace));

        var max = (_cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure)) > 0 ? firstCardsFigure : secCardsFigure;
        Assert.AreEqual(max, secCardsFigure);
    }

    [Test]
    public void T04_comparing_fullHouseKA_with_fullhouseAK_should_mark_fullhouseAK_higher()
    {
        secCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Diamonds, Value.Ace),
            new Card(Symbol.Hearts, Value.Ace), new Card(Symbol.Clubs, Value.King), new Card(Symbol.Diamonds, Value.King));
        firstCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Hearts, Value.Ace),
            new Card(Symbol.Diamonds, Value.King), new Card(Symbol.Clubs, Value.King), new Card(Symbol.Spades, Value.King));

        var max = (_cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure)) > 0 ? firstCardsFigure : secCardsFigure;

        Assert.AreEqual(max, secCardsFigure);
    }


    [Test]
    public void T05_Comparing_two_flushes_should_mark_neither_higher()
    {
        secCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Ace), new Card(Symbol.Clubs, Value.Two),
            new Card(Symbol.Clubs, Value.Four), new Card(Symbol.Clubs, Value.King), new Card(Symbol.Clubs, Value.Eight));
        firstCardsFigure = new CardsFigure(new Card(Symbol.Spades, Value.Two), new Card(Symbol.Spades, Value.Ace),
            new Card(Symbol.Spades, Value.Three), new Card(Symbol.Spades, Value.Four), new Card(Symbol.Spades, Value.King));

        Assert.AreEqual(secCardsFigure.Type, HandType.Flush);
        Assert.AreEqual(firstCardsFigure.Type, HandType.Flush);

        var val = _cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure);
        CardsFigure max = null;
        if (val != 0)
        {
            max = new CardsFigure();
            max = (val) > 0 ? firstCardsFigure : secCardsFigure;
        }

        Assert.IsNull(max);
    }

    [Test]
    public void T06_comparing_FH23_with_FH32_should_mark_FH32_higher()
    {
        firstCardsFigure = new CardsFigure(new Card(Symbol.Clubs, Value.Two), new Card(Symbol.Diamonds, Value.Two), new Card(Symbol.Hearts, Value.Two), new Card(Symbol.Clubs, Value.Three), new Card(Symbol.Diamonds, Value.Three));
        secCardsFigure = new CardsFigure(new Card(Symbol.Hearts, Value.Three), new Card(Symbol.Clubs, Value.Three), new Card(Symbol.Diamonds, Value.Three), new Card(Symbol.Clubs, Value.Two), new Card(Symbol.Diamonds, Value.Two));

        var val = _cardsFigureComparer.Compare(firstCardsFigure, secCardsFigure);

        Assert.Less(val, 0);

    }

}

