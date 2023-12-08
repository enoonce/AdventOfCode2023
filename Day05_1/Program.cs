namespace Day05_1
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         long[] seeds = allLines[0].Split(':')[1].Trim().Split(' ')
            .Select(s => long.Parse(s)).ToArray();

         List<Map> maps = ReadMaps(allLines);
         foreach (Map map in maps)
         {
            for (int i = 0; i < seeds.Length; i++)
            {
               Range range = map.SourceRanges.FirstOrDefault(range => range.Start <= seeds[i] && range.End >= seeds[i]);
               if (range != null)
               {
                  int rangeIndex = map.SourceRanges.IndexOf(range);

                  long rangeOffset = seeds[i] - range.Start;
                  seeds[i] = map.DestinationRanges[rangeIndex].Start + rangeOffset;
               }
            }
         }

         Console.WriteLine(seeds.Min());
         Console.ReadLine();
      }

      static List<Map> ReadMaps(string[] allLines)
      {
         List<Map> maps = new List<Map>();
         List<string> curMapLines = new List<string>();

         for (int i = 0; i < allLines.Length; i++)
         {
            if (allLines[i].EndsWith("map:"))
            {
               i++;
               while (i < allLines.Length && !string.IsNullOrEmpty(allLines[i]))
               {
                  curMapLines.Add(allLines[i]);
                  i++;
               }

               maps.Add(new Map(curMapLines));
               curMapLines.Clear();
            }
         }

         return maps;
      }
   }



   internal class Map
   {
      public List<Range> SourceRanges { get; } = new List<Range>();
      public List<Range> DestinationRanges { get; } = new List<Range>();

      public Map(List<string> mapData)
      {
         ParseRanges(mapData);
      }

      private void ParseRanges(List<string> mapData)
      {
         foreach (string line in mapData)
         {
            long[] parts = line.Split(' ').Select(p => long.Parse(p)).ToArray();
            DestinationRanges.Add(new Range(parts[0], parts[0] + parts[2] - 1));
            SourceRanges.Add(new Range(parts[1], parts[1] + parts[2] - 1));
         }
      }
   }

   internal class Range
   {
      public long Start { get; }
      public long End { get; }

      public Range(long start, long end)
      {
         Start = start;
         End = end;
      }
   }
}