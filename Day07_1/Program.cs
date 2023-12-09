using System.Text;

namespace Day07_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         List<Hand> allHands = new List<Hand>();
         foreach (string line in allLines)
         {
            string[] handParts = line.Split(' ').Select(x => x.Trim()).ToArray();
            CardType[] cards = ExtractCards(handParts[0]);
            int bid = int.Parse(handParts[1]);
            allHands.Add(new Hand(bid, cards));
         }

         allHands = allHands.OrderBy(hand => (int)hand.Strength)
            .ThenBy(hand => (int)hand.Cards[0])
            .ThenBy(hand => (int)hand.Cards[1])
            .ThenBy(hand => (int)hand.Cards[2])
            .ThenBy(hand => (int)hand.Cards[3])
            .ThenBy(hand => (int)hand.Cards[4])
            .ToList();

         int totalWinnings = 0;
         for (int i = 0; i < allHands.Count; i++)
         {
            totalWinnings += allHands[i].Bid * (i + 1);
         }

         Console.WriteLine(totalWinnings);
         Console.ReadLine();
      }

      private static CardType[] ExtractCards(string cardsData)
      {
         CardType[] cards = new CardType[5];
         for (int i = 0; i < cardsData.Length; i++)
         {
            char curChar = cardsData[i];
            if (char.IsDigit(curChar))
            {
               cards[i] = (CardType)int.Parse(curChar.ToString());
            }
            else
            {
               cards[i] = (CardType)Enum.Parse(typeof(CardType), curChar.ToString());
            }
         }

         return cards;
      }
   }

   internal class Hand
   {
      public CardType[] Cards { get; }
      public int Bid { get; }
      public HandStrength Strength { get; set; }

      public Hand(int bid, params CardType[] cards)
      {
         Cards = cards.Length == 5 ? cards : throw new ArgumentException(nameof(cards));
         Bid = bid;
         Strength = CalculateStrength();
      }

      private HandStrength CalculateStrength()
      {
         Dictionary<CardType, int> cardCount = CountCards();
         if (cardCount.ContainsValue(5))
         {
            return HandStrength.FiveOfKind;
         }
         else if (cardCount.ContainsValue(4))
         {
            return HandStrength.FourOfKind;
         }
         else if (cardCount.ContainsValue(3))
         {
            if (cardCount.ContainsValue(2))
            {
               return HandStrength.FullHouse;
            }
            return HandStrength.ThreeOfKind;
         }
         else if (cardCount.ContainsValue(2))
         {
            int pairCount = cardCount.Count(x => x.Value== 2);
            return pairCount == 2 ? HandStrength.TwoPair : HandStrength.OnePair;
         }

         return HandStrength.HighCard;
      }

      private Dictionary<CardType, int> CountCards()
      {
         Dictionary<CardType, int> cardCount = new Dictionary<CardType, int>();
         foreach (CardType card in Cards)
         {
            if (cardCount.ContainsKey(card))
            {
               cardCount[card]++;
            }
            else
            {
               cardCount[card] = 1;
            }
         }

         return cardCount;
      }

      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();
         foreach (CardType card in Cards)
         {
            sb.Append((int)card < 10 ? (int)card : card.ToString());
         }
         sb.Append($" - {Strength.ToString().PadRight(11)} - {Bid}");
         return sb.ToString();
      }
   }

   internal enum CardType { _2 = 2, _3 = 3, _4 = 4, _5 = 5, _6 = 6, _7 = 7, _8 = 8, _9 = 9, T = 10, J = 11, Q = 12, K = 13, A = 14 }
   internal enum HandStrength { HighCard, OnePair, TwoPair, ThreeOfKind, FullHouse, FourOfKind, FiveOfKind }
}