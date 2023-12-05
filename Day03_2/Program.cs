using System.Collections.Generic;
using System.Text;

namespace Day03_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         IEnumerable<Dictionary<int, string>> allPartSets = FindPartsRecursive(allLines, 0, prevLineSymbolIndexes: new Dictionary<int, char>(), GetLineSymbols(allLines[1]));

         List<Dictionary<int, char>> allSymbolSets = new List<Dictionary<int, char>>();
         foreach (string line in allLines)
         {
            allSymbolSets.Add(GetLineSymbols(line));
         }

         int gearRatioSum = GetGearRatios(allPartSets, allSymbolSets);

         Console.WriteLine(gearRatioSum);
         Console.ReadLine();
      }

      private static IEnumerable<Dictionary<int, string>> FindPartsRecursive(string[] allLines, int curLineIndex, Dictionary<int, char> prevLineSymbolIndexes, Dictionary<int, char> nextLineSymbolIndexes)
      {
         string line = allLines[curLineIndex];

         Dictionary<int, char> lineSymbols = GetLineSymbols(line);
         Dictionary<int, string> parts = new Dictionary<int, string>();

         StringBuilder tempNum = new StringBuilder();
         int tempNumStartIndex = -1;
         for (int i = 0; i < line.Length; i++)
         {
            char curChar = line[i];
            if (char.IsDigit(curChar))
            {
               if (tempNum.Length == 0)
               {
                  tempNumStartIndex = i;
               }

               tempNum.Append(curChar);
            }

            if (tempNumStartIndex > -1 && (!char.IsDigit(curChar) || i == line.Length - 1))
            {
               int start = tempNumStartIndex > 0 ? tempNumStartIndex - 1 : 0;
               int end = tempNumStartIndex + tempNum.Length;
               if (end > line.Length - 1)
               {
                  end = line.Length - 1;
               }
               for (int j = start; j <= end; j++)
               {
                  if (lineSymbols.Keys.Contains(j) || prevLineSymbolIndexes.Keys.Contains(j) || nextLineSymbolIndexes.Keys.Contains(j))
                  {
                     parts.Add(tempNumStartIndex, tempNum.ToString());
                     break;
                  }
               }

               tempNum.Clear();
               tempNumStartIndex = -1;
            }
         }

         yield return parts;

         if (curLineIndex < allLines.Length - 1)
         {
            curLineIndex++;
            Dictionary<int, char> newNextLineSymbols = curLineIndex == allLines.Length - 1 ? new Dictionary<int, char>() : GetLineSymbols(allLines[curLineIndex + 1]);
            foreach (Dictionary<int, string> partSet in FindPartsRecursive(allLines, curLineIndex, prevLineSymbolIndexes: lineSymbols, nextLineSymbolIndexes: newNextLineSymbols))
            {
               yield return partSet;
            }
         }
      }

      private static Dictionary<int, char> GetLineSymbols(string line)
      {
         Dictionary<int, char> lineSymbols = new Dictionary<int, char>();
         for (int i = 0; i < line.Length; i++)
         {
            char curChar = line[i];
            if (char.IsPunctuation(curChar) && !curChar.Equals('.') || char.IsSymbol(curChar))
            {
               lineSymbols.Add(i, curChar);
            }
         }

         return lineSymbols;
      }

      private static int GetGearRatios(IEnumerable<Dictionary<int, string>> allPartSets, List<Dictionary<int, char>> allSymbolSets)
      {
         var potentialGearSets = allSymbolSets.Select(set => set
            .Where(x => x.Value.Equals('*'))).ToList();

         int gearRatioSum = 0;
         List<Dictionary<int, string>> allPartSetsList = allPartSets.ToList();
         for (int i = 0; i < potentialGearSets.Count(); i++)
         {
            Dictionary<int, string> curLineParts = allPartSetsList[i];
            Dictionary<int, string> prevLineParts = i > 0 ? allPartSetsList[i - 1] : new Dictionary<int, string>();
            Dictionary<int, string> nextLineParts = i < potentialGearSets.Count - 1 ? allPartSetsList[i + 1] : new Dictionary<int, string>();

            foreach (var gear in potentialGearSets[i])
            {
               int start = gear.Key - 1;
               int end = gear.Key + 1;

               List<int> validParts = ExtractValidParts(curLineParts, start, end)
                   .Concat(ExtractValidParts(prevLineParts, start, end))
                   .Concat(ExtractValidParts(nextLineParts, start, end))
                   .ToList();

               if (validParts.Count == 2)
               {
                  int gearRatio = validParts[0] * validParts[1];
                  gearRatioSum += gearRatio;
               }
            }
         }

         return gearRatioSum;
      }

      private static List<int> ExtractValidParts(Dictionary<int, string> lineParts, int start, int end)
      {
         List<int> validParts = new List<int>();
         for (int i = 0; i < lineParts.Count; i++)
         {
            string curPart = lineParts.ElementAt(i).Value;
            int curPartStart = lineParts.ElementAt(i).Key;
            int curPartEnd = curPartStart + curPart.Length - 1;
            for (int j = curPartStart; j <= curPartEnd; j++)
            {
               if (j >= start && j <= end)
               {
                  validParts.Add(int.Parse(curPart));
                  break;
               }
            }
         }

         return validParts;
      }
   }
}