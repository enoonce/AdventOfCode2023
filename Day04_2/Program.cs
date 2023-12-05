namespace Day04_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");
         List<Card> cards = new List<Card>();
         for (int i = 0; i < allLines.Length; i++)
         {
            if (!TryFindExistingCard(i + 1, out Card curCard))
            {
               Card newCard = new Card(allLines[i]);
               cards.Add(newCard);
               curCard = newCard;
            }
            else
            {
               curCard.Copies.Add(curCard.Copy());
            }

            int matches = curCard.PlayerNumbers.Count(s => curCard.WinningNumbers.Contains(s));
            Console.WriteLine($"Card {curCard.Id} has {matches} matches and {curCard.Copies.Count} copies.");
            for (int j = 0; j < matches; j++)
            {
               int offsetId = curCard.Id + 1 + j;
               if (offsetId > allLines.Length)
               {
                  continue;
               }

               for (int k = 0; k < curCard.Copies.Count + 1; k++)
               {
                  if (!TryFindExistingCard(offsetId, out Card foundCard))
                  {
                     Card newCard = new Card(allLines[offsetId - 1]);
                     foundCard = newCard;
                     cards.Add(newCard);
                  }
                  else
                  {
                     foundCard.Copies.Add(foundCard.Copy());
                  }
               }
            }
         }

         int sum = cards.Select(c => c.Copies.Count()).Sum() + cards.Count();
         Console.WriteLine(sum);
         Console.ReadLine();

         bool TryFindExistingCard(int id, out Card foundCard)
         {
            foundCard = cards.FirstOrDefault(x => x.Id == id);
            return foundCard != null;
         }
      }

      private class Card
      {
         public int Id { get; }
         public int[] WinningNumbers { get; }
         public int[] PlayerNumbers { get; }
         public List<Card> Copies { get; } = new List<Card>();

         public Card(int id, int[] winningNumbers, int[] playerNumbers)
         {
            Id = id;
            WinningNumbers = winningNumbers;
            PlayerNumbers = playerNumbers;
         }

         public Card(string rawCard)
         {
            string[] cardParts = rawCard.Split(':');

            Id = int.Parse(cardParts[0].Split(' ').Last().Trim());

            string[] numberParts = cardParts[1].Split("|");
            WinningNumbers = ExtractAllNumbers(numberParts[0]).ToArray();
            PlayerNumbers = ExtractAllNumbers(numberParts[1]).ToArray();
         }

         public Card Copy() => new Card(Id, WinningNumbers, PlayerNumbers);

         private static IEnumerable<int> ExtractAllNumbers(string rawNumbers)
         {
            return rawNumbers.Split(' ')
               .Where(str => !string.IsNullOrWhiteSpace(str))
               .Select(nr => int.Parse(nr
               .Trim()));
         }
      }
   }
}