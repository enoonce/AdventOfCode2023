namespace Day05_2
{
   internal class Program
   {
      static void Main(string[] args)
      {
         string[] allLines = File.ReadAllLines("input.txt");

         long[] seedParts = allLines[0].Split(':')[1].Trim().Split(' ')
            .Select(s => long.Parse(s)).ToArray();

         // Group seeds into ranges.
         List<Range> seedRanges = new List<Range>();
         for (int i = 0; i < seedParts.Length - 1; i += 2)
         {
            seedRanges.Add(new Range(seedParts[i], seedParts[i] + seedParts[i + 1] - 1));
         }

         List<Map> maps = ReadMaps(allLines);
         List<Range> mappedRanges = new List<Range>();
         for (int i = 0; i < seedRanges.Count; i++)
         {
            List<Range> convertedRanges = new List<Range>();
            List<Range> remainingRanges = new List<Range>() { seedRanges[i] };
            foreach (Map map in maps)
            {
               remainingRanges.AddRange(convertedRanges);
               convertedRanges.Clear();
               while (remainingRanges.Count > 0)
               {
                  for (int j = 0; j < remainingRanges.Count; j++)
                  {
                     Range remainingRange = remainingRanges[j];
                     foreach (Range mapRange in map.SourceRanges)
                     {
                        if (remainingRange.Start >= mapRange.Start && remainingRange.End <= mapRange.End)
                        {
                           // Fits whole inside the map range. Move on to the next seed range.
                           long offset = remainingRange.Start - mapRange.Start;
                           long length = remainingRange.End - remainingRange.Start;
                           int destinationIndex = map.SourceRanges.IndexOf(mapRange);
                           Range newRange = new Range(map.DestinationRanges[destinationIndex].Start + offset, map.DestinationRanges[destinationIndex].Start + offset + length);
                           convertedRanges.Add(newRange);
                           remainingRanges.Remove(remainingRange);
                           break;
                        }
                        else if (remainingRange.End < mapRange.Start || remainingRange.Start > mapRange.End)
                        {
                           // Out of range, proceed to the next map range to keep looking.
                           continue;
                        }
                        else if (remainingRange.Start < mapRange.Start && remainingRange.End >= mapRange.Start)
                        {
                           // Needs chopping left.
                           remainingRanges.Add(new Range(remainingRange.Start, mapRange.Start - 1));
                           remainingRanges.Add(new Range(mapRange.Start, mapRange.End));
                           remainingRanges.Remove(remainingRange);
                           break;
                        }
                        else if (remainingRange.Start <= mapRange.End && remainingRange.End > mapRange.End)
                        {
                           // Needs chopping right.
                           remainingRanges.Add(new Range(mapRange.End + 1, remainingRange.End));
                           remainingRanges.Add(new Range(remainingRange.Start, mapRange.End));
                           remainingRanges.Remove(remainingRange);
                           break;
                        }
                     }

                     if (remainingRanges.Contains(remainingRange))
                     {
                        convertedRanges.Add(remainingRange);
                        remainingRanges.Remove(remainingRange);
                     }
                  }
               }

            }
            mappedRanges.AddRange(convertedRanges);
         }

         Console.WriteLine(mappedRanges.Select(x => x.Start).Min());
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
      public long Start { get; set; }
      public long End { get; set; }

      public Range(long start, long end)
      {
         Start = start;
         End = end;
      }
   }
}