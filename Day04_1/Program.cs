namespace Day04_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         int totalScore = 0;
         foreach (string line in allLines)
         {
            string[] cardParts = line.Split(':')
               .Last()
               .Split('|');

            IEnumerable<int> winningNumbers = ExtractAllNumbers(cardParts[0]);
            IEnumerable<int> playerNumbers = ExtractAllNumbers(cardParts[1]);

            int matches = playerNumbers.Count(s => winningNumbers.Contains(s));
            int score = matches > 0 ? 1 : 0;
            for (int i = 0; i < matches - 1; i++)
            {
               score *= 2;
            }
            totalScore += score;
            //totalScore += (int)Math.Pow(2, matches - 1);
         }

         Console.WriteLine(totalScore);
         Console.ReadLine();
      }

      private static IEnumerable<int> ExtractAllNumbers(string rawNumbers)
      {
         return rawNumbers.Split(' ')
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .Select(nr => int.Parse(nr
            .Trim()));
      }
   }
}