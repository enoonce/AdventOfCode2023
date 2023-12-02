using System;

namespace Day02_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         int sumSuccessfulGames = 0;
         Dictionary<CubeColor, int> limits = new Dictionary<CubeColor, int>() { { CubeColor.red, 12 }, { CubeColor.green, 13 }, { CubeColor.blue, 14 } };

         string[] inputLines = File.ReadAllLines("input.txt");
         foreach (string line in inputLines)
         {
            string[] gameRecordParts = line.Split(':');
            int gameId = int.Parse(gameRecordParts[0].Split(' ').Last());
            string[] setsRaw = gameRecordParts[1].Split(';');

            Dictionary<CubeColor, int>[] gameSets = new Dictionary<CubeColor, int>[setsRaw.Length];
            for (int i = 0; i < setsRaw.Length; i++)
            {
               gameSets[i] = new Dictionary<CubeColor, int>();

               string[] cubeInfoRaw = setsRaw[i].Split(',');
               foreach (string cubeType in cubeInfoRaw)
               {
                  string[] cubeInfo = cubeType.Trim().Split(' ');
                  if (!Enum.TryParse(cubeInfo[1], out CubeColor color))
                  {
                     throw new InvalidOperationException($"Could not parse {cubeInfo[1]} to {nameof(CubeColor)}");
                  }
                  gameSets[i].Add(color, int.Parse(cubeInfo[0]));
               }
            }

            if (gameSets.Any(set => set.Any(entry => entry.Value > limits[entry.Key])))
            {
               continue;
            }

            sumSuccessfulGames += gameId;
         }

         Console.WriteLine(sumSuccessfulGames);
         Console.ReadLine();
      }

      private enum CubeColor { red, green, blue }
   }
}