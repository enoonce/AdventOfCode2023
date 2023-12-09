namespace Day06_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         long time = ExtractNumberFromLine(allLines[0]);
         long distance = ExtractNumberFromLine(allLines[1]);
         Race race = new Race(time, distance);

         long winningCombinations = GetWinningCombinations(race);

         Console.WriteLine(winningCombinations);
         Console.ReadLine();


         long GetWinningCombinations(Race race)
         {
            long winningCombos = 0;
            for (int i = 0; i <= race.time; i++)
            {
               int chargeMillis = i;
               long timeAvailable = race.time - chargeMillis;
               long speed = 1 * i;
               long distance = timeAvailable * speed;

               if (distance > race.distance)
               {
                  winningCombos++;
               }
            }

            return winningCombos;
         }
      }

      private static long ExtractNumberFromLine(string line)
      {
         string concatenatedNumbers = string.Join("", line
            .Split(':')[1]
            .Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x)));

         return long.Parse(concatenatedNumbers);
      }
   }

   internal record Race(long time, long distance);
}