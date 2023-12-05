using System.Text;

namespace Day03_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");
         IEnumerable<int> partNumbers = FindPartNumbersRecursive(allLines, 0, prevLineSymbolIndexes: new List<int>(), GetLineSymbols(allLines[1]).ToList());

         Console.WriteLine(partNumbers.Sum());
         Console.ReadLine();
      }

      private static IEnumerable<int> FindPartNumbersRecursive(string[] allLines, int curLineIndex, List<int> prevLineSymbolIndexes, List<int> nextLineSymbolIndexes)
      {
         string line = allLines[curLineIndex];

         List<int> symbolIndexes = GetLineSymbols(line).ToList();

         StringBuilder tempNumSB = new StringBuilder();
         int tempNumStartIndex = -1;
         for (int i = 0; i < line.Length; i++)
         {
            char curChar = line[i];
            if (char.IsDigit(curChar))
            {
               if (tempNumSB.Length == 0)
               {
                  tempNumStartIndex = i;
               }

               tempNumSB.Append(curChar);
            }
            
            if (tempNumStartIndex > -1 && (!char.IsDigit(curChar) || i == line.Length - 1))
            {
               bool isValidNumber = false;

               int start = tempNumStartIndex > 0 ? tempNumStartIndex - 1 : 0;
               int end = tempNumStartIndex + tempNumSB.Length;
               if (end > line.Length - 1)
               {
                  end = line.Length - 1;
               }
               for (int j = start; j <= end; j++)
               {
                  if (symbolIndexes.Contains(j) || prevLineSymbolIndexes.Contains(j) || nextLineSymbolIndexes.Contains(j))
                  {
                     isValidNumber = true;
                     break;
                  }
               }

               if (isValidNumber)
               {
                  yield return int.Parse(tempNumSB.ToString());
               }

               tempNumSB.Clear();
               tempNumStartIndex = -1;
            }
         }

         if (curLineIndex < allLines.Length - 1)
         {
            curLineIndex++;
            List<int> newNextLineSymbols = curLineIndex == allLines.Length - 1 ? new List<int>() : GetLineSymbols(allLines[curLineIndex + 1]).ToList();
            foreach (int num in FindPartNumbersRecursive(allLines, curLineIndex, prevLineSymbolIndexes: symbolIndexes, nextLineSymbolIndexes: newNextLineSymbols))
            {
               yield return num;
            }
         }
      }

      private static IEnumerable<int> GetLineSymbols(string line)
      {
         for (int i = 0; i < line.Length; i++)
         {
            char curChar = line[i];
            if (char.IsPunctuation(curChar) && !curChar.Equals('.') || char.IsSymbol(curChar))
            {
               yield return i;
            }
         }
      }
   }
}