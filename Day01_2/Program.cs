using System;
using System.Reflection;

namespace Day01_2
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
            int[] digits = new int[2];
            string curStr;
            for (int curSubstringEnd = 1; curSubstringEnd <= line.Length; curSubstringEnd++)
            {
               curStr = line.Substring(0, curSubstringEnd);

               if (TryGetDigit(curStr, out digits[0]))
               {
                  break;
               }
               else
               {
                  ThrowIfDigitsNotFoundOnLine(line);
               }
            }

            for (int curSubstringStart = line.Length - 1; curSubstringStart > -1; curSubstringStart--)
            {
               curStr = line.Substring(curSubstringStart);
               if (TryGetDigit(curStr, out digits[1]))
               {
                  break;
               }
               else
               {
                  ThrowIfDigitsNotFoundOnLine(line);
               }
            }

            int wholeNumber = int.Parse(string.Concat(digits.First(), digits.Last()));
            yield return wholeNumber;
         }
      }

      private static bool TryGetDigit(string subString, out int digit)
      {
         if (subString.Any(char.IsDigit))
         {
            digit = int.Parse(subString.First(x => char.IsDigit(x)).ToString());
            return true;
         }
         else
         {
            int numbersLength = Enum.GetNames(typeof(Numbers)).Length;
            for (int i = 0; i < numbersLength; i++)
            {
               string curNumberName = Enum.GetName(typeof(Numbers), i).ToLower();
               string found = string.Empty;
               int foundStartIndex = subString.IndexOf(curNumberName);
               if (foundStartIndex > -1)
               {
                  found = subString.Substring(foundStartIndex, curNumberName.Length);
                  if (!found.Equals(string.Empty))
                  {
                     if (!Enum.TryParse(found, out Numbers number))
                     {
                        throw new InvalidOperationException($"Could not convert {number} to int.");
                     }

                     digit = (int)number;
                     return true;
                  }
               }
            }
         }

         digit = -1;
         return false;
      }

      private static void ThrowIfDigitsNotFoundOnLine(string line)
      {
         throw new InvalidOperationException($"No digits found on line {line}.");
      }

      internal enum Numbers
      {
         one,
         two,
         three,
         four,
         five,
         six,
         seven,
         eight,
         nine,
      }
   }
}