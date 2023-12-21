namespace AoC2023;

public class Day12 : IDay
{
    Dictionary<(int, int, int), long> DP = [];

    private long Solve(string dots, List<int> blocks, int i, int bi, int current)
    {
        var entryKey = (i, bi, current);
        if (DP.TryGetValue(entryKey, out long value))
            return value;

        if (i == dots.Length)
        {
            if (bi == blocks.Count && current == 0)
                return 1;
            if (bi == blocks.Count - 1 && blocks[bi] == current)
                return 1;
            else
                return 0;
        }
        var ans = 0L;
        List<char> cs = ['.', '#'];
        foreach (char c in cs)
        {
            if (dots[i] == c || dots[i] == '?')
            {
                if (c == '.' && current == 0)
                {
                    ans += Solve(dots, blocks, i + 1, bi, 0);
                }
                else if (c == '.' && current > 0 && bi < blocks.Count && blocks[bi] == current)
                {
                    ans += Solve(dots, blocks, i + 1, bi + 1, 0);
                }
                else if (c == '#')
                {
                    ans += Solve(dots, blocks, i + 1, bi, current + 1);
                }
            }
        }
        DP[entryKey] = ans;
        return ans;
    }

    public string PartOne(string input)
    {
        var patternToCounts = input.Lines().Select(l => l.Split(" ")).Select(l => new { dots = l[0], block = l[1].Ints().ToList() });
        var res = 0L;
        foreach (var patternAndCount in patternToCounts)
        {
            DP.Clear();
            res += Solve(patternAndCount.dots, patternAndCount.block, 0, 0, 0);
        }
        return res.ToString();
    }

    public string PartTwo(string input)
    {
        var patternToCounts = input.Lines().Select(l => l.Split(" ")).Select(l => new { dots = l[0], block = l[1].Ints().ToList() });
        var res = 0L;
        foreach (var patternAndCount in patternToCounts)
        {
            var dots = String.Join('?', [patternAndCount.dots, patternAndCount.dots, patternAndCount.dots, patternAndCount.dots, patternAndCount.dots]);
            var block = Enumerable.Repeat(patternAndCount.block, 5).SelectMany(t => t).ToList();
            DP.Clear();
            res += Solve(dots, block, 0, 0, 0);
            Console.WriteLine((dots, String.Join(',', block), res));
        }
        return res.ToString();
    }
}
