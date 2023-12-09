namespace AoC2023;

public class Day5 : IDay
{
    public string PartOne(string input)
    {
        var seeds = input.Lines().First().Longs();
        var maps = new Dictionary<int, List<(long dest, long start, long len)>>();

        var i = 0;
        foreach (var line in input.Lines())
        {
            if (line.Contains("map:"))
            {
                i++;
                maps.Add(i, []);
            }
            else if (char.IsDigit(line.First()))
            {
                var intsOnLine = line.Longs();
                maps[i].Add((intsOnLine.ElementAt(0), intsOnLine.ElementAt(1), intsOnLine.ElementAt(2)));
            }
        }

        var queue = new Queue<long>();
        foreach (var seed in seeds)
        {
            queue.Enqueue(seed);
        }
        var lst = new HashSet<long>();

        for (int m = 1; m <= maps.Count; m++)
        {
            lst.Clear();
            while (queue.Count != 0)
            {
                var currentSeed = queue.Dequeue();
                var applied = Apply(currentSeed, maps[m]);
                lst.Add(applied);
            }
            foreach (var newSeed in lst)
                queue.Enqueue(newSeed);
        }

        return queue.Min().ToString();
    }

    private static long Apply(long seed, List<(long dest, long start, long len)> map)
    {
        foreach (var (dest, start, len) in map)
            if (start <= seed && seed < (start + len))
                return seed + (dest - start);
        return seed;
    }

    public string PartTwo(string input)
    {
        var ranges = input.Lines().First().Longs().Chunk(2).Select(cnk => (from: cnk.First(), to: cnk.First() + cnk.Last() - 1)).ToList();
        var maps = new List<List<(long from, long to, long displacement)>>();
        List<(long from, long to, long displacement)> currentMap = [];

        foreach (var line in input.Lines())
        {
            if (line.Contains("map:"))
            {
                if (currentMap.Count > 0)
                    maps.Add(currentMap);
                currentMap = [];
            }
            else if (char.IsDigit(line.First()))
            {
                var longs = line.Longs().ToArray();
                currentMap.Add((longs[1], longs[1] + longs[2] - 1, longs[0] - longs[1]));
            }
        }
        maps.Add(currentMap);

        foreach (var map in maps)
        {
            var orderedmap = map.OrderBy(x => x.from).ToList();

            var newranges = new List<(long from, long to)>();
            foreach (var r in ranges)
            {
                (long rangeFrom, long rangeTo) = r;
                foreach (var (from, to, displacement) in orderedmap)
                {
                    if (rangeFrom < from)
                    {
                        newranges.Add((rangeFrom, Math.Min(rangeTo, from - 1)));
                        rangeFrom = from;
                        if (rangeFrom > rangeTo)
                            break;
                    }
 
                    if (rangeFrom <= to)
                    {
                        newranges.Add((rangeFrom + displacement, Math.Min(rangeTo, to) + displacement));
                        rangeFrom = to + 1;
                        if (rangeFrom > rangeTo)
                            break;
                    }
                }
                if (rangeFrom <= rangeTo)
                    newranges.Add((rangeFrom, rangeTo));
            }
            ranges = newranges;
        }

        return ranges.Min(r => r.from).ToString();
    }
}