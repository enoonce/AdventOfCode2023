using System.Diagnostics;

namespace Day06_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         int[] times = ExtractNumbersFromLine(allLines[0]);
         int[] distances = ExtractNumbersFromLine(allLines[1]);
         List<Race> races = new List<Race>();
         for (int i = 0; i < times.Length; i++)
         {
            races.Add(new Race(times[i], distances[i]));
         }

         int[] allWinningCombinations = new int[races.Count];
         for (int i = 0; i < races.Count; i++)
         {
            allWinningCombinations[i] = GetWinningCombinations(races[i]);
         }

         int result = 1;
         for (int i = 0; i < allWinningCombinations.Length; i++)
         {
            result *= allWinningCombinations[i];
         }
         Console.WriteLine(result);
         Console.ReadLine();


         int GetWinningCombinations(Race race)
         {
            int winningCombos = 0;
            for (int i = 0; i <= race.time; i++)
            {
               int chargeMillis = i;
               int timeAvailable = race.time - chargeMillis;
               int speed = 1 * i;
               int distance = timeAvailable * speed;

               if (distance > race.distance)
               {
                  winningCombos++;
               }
            }

            return winningCombos;
         }
      }

      private static int[] ExtractNumbersFromLine(string line)
      {
         return line.Split(':')[1]
            .Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => int.Parse(x.Trim()))
            .ToArray();
      }
   }

   internal record Race(int time, int distance);
}