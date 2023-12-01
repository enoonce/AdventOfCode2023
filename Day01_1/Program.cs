using System.Reflection;

namespace Day01_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         //string assemblyPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).LocalPath;
         //string inputPath = Path.Combine(assemblyPath, "input.txt");
         string[]? inputLines = File.ReadAllLines("input.txt");

         IEnumerable<int> allDigits = GetAllDigits(inputLines);
         int sum = allDigits.Sum();

         Console.WriteLine(sum);
         Console.ReadLine();
      }

      private static IEnumerable<int> GetAllDigits(string[] allLines)
      {
         foreach (string line in allLines)
         {
            IEnumerable<char> digits = line.Where(c => char.IsDigit(c));
            int wholeDigit = int.Parse(string.Concat(digits.First(), digits.Last()));

            yield return wholeDigit;
         }
      }
   }
}