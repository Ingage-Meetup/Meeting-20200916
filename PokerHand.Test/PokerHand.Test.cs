using System.Collections.Generic;
using PokerHand;
using NUnit.Framework;

namespace PokerHand.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanCreateHand()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD TD");
            Assert.AreEqual(5, testHand.GetHand().Length);
        }

        [Test]
        public void ValidHandReturnsValid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD TD");
            string[] hand = {"KS", "2H", "5C", "JD", "TD"};
            Assert.IsTrue(testHand.IsHandValid(hand), "Valid hand flagged as invalid");

        }

        [Test]
        public void BadSuitReturnsInvalid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5L JD TD");
            string[] hand = { "KS", "2H", "5L", "JD", "TD" };
            Assert.IsFalse(testHand.IsHandValid(hand), "Hand with illegal suit returned valid");

        }

        [Test]
        public void BadRankReturnsInvalid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C 1D TD");
            string[] hand = { "KS", "2H", "5C", "1D", "TD" };
            Assert.IsFalse(testHand.IsHandValid(hand), "Hand with illegal rank returned valid");

        }

        [Test]
        public void ExtraneousCharacterReturnsInvalid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5CQ JD TD");
            string[] hand = { "KS", "2H", "5CQ", "JD", "TD" };
            Assert.IsFalse(testHand.IsHandValid(hand), "Hand with extraneous character returned valid");

        }

        [Test]
        public void TooManyCardsReturnsInvalid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD TD AH");
            string[] hand = { "KS", "2H", "5C", "JD", "TD", "AH" };
            Assert.IsFalse(testHand.IsHandValid(hand), "Hand with too many cards returned valid");

        }

        [Test]
        public void TooFewCardsReturnsInvalid()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD");
            string[] hand = { "KS", "2H", "5C", "JD" };
            Assert.IsFalse(testHand.IsHandValid(hand), "Hand with too few cards returned valid");

        }

        [Test]
        public void InitSuits4Suits()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD TD");
            Dictionary<char, int> suits = testHand.GetMySuits();
            Assert.AreEqual(1, suits['S'] );
            Assert.AreEqual(1, suits['H']);
            Assert.AreEqual(1, suits['C']);
            Assert.AreEqual(2, suits['D']);

        }

        [Test]
        public void InitRanks4Ranks()
        {
            PokerHand testHand = new PokerHand("KS 2H 5C JD TD");
            Dictionary<int, int> ranks = testHand.GetMyRanks();
            Assert.AreEqual(1, ranks[11]);
            Assert.AreEqual(1, ranks[0]);
            Assert.AreEqual(1, ranks[3]);
            Assert.AreEqual(1, ranks[9]);
            Assert.AreEqual(1, ranks[8]);

        }

        [Test]
        public void InitRanks2Ranks()
        {
            PokerHand testHand = new PokerHand("KS KH KC TD TH");
            Dictionary<int, int> ranks = testHand.GetMyRanks();
            Assert.AreEqual(3, ranks[11]);
            Assert.AreEqual(2, ranks[8]);

        }

        [Test]
        public void DetectRoyalFlushOfHearts()
        {
            PokerHand testHand = new PokerHand("AH TH QH JH KH");
            Assert.AreEqual(HandType.Royal_Flush, testHand.GetHandType());
        }

        [Test]
        public void DetectRoyalFlushOfSpades()
        {
            PokerHand testHand = new PokerHand("AS TS QS JS KS");
            Assert.AreEqual(HandType.Royal_Flush, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight1()
        {
            PokerHand testHand = new PokerHand("2S 3H 4C 5D 6H");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight2()
        {
            PokerHand testHand = new PokerHand("7S 3H 4C 5D 6H");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight3()
        {
            PokerHand testHand = new PokerHand("7S 8H 4C 5D 6H");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight4()
        {
            PokerHand testHand = new PokerHand("7S 8H 9C 5D 6H");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight5()
        {
            PokerHand testHand = new PokerHand("7S 8H 9C TD 6H");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight6()
        {
            PokerHand testHand = new PokerHand("7S 8H 9C TD JH");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight7()
        {
            PokerHand testHand = new PokerHand("QS 8H 9C TD JH");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight8()
        {
            PokerHand testHand = new PokerHand("QS KH 9C TD JH");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraight9()
        {
            PokerHand testHand = new PokerHand("QS KH AC TD JH");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }



        [Test]
        public void DetectStraightAceHigh()
        {
            PokerHand testHand = new PokerHand("TS JH QC KD AH");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void DetectStraightAceLow()
        {
            PokerHand testHand = new PokerHand("AS 2S 3H 4C 5D");
            Assert.AreEqual(HandType.Straight, testHand.GetHandType());
        }

        [Test]
        public void RejectStraightAceLowWhenOffByOne()
        {
            PokerHand testHand = new PokerHand("AS 2S 3H 4C 6D");
            Assert.AreEqual(HandType.High_Card, testHand.GetHandType());
        }

        [Test]
        public void DetectFlush()
        {
            PokerHand testHand = new PokerHand("2S 3S 4S KS 6S");
            Assert.AreEqual(HandType.Flush, testHand.GetHandType());
        }

        [Test]
        public void DetectStraightFlush()
        {
            PokerHand testHand = new PokerHand("2S 3S 4S 5S 6S");
            Assert.AreEqual(HandType.Straight_Flush, testHand.GetHandType());
        }

        [Test]
        public void DetectFourOfAKind()
        {
            PokerHand testHand = new PokerHand("2S 2H 2C 2D 6S");
            Assert.AreEqual(HandType.Four_Of_A_Kind, testHand.GetHandType());
        }

        [Test]
        public void DetectFullHouse()
        {
            PokerHand testHand = new PokerHand("2S 2H 2C 6D 6S");
            Assert.AreEqual(HandType.Full_House, testHand.GetHandType());
        }

        [Test]
        public void DetectThreeOfAKind()
        {
            PokerHand testHand = new PokerHand("2S 2H 2C 6D 7S");
            Assert.AreEqual(HandType.Three_Of_A_Kind, testHand.GetHandType());
        }

        [Test]
        public void DetectTwoPair()
        {
            PokerHand testHand = new PokerHand("2S 2H 6C 6D 7S");
            Assert.AreEqual(HandType.Two_Pair, testHand.GetHandType());
        }

        [Test]
        public void DetectPair()
        {
            PokerHand testHand = new PokerHand("2S 2H 6C 7D 9S");
            Assert.AreEqual(HandType.Pair, testHand.GetHandType());
        }

        [Test]
        public void DetectHighCard()
        {
            PokerHand testHand = new PokerHand("2S 4H 6C 8D TS");
            Assert.AreEqual(HandType.High_Card, testHand.GetHandType());
        }

        [Test]
        public void DetectAWinner()
        {
            PokerHand player1 = new PokerHand("2S 2H 6C 8D TS");
            PokerHand player2 = new PokerHand("4S 7H 6C 8D TS");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void DetectALoss()
        {
            PokerHand player1 = new PokerHand("2S 2H 6C 8D TS");
            PokerHand player2 = new PokerHand("4S 4H 4C 8S TH");
            Assert.AreEqual(Result.Loss, player1.CompareWith(player2));
        }

        [Test]
        public void DetectATie()
        {
            PokerHand player1 = new PokerHand("2S 2H 6H 8D TS");
            PokerHand player2 = new PokerHand("2C 2D 6C 8S TH");
            Assert.AreEqual(Result.Tie, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieHighCardIWin()
        {
            PokerHand player1 = new PokerHand("2S 4H 6H 8D TS");
            PokerHand player2 = new PokerHand("2C 3D 5C 6S 7H");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieHighCardTheyWin()
        {
            PokerHand player1 = new PokerHand("2S 4H 6H 8D 9S");
            PokerHand player2 = new PokerHand("2C 3D 5C 6S TH");
            Assert.AreEqual(Result.Loss, player1.CompareWith(player2));
        }

        [Test]
        public void CantBreakATieHighCard()
        {
            PokerHand player1 = new PokerHand("2S 4H 6H 8D 9S");
            PokerHand player2 = new PokerHand("2C 3D 5C 6S 9H");
            Assert.AreEqual(Result.Tie, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieFlushIWin()
        {
            PokerHand player1 = new PokerHand("2H 4H 6H 8H TH");
            PokerHand player2 = new PokerHand("2C 3C 5C 6C 7C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieFlushTheyWin()
        {
            PokerHand player1 = new PokerHand("2H 4H 6H 8H 9H");
            PokerHand player2 = new PokerHand("2C 3C 5C 6C TC");
            Assert.AreEqual(Result.Loss, player1.CompareWith(player2));
        }

        [Test]
        public void CantBreakATieFlush()
        {
            PokerHand player1 = new PokerHand("2H 4H 6H 8H TH");
            PokerHand player2 = new PokerHand("2C 3C 5C 6C TC");
            Assert.AreEqual(Result.Tie, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieTwoPairIWin()
        {
            PokerHand player1 = new PokerHand("2H 2S 6C 6H TH");
            PokerHand player2 = new PokerHand("2C 2D 5D 5S 4C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieTwoPairTheyWin()
        {
            PokerHand player1 = new PokerHand("2H 2S 5C 5H TH");
            PokerHand player2 = new PokerHand("2C 2D 6D 6S 4C");
            Assert.AreEqual(Result.Loss, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieTwoPairHighCard()
        {
            PokerHand player1 = new PokerHand("2H 2S 6C 6H TH");
            PokerHand player2 = new PokerHand("2C 2D 6D 6S 4C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATiePairIWin()
        {
            PokerHand player1 = new PokerHand("2H 4S 6C 6H TH");
            PokerHand player2 = new PokerHand("2C 7D 5D 5S 4C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATiePairTheyWin()
        {
            PokerHand player1 = new PokerHand("2C 7D 5D 5S 4C");
            PokerHand player2 = new PokerHand("2H 4S 6C 6H TH");
            Assert.AreEqual(Result.Loss, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATiePairHighCard()
        {
            PokerHand player1 = new PokerHand("2H 4S 6C 6H TH");
            PokerHand player2 = new PokerHand("2C 3D 6D 6S 4C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieFullHouseIWin()
        {
            PokerHand player1 = new PokerHand("2H 2S 6C 6H 6D");
            PokerHand player2 = new PokerHand("2C 2D 5D 5S 5C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieThreeOfAKindIWin()
        {
            PokerHand player1 = new PokerHand("2H 3S 6C 6H 6D");
            PokerHand player2 = new PokerHand("2C 3D 5D 5S 5C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }

        [Test]
        public void BreakATieFourOfAKindIWin()
        {
            PokerHand player1 = new PokerHand("2H 6S 6C 6H 6D");
            PokerHand player2 = new PokerHand("2C 5H 5D 5S 5C");
            Assert.AreEqual(Result.Win, player1.CompareWith(player2));
        }


    }
}