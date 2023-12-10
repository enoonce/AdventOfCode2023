namespace Day08_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         char[] instructions = allLines[0].ToCharArray();
         Dictionary<string, Tuple<string, string>> nodes = ReadMaps(allLines).ToDictionary(kv => kv.Key, kv => kv.Value);
         int steps = 0;
         string curKey = "AAA";
         while (curKey != "ZZZ")
         {
            foreach (char c in instructions)
            {
               curKey = c == 'L' ? nodes[curKey].Item1 : nodes[curKey].Item2;
               steps++;
            }
         }

         Console.WriteLine(steps);
         Console.ReadLine();
      }

      private static IEnumerable<KeyValuePair<string, Tuple<string, string>>> ReadMaps(string[] mapData)
      {
         int startIndex = 0;
         while (!mapData[startIndex].Contains('='))
         {
            startIndex++;
         }

         for (int i = startIndex; i < mapData.Length; i++)
         {
            string[] nodeParts = mapData[i].Split('=').Select(part => part.Trim()).ToArray();
            string[] nextNodes = nodeParts[1].Split(',')
               .Select(part => new string(part.Where(char.IsLetter).ToArray()))
               .ToArray();

            yield return new KeyValuePair<string, Tuple<string, string>>(nodeParts[0], new Tuple<string, string>(nextNodes[0], nextNodes[1]));
         }
      }
   }
}