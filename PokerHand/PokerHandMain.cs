using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.VisualBasic;

namespace PokerHand
{
    class PokerHandMain
    {
        private static readonly char[] Suits = { 'H', 'S', 'C', 'D' };
        private static readonly char[] Ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        private static readonly string[] Cards = new string[52];
        private static Dictionary<HandType, int> aggregateHands = new Dictionary<HandType, int>();
        static void Main(string[] args)
        {
            CreateDeck(); 

            // Generate 100 hands and print them out for analysis
            Console.WriteLine("Generate 100 hands and print them out for analysis");
            Console.WriteLine();
            for (int i = 0; i < 100; i++)
            {
                ShuffleCards();
                var player1 = new PokerHand(DealHand(0));
                Console.WriteLine($"{player1.GetInputString().ToString()}\t\t{player1.GetHandType()}");

                var player2 = new PokerHand(DealHand(5));
                Console.WriteLine($"{player2.GetInputString().ToString()}\t\t{player2.GetHandType()}");
                var player3 = new PokerHand(DealHand(10));
                Console.WriteLine($"{player3.GetInputString().ToString()}\t\t{player3.GetHandType()}");
                var player4 = new PokerHand(DealHand(15));
                Console.WriteLine($"{player4.GetInputString().ToString()}\t\t{player4.GetHandType()}");
                var player5 = new PokerHand(DealHand(20));
                Console.WriteLine($"{player5.GetInputString().ToString()}\t\t{player5.GetHandType()}");
            }

            // Generate hands until 10 royal flushes are dealt
            Console.WriteLine("Generate hands until 10 royal flushes are dealt");
            Console.WriteLine();
            int numberOfRoyalFlushes = 0;

            //while (numberOfRoyalFlushes<10)
            for (int i=0; i<2000000; i++)
            {
                ShuffleCards();
                var player1 = new PokerHand(DealHand(0));
                LogHand(player1);
                var player2 = new PokerHand(DealHand(5));
                LogHand(player2);
                var player3 = new PokerHand(DealHand(10));
                LogHand(player3);
                var player4 = new PokerHand(DealHand(15));
                LogHand(player4);
                var player5 = new PokerHand(DealHand(20));
                LogHand(player5);
                if (player1.GetHandType() == HandType.Royal_Flush 
                    || player2.GetHandType() == HandType.Royal_Flush
                    || player3.GetHandType() == HandType.Royal_Flush
                    || player4.GetHandType() == HandType.Royal_Flush
                    || player5.GetHandType() == HandType.Royal_Flush
                    )
                {
                    numberOfRoyalFlushes++;
                }
            }

            aggregateHands.ToList().ForEach(x=> Console.WriteLine($"{x.Key.ToString().PadRight(20)}{x.Value, 20}"));

        }

        static void LogHand(PokerHand hand)
        {
            if (aggregateHands.ContainsKey(hand.GetHandType()))
            {
                aggregateHands[hand.GetHandType()] += 1;
            }
            else
            {
                aggregateHands[hand.GetHandType()] = 1;
            }
        }

        static string DealHand(int index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(Cards[index++]);
                sb.Append(" ");
            }

            return sb.ToString().TrimEnd();
        }

        static void CreateDeck()
        {
            for (int i = 0; i < Suits.Length; i++)
            {
                for (int j = 0; j < Ranks.Length; j++)
                {
                    Cards[(i * Ranks.Length) + j] = String.Concat(Ranks[j], Suits[i]);
                }
            }
        }

        static void ShuffleCards()
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int index1 = r.Next(0, 51);
                int index2 = r.Next(0, 51);
                SwapCards(index1, index2);
            }
        }

        static void SwapCards(int indexCard1, int indexCard2)
        {
            string temp = Cards[indexCard1];
            Cards[indexCard1] = Cards[indexCard2];
            Cards[indexCard2] = temp;

        }
    }

    public enum Result
    {
        Win,
        Loss,
        Tie
    }

    public enum HandType
    {
        High_Card,
        Pair,
        Two_Pair,
        Three_Of_A_Kind,
        Straight,
        Flush,
        Full_House,
        Four_Of_A_Kind,
        Straight_Flush,
        Royal_Flush
    }

    public class PokerHand
    {
        private string[] MyCards { get; set; }
        private HandType MyHandType { get; set; }
        private string InputString { get; set; }
        private readonly char[] Suits = { 'H', 'S', 'C', 'D' };
        private readonly char[] Ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        private Dictionary<char, int> MySuits { get; set; }
        private Dictionary<int, int> MyRanks { get; set; }
        public PokerHand(string cards)
        {
            InputString = cards;
            MyCards = cards.Split(' ');
            InitMySuits();
            InitMyRanks();
            MyHandType = DetermineHandType();
        }

        public void InitMyRanks()
        {
            MyRanks = new Dictionary<int, int>();
            for (int i = 0; i < MyCards.Length; i++)
            {
                char ThisFace = MyCards[i][0];
                int ThisRank = Ranks.ToList().IndexOf(ThisFace);
                if (MyRanks.ContainsKey(ThisRank))
                {
                    MyRanks[ThisRank] += 1;
                }
                else
                {
                    MyRanks[ThisRank] = 1;
                }
            }
        }

        public void InitMySuits()
        {
            MySuits = new Dictionary<char, int>();
            for (int i = 0; i < MyCards.Length; i++)
            {
                char ThisSuit = MyCards[i][1];
                if (MySuits.ContainsKey(ThisSuit))
                {
                    MySuits[ThisSuit] += 1;
                }
                else
                {
                    MySuits[ThisSuit] = 1;
                }
            }
        }

        public string GetInputString()
        {
            return InputString;
        }

        public Dictionary<int, int> GetMyRanks()
        {
            return MyRanks;
        }

        public Dictionary<char, int> GetMySuits()
        {
            return MySuits;
        }

        public int GetHighestPair(Dictionary<int, int> ranks)
        {
            int rank = -1;

            return rank;
        }

        public string[] GetHand()
        {
            return MyCards;
        }

        public HandType GetHandType()
        {
            return MyHandType;
        }

        public Result CompareWith(PokerHand opponentHand)
        {
            Result res = Result.Tie;
            int myHandVal = (int)this.MyHandType;
            int opponentHandVal = (int) opponentHand.GetHandType();
            if (myHandVal > opponentHandVal)
            {
                res = Result.Win;
            }
            else if
                (myHandVal < opponentHandVal)
            {
                res = Result.Loss;
            }
            else
            {
                res = BreakTheTie(opponentHand);
            }

            return res;
        }

        public Result BreakTheTie(PokerHand opponentHand)
        {
            Result res = Result.Tie;

            switch (MyHandType)
            {
                case HandType.High_Card:        // highest card wins
                case HandType.Flush:            // highest ranked card wins
                case HandType.Straight_Flush:   // highest rank at the top of the sequence wins
                case HandType.Straight:         // highest ranking card at the top of the sequence wins
                    int myHighCard = MyRanks.Keys.Max(x => x);
                    int oppHighCard = opponentHand.MyRanks.Keys.Max(x => x);
                    if (myHighCard > oppHighCard)
                    {
                        res = Result.Win;
                    }
                    else if (myHighCard < oppHighCard)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        res = Result.Tie;
                    }
                    break;
                case HandType.Pair:             // highest pair wins
                case HandType.Two_Pair:         // highest pair wins
                    int myMaxRank = MyRanks.Where(x => x.Value == 2).Max(x => x.Key);
                    int opponentMaxRank = opponentHand.MyRanks.Where(x => x.Value == 2).Max(x => x.Key);
                    if (myMaxRank > opponentMaxRank)
                    {
                        res = Result.Win;
                    }
                    else if (myMaxRank < opponentMaxRank)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        myMaxRank = MyRanks.Where(x => x.Value == 1).Max(x => x.Key);
                        opponentMaxRank = opponentHand.MyRanks.Where(x => x.Value == 1).Max(x => x.Key);
                        if (myMaxRank > opponentMaxRank)
                        {
                            res = Result.Win;
                        }
                        else if (myMaxRank < opponentMaxRank)
                        {
                            res = Result.Loss;
                        }
                        else
                        {
                            res = Result.Tie;
                        }
                    }
                    break;
                case HandType.Three_Of_A_Kind:  // highest ranking 3 of a kind wins
                case HandType.Full_House:       // Highest 3 cards wins
                    myMaxRank = MyRanks.Where(x => x.Value == 3).Max(x => x.Key);
                    opponentMaxRank = opponentHand.MyRanks.Where(x => x.Value == 3).Max(x => x.Key);
                    if (myMaxRank > opponentMaxRank)
                    {
                        res = Result.Win;
                    }
                    else if (myMaxRank < opponentMaxRank)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        res = Result.Tie;
                    }
                    break;
                case HandType.Four_Of_A_Kind:   // highest 4 of a kind wins
                    myMaxRank = MyRanks.Where(x => x.Value == 4).Max(x => x.Key);
                    opponentMaxRank = opponentHand.MyRanks.Where(x => x.Value == 4).Max(x => x.Key);
                    if (myMaxRank > opponentMaxRank)
                    {
                        res = Result.Win;
                    }
                    else if (myMaxRank < opponentMaxRank)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        res = Result.Tie;
                    }
                    break;
            }

            return res;
        }

        public HandType DetermineHandType()
        {
            HandType type = HandType.High_Card;
            if (IsFlush())
            {
                type = HandType.Flush;
                if (IsStraightFlush())
                {
                    if (IsRoyalFlush())
                    {
                        type = HandType.Royal_Flush;
                    }
                    else
                    {
                        type = HandType.Straight_Flush;
                    }
                }
            }
            else if (IsStraight())
            {
                type = HandType.Straight;
            }
            else if (IsFourOfAKind())
            {
                type = HandType.Four_Of_A_Kind;
            }
            else if (IsFullHouse())
            {
                type = HandType.Full_House;
            }
            else if (IsThreeOfAKind())
            {
                type = HandType.Three_Of_A_Kind;
            }
            else if (IsTwoPair())
            {
                type = HandType.Two_Pair;
            }
            else if (IsAPair())
            {
                type = HandType.Pair;
            }
            return type;
        }

        private bool IsRoyalFlush()
        {
            return MyRanks.ContainsKey(8) && MyRanks.ContainsKey(12);
        }

        private bool IsAPair()
        {
            bool isAPair = false;
            if (MyRanks.Count == 4)
            {
                if (MyRanks.ToList().Max(x => x.Value) == 2)
                {
                    isAPair = true;
                }
            }
            return isAPair;
        }

        private bool IsTwoPair()
        {
            bool isTwoPair = false;
            if (MyRanks.Count == 3)
            {
                if (MyRanks.ToList().Max(x => x.Value) == 2)
                {
                    isTwoPair = true;
                }
            }
            return isTwoPair;
        }

        private bool IsThreeOfAKind()
        {
            bool isThreeOfAKind = false;
            if (MyRanks.Count == 3)
            {
                if (MyRanks.ToList().Max(x => x.Value) == 3)
                {
                    isThreeOfAKind = true;
                }
            }
            return isThreeOfAKind;
        }

        private bool IsFullHouse()
        {
            bool isFullHouse = false;
            if (MyRanks.Count == 2)
            {
                if (MyRanks.ToList().Max(x => x.Value) == 3)
                {
                    isFullHouse = true;
                }
            }
            return isFullHouse;
        }

        private bool IsFourOfAKind()
        {
            bool isFourOfAKind = false;

            if (MyRanks.Count == 2)
            {
                if (MyRanks.ToList().Max(x => x.Value) == 4)
                {
                    isFourOfAKind = true;
                }
            }

            return isFourOfAKind;
        }

        private bool IsStraightFlush()
        {
            return IsStraight();
        }

        private bool IsStraight()
        {
            bool isStraight = false;
            if (MyRanks.Count == 5)
            {
                int max = MyRanks.ToList().Max(x => x.Key);
                int min = MyRanks.ToList().Min(x => x.Key);
                if (max - min == 4 || (max - min == 12 && SecondHighestRank()==3))
                {
                    isStraight = true;
                }
            }

            return isStraight;
        }

        private int SecondHighestRank()
        {
            // Used for tie breaker in a straight. Assumes 5 ranks
            var sortedRanks = MyRanks.ToList().OrderBy(x => x.Key).ToArray();
            return sortedRanks[3].Key;
        }

        private bool IsFlush()
        {
            return MySuits.Count == 1;
        }

        public bool IsHandValid(string[] hand)
        {
            if ((hand.Length != 5)
                    || (!hand.ToList().TrueForAll(x => x.Length == 2))
                    || (!hand.ToList().TrueForAll(x => SuitIsValid(x[1])))
                    || (!hand.ToList().TrueForAll(x => RankIsValid(x[0])))
                )
                return false;

            return true;
        }

        private bool SuitIsValid(char c)
        {
            return Suits.Contains(c.ToString().ToUpper().ToCharArray()[0]);
        }

        private bool RankIsValid(char c)
        {
            return Ranks.Contains(c.ToString().ToUpper().ToCharArray()[0]);
        }

    }
}
