namespace Day09_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         List<Reading> readings = new List<Reading>();
         foreach (string line in allLines)
         {
            readings.Add(new Reading(line.Split(' ').Select(c => long.Parse(c)).ToArray()));
         }

         long total = 0;
         foreach (Reading reading in readings)
         {
            reading.History.Last().Add(0);
            for (int i = reading.History.Count - 2; i >= 0; i--)
            {
               long lastVal = reading.History[i].Last();
               reading.History[i].Add(lastVal + reading.History[i + 1].Last());
            }

            total += reading.History[0].Last();
         }

         Console.WriteLine(total);
         Console.ReadLine();
      }
   }

   internal class Reading
   {
      public List<List<long>> History { get; set; } = new List<List<long>>();

      public Reading(long[] values)
      {
         GeneratePredictions(values);
      }

      private void GeneratePredictions(long[] values)
      {
         List<long> curValues = values.ToList();
         History.Add(curValues);

         bool isCompleted = false;
         List<long> newValues;
         while (!isCompleted)
         {
            isCompleted= true;
            newValues = new List<long>();
            for (int i = 1; i < curValues.Count; i++)
            {
               long curVal = curValues[i];
               long difference = curVal - curValues[i - 1];
               newValues.Add(difference);

               if (isCompleted == true && difference != 0)
               {
                  isCompleted = false;
               }
            }
            History.Add(newValues);
            curValues = newValues;
         }
      }
   }
}