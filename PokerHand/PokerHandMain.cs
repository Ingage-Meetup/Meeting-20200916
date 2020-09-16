using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
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
            DisplayMenu();

        }

        public static void DisplayMenu()
        {
            bool donePlaying = false;
            while (!donePlaying)
            {
                int option = 0;
                Console.WriteLine();
                Console.WriteLine("Menu");
                Console.WriteLine();
                Console.WriteLine("1. Generate Lots Of Hands And Gather Stats");
                Console.WriteLine("2. Play a game of poker with two players - console input");
                Console.WriteLine("3. Play a game of poker with two players - computer generated");
                Console.WriteLine("4. Exit the app");
                Console.WriteLine();
                Console.Write("Enter 1, 2, 3, or 4: ");
                option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        GenerateLotsOfHandsAndGatherStats();
                        break;
                    case 2:
                        TwoPlayerGameConsoleInput();
                        break;
                    case 3:
                        TwoPlayerGameGenerated();
                        break;
                    case 4:
                        donePlaying = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;

                }
            }

            Console.WriteLine();
            Console.WriteLine("Thanks for playing!");
            Console.WriteLine();
        }

        private static void TwoPlayerGameGenerated()
        {
            CreateDeck();
            ShuffleCards();
            var player1 = new PokerHand(DealHand(0));
            var player2 = new PokerHand(DealHand(5));

            DetermineGameOutcome(player1, player2);
        }

        private static void TwoPlayerGameConsoleInput()
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write("Enter Player 1's cards: ");
            var player1Cards = Console.ReadLine();
            var player1 = new PokerHand(player1Cards);

            Console.WriteLine();
            Console.Write("Enter Player 2's cards: ");
            var player2Cards = Console.ReadLine();
            var player2 = new PokerHand(player2Cards);

            DetermineGameOutcome(player1, player2);
        }

        public static void DetermineGameOutcome(PokerHand player1, PokerHand player2)
        {
            Result res = player1.CompareWith(player2);

            Console.WriteLine();
            Console.WriteLine($"Player 1's hand: {player1.GetInputString()}");
            Console.WriteLine($"Player 2's hand: {player2.GetInputString()}");
            Console.WriteLine();

            if (res == Result.Win)
            {
                Console.Write("Player One is a winner! ");
                Console.WriteLine($"A {player1.GetHandType()} beats a {player2.GetHandType()}");
            }
            else if (res == Result.Loss)
            {
                Console.Write("Player Two is a winner! ");
                Console.WriteLine($"A {player2.GetHandType()} beats a {player1.GetHandType()}");
            }
            else
            {
                Console.Write("Player One and Player Two tie!");
                Console.WriteLine($"A {player1.GetHandType()} equals {player2.GetHandType()}");
            }

            Console.WriteLine();
        }



        public static void GenerateLotsOfHandsAndGatherStats()
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

            Console.WriteLine();
            Console.WriteLine("Generate hands until a royal flush is dealt");
            Console.WriteLine();

            int numberOfHands = 0;
            aggregateHands.Clear();

            while (true)
            {
                ShuffleCards();
                Random r = new Random();
                int index = r.Next(0, 47);
                var player = new PokerHand(DealHand(index));
                numberOfHands++;
                LogHand(player);
                if (player.GetHandType() == HandType.Royal_Flush)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"{numberOfHands} hands were dealt.");
            Console.WriteLine();
            aggregateHands.ToList().ForEach(x => Console.WriteLine($"{x.Key.ToString().PadRight(20)}{x.Value,20}"));
        }

        static void LogHand(PokerHand hand)
        {
            if (hand.GetHandType() == HandType.Royal_Flush)
            {
                string s = "Stop the presses!";
            }
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

            if (HandIsValid())
            {
                InitMySuits();
                InitMyRanks();
                MyHandType = DetermineHandType();
            }
            else
            {
                throw new Exception("Invalid hand!");
            }
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
            int opponentHandVal = (int)opponentHand.GetHandType();
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
            var myRanksArray = MyRanks.ToArray();
            var oppRanksArray = opponentHand.MyRanks.ToArray();
            switch (MyHandType)
            {
                case HandType.High_Card:        // highest rank wins
                case HandType.Flush:            // highest rank card wins
                case HandType.Straight_Flush:   // highest rank at the top of the sequence wins
                case HandType.Straight:         // highest rank card at the top of the sequence wins

                    int myHighCardRank = 0;
                    for (int i = 0; i < MyRanks.Count; i++)
                    {
                        if (myRanksArray[i].Key > myHighCardRank)
                            myHighCardRank = myRanksArray[i].Key;
                    }
                    int oppHighCardRank = 0;
                    for (int i = 0; i < opponentHand.MyRanks.Count; i++)
                    {
                        if (oppRanksArray[i].Key > oppHighCardRank)
                            oppHighCardRank = oppRanksArray[i].Key;
                    }
                    if (myHighCardRank > oppHighCardRank)
                    {
                        res = Result.Win;
                    }
                    else if (myHighCardRank < oppHighCardRank)
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
                    int myHighPairRank = 0;
                    for (int i = 0; i < MyRanks.Count; i++)
                    {
                        if (myRanksArray[i].Value == 2 && myRanksArray[i].Key > myHighPairRank)
                            myHighPairRank = myRanksArray[i].Key;
                    }
                    int oppHighPairRank = 0;
                    for (int i = 0; i < opponentHand.MyRanks.Count; i++)
                    {
                        if (oppRanksArray[i].Value == 2 && oppRanksArray[i].Key > oppHighPairRank)
                            oppHighPairRank = oppRanksArray[i].Key;
                    }
                    if (myHighPairRank > oppHighPairRank)
                    {
                        res = Result.Win;
                    }
                    else if (myHighPairRank < oppHighPairRank)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        myHighPairRank = MyRanks.Where(x => x.Value == 1).Max(x => x.Key);
                        oppHighPairRank = opponentHand.MyRanks.Where(x => x.Value == 1).Max(x => x.Key);
                        if (myHighPairRank > oppHighPairRank)
                        {
                            res = Result.Win;
                        }
                        else if (myHighPairRank < oppHighPairRank)
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
                    int myHighPairRankForThreeOfAKind = 0;
                    for (int i = 0; i < MyRanks.Count; i++)
                    {
                        if (myRanksArray[i].Value == 3 && myRanksArray[i].Key > myHighPairRankForThreeOfAKind)
                            myHighPairRankForThreeOfAKind = myRanksArray[i].Key;
                    }
                    int oppHighPairRankForThreeOfAKind = 0;
                    for (int i = 0; i < opponentHand.MyRanks.Count; i++)
                    {
                        if (oppRanksArray[i].Value == 3 && oppRanksArray[i].Key > oppHighPairRankForThreeOfAKind)
                            oppHighPairRankForThreeOfAKind = oppRanksArray[i].Key;
                    }
                    if (myHighPairRankForThreeOfAKind > oppHighPairRankForThreeOfAKind)
                    {
                        res = Result.Win;
                    }
                    else if (myHighPairRankForThreeOfAKind < oppHighPairRankForThreeOfAKind)
                    {
                        res = Result.Loss;
                    }
                    else
                    {
                        res = Result.Tie;
                    }
                    break;
                case HandType.Four_Of_A_Kind:   // highest 4 of a kind wins
                    int myHighPairRankForFourOfAKind = 0;
                    for (int i = 0; i < MyRanks.Count; i++)
                    {
                        if (myRanksArray[i].Value == 4 && myRanksArray[i].Key > myHighPairRankForFourOfAKind)
                            myHighPairRankForFourOfAKind = myRanksArray[i].Key;
                    }
                    int oppHighPairRankForFourOfAKind = 0;
                    for (int i = 0; i < opponentHand.MyRanks.Count; i++)
                    {
                        if (oppRanksArray[i].Value == 4 && oppRanksArray[i].Key > oppHighPairRankForFourOfAKind)
                            oppHighPairRankForFourOfAKind = oppRanksArray[i].Key;
                    }
                    myHighPairRank = MyRanks.Where(x => x.Value == 4).Max(x => x.Key);
                    oppHighPairRank = opponentHand.MyRanks.Where(x => x.Value == 4).Max(x => x.Key);
                    if (myHighPairRankForFourOfAKind > oppHighPairRankForFourOfAKind)
                    {
                        res = Result.Win;
                    }
                    else if (myHighPairRankForFourOfAKind < oppHighPairRankForFourOfAKind)
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
            bool isStraight = false;
            if (MySuits.Count == 1)
            {
                type = HandType.Flush;
                if (MyRanks.Count == 5)
                {
                    var ranksArray = MyRanks.ToArray();
                    int max = 0;
                    int min = 15;
                    for (int i = 0; i < MyRanks.Count; i++)
                    {
                        if (ranksArray[i].Key > max)
                            max = ranksArray[i].Key;
                        if (ranksArray[i].Key < min)
                            min = ranksArray[i].Key;
                    }

                    if (max - min == 4 || (max - min == 12 && MyRanks.ToList().OrderBy(x => x.Key).ToArray()[3].Key == 3))
                    {
                        if (MyRanks.ContainsKey(8) && MyRanks.ContainsKey(12))
                        {
                            type = HandType.Royal_Flush;
                        }
                        else
                        {
                            type = HandType.Straight_Flush;
                        }
                    }
                }
                return type;
            }
            isStraight = false;
            if (MyRanks.Count == 5)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                int min = 15;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Key > max)
                        max = ranksArray[i].Key;
                    if (ranksArray[i].Key < min)
                        min = ranksArray[i].Key;
                }
                if (max - min == 4 || (max - min == 12 && MyRanks.ToList().OrderBy(x => x.Key).ToArray()[3].Key == 3))
                {
                    return HandType.Straight;
                }
            }
            bool isFourOfAKind = false;

            if (MyRanks.Count == 2)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Value > max)
                        max = ranksArray[i].Value;
                }
                if (max == 4)
                {
                    return HandType.Four_Of_A_Kind;
                }
            }
            bool isFullHouse = false;
            if (MyRanks.Count == 2)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Value > max)
                        max = ranksArray[i].Value;
                }
                if (max == 3)
                {
                    return HandType.Full_House;
                }
            }
            bool isThreeOfAKind = false;
            if (MyRanks.Count == 3)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Value > max)
                        max = ranksArray[i].Value;
                }
                if (max == 3)
                {
                    return HandType.Three_Of_A_Kind;
                }
            }
            bool isTwoPair = false;
            if (MyRanks.Count == 3)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Value > max)
                        max = ranksArray[i].Value;
                }
                if (max == 2)
                {
                    return HandType.Two_Pair;
                }
            }
            bool isAPair = false;
            if (MyRanks.Count == 4)
            {
                var ranksArray = MyRanks.ToArray();
                int max = 0;
                for (int i = 0; i < MyRanks.Count; i++)
                {
                    if (ranksArray[i].Value > max)
                        max = ranksArray[i].Value;
                }
                if (max == 2)
                {
                    return HandType.Pair;
                }
            }
            return type;
        }

        public bool HandIsValid()
        {
            if (MyCards.Length != 5)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < MyCards.Length; i++)
                {
                    if (MyCards[i].Length != 2)
                    {
                        return false;
                    }

                }
                for (int i = 0; i < MyCards.Length; i++)
                {
                    bool suitIsValid = false;
                    char c = MyCards[i][1];
                    var toChar = c.ToString().ToUpper().ToCharArray()[0];
                    for (var index = 0; index < Suits.Length; index++)
                    {
                        var suit = Suits[index];
                        if (Equals(suit, toChar))
                            suitIsValid = true;
                    }

                    if (!suitIsValid)
                    {
                        return false;
                    }
                }
                for (int i = 0; i < MyCards.Length; i++)
                {
                    bool rankIsValid = false;
                    char c = MyCards[i][0];
                    var toChar = c.ToString().ToUpper().ToCharArray()[0];
                    foreach (var rank in Ranks)
                        if (Equals(rank, toChar))
                            rankIsValid = true;

                    if (!rankIsValid)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
