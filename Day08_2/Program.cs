using System.Numerics;
using System.Security.Cryptography;

namespace Day08_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         char[] instructions = allLines[0].ToCharArray();
         Dictionary<string, Tuple<string, string>> nodes = ReadMaps(allLines).ToDictionary(kv => kv.Key, kv => kv.Value);
         long individualSteps = 0;
         string[] curKeys = nodes.Where(x => x.Key.EndsWith('A')).Select(x => x.Key).ToArray();
         Dictionary<long, long> loopSteps = new Dictionary<long, long>();
         while (loopSteps.Count < curKeys.Length)
         {
            foreach (char c in instructions)
            {
               individualSteps++;
               for (int i = 0; i < curKeys.Length; i++)
               {
                  string curKey = curKeys[i];
                  curKeys[i] = c == 'L' ? nodes[curKey].Item1 : nodes[curKey].Item2;
                  if (curKeys[i].EndsWith('Z'))
                  {
                     if (!loopSteps.ContainsKey(i))
                     {
                        loopSteps[i] = individualSteps;
                     }
                  }
               }
            }
         }

         long lcm = 1;
         foreach (int value in loopSteps.Values)
         {
            lcm = CalculateLCM(lcm, value);
         }

         Console.WriteLine($"Least Common Multiple: {lcm}");
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
                .Select(part => new string(part.Where(c => char.IsLetterOrDigit(c)).ToArray()))
                .ToArray();

            yield return new KeyValuePair<string, Tuple<string, string>>(nodeParts[0], new Tuple<string, string>(nextNodes[0], nextNodes[1]));
         }
      }

      private static long CalculateLCM(long a, long b)
      {
         return (a * b) / CalculateGCD(a, b);
      }

      private static long CalculateGCD(long a, long b)
      {
         while (b != 0)
         {
            long temp = b;
            b = a % b;
            a = temp;
         }
         return a;
      }
   }
}