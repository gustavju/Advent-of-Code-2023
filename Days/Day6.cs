namespace AoC2023;

public class Day6 : IDay
{
    public string PartOne(string input)
    {
        var times = input.Lines().First().Ints().ToArray();
        var distances = input.Lines().Last().Ints().ToArray();
        var races = times.Zip(distances, (time, distance) => (time, distance));

        int ret = 1;
        foreach (var (time, distance) in races)
        {
            ret *= Race(time, distance);
        }

        return ret.ToString();
    }

    private static int Race(int time, int distance)
    {
        var waysToWin = 0;
        for (int x = 0; x <= time; x++)
        {
            var dx = x * (time - x);
            if (dx > distance)
            {
                waysToWin++;
            }
        }
        return waysToWin;
    }

    public string PartTwo(string input)
    {
        var times = input.Lines().First().Longs();
        var time = long.Parse(string.Join("", times));

        var distances = input.Lines().Last().Longs();
        var distance = long.Parse(string.Join("", distances));

        var waysToWin = 0;
        for (long x = 0; x <= time; x++)
        {
            var dx = x * (time - x);
            if (dx > distance)
            {
                waysToWin++;
            }
        }
        return waysToWin.ToString();
    }
}
