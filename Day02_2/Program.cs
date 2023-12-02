namespace Day02_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         int sumOfPowers = 0;

         string[] inputLines = File.ReadAllLines("input.txt");
         foreach (string line in inputLines)
         {
            string[] gameRecordParts = line.Split(':');
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

            int[] maxOfEachColor = gameSets
               .SelectMany(set => set)
               .GroupBy(entry => entry.Key)
               .Select(group => group
                  .Max(e => e.Value)).ToArray();

            int power = maxOfEachColor.Length > 0 ? 1 : 0;
            for (int i = 0; i < maxOfEachColor.Length; i++)
            {
               power *= maxOfEachColor[i];
            }

            sumOfPowers += power;
         }

         Console.WriteLine(sumOfPowers);
         Console.ReadLine();
      }

      private enum CubeColor { red, green, blue }
   }
}