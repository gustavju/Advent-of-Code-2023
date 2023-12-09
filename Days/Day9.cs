namespace AoC2023;

public class Day9 : IDay
{
    public string PartOne(string input)
    {
        long ans = 0;
        foreach (var line in input.Lines())
        {
            var differences = ExtrapolateSequence(line.Longs().ToList());
            ans += differences[0].Last();
        }
        return ans.ToString();
    }

    public static List<List<long>> ExtrapolateSequence(List<long> sequence)
    {
        var differences = new List<List<long>> { sequence };

        do
        {
            var prevSequence = differences.Last();
            List<long> nextSequence = [];
            for (var i = 0; i < prevSequence.Count - 1; i++)
            {
                var difference = prevSequence[i + 1] - prevSequence[i];
                nextSequence.Add(difference);
            }
            differences.Add(nextSequence);

            if (nextSequence.All(s => s == 0))
                break;
        } while (true);

        for (var i = differences.Count - 1; i > 0; i--)
        {
            differences[i - 1].Add(differences[i - 1].Last() + differences[i].Last());
            differences[i - 1].Insert(0, differences[i - 1].First() - differences[i].First());
        }
        return differences;
    }

    public string PartTwo(string input)
    {
        long ans = 0;
        foreach (var line in input.Lines())
        {
            var differences = ExtrapolateSequence(line.Longs().ToList());
            ans += differences[0].First();
        }
        return ans.ToString();
    }
}