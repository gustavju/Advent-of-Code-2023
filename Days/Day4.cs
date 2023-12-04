namespace AoC2023;

public class Day4 : IDay
{
    public string PartOne(string input)
    {
        var totalPoints = 0;
        foreach (var line in input.Lines())
        {
            var parts = line.Split(':')[1];
            var numbers = parts.Split("|").Select(p => p.Ints());
            var currentMatches = numbers.First().Intersect(numbers.Last()).Count();

            var points = 0;
            for (int i = 0; i < currentMatches; i++)
            {
                points = points == 0 ? 1 : points * 2;
            }
            totalPoints += points;
        }
        return totalPoints.ToString();
    }

    public string PartTwo(string input)
    {
        var ticketsCount = new Dictionary<int, int>();
        var lines = input.Lines().ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var parts = line.Split(':')[1];
            var id = line.Ints().First();
            var numbers = parts.Split("|").Select(p => p.Ints());
            var currentMatches = numbers.First().Intersect(numbers.Last()).Count();

            if (!ticketsCount.TryAdd(i + 1, 1))
                ticketsCount[i + 1] += 1;

            for (int j = 1; j <= currentMatches && (i + 1 + j) <= lines.Length; j++)
            {
                if (!ticketsCount.TryAdd(i + 1 + j, ticketsCount[i + 1]))
                    ticketsCount[i + 1 + j] += ticketsCount[i + 1];
            }
        }
        return ticketsCount.Select(d => d.Value).Sum().ToString();
    }
}

