using System.Reflection.Metadata;

namespace AoC2023;

public class Day11 : IDay
{
    private static string ExpandAndTraverse(string input, long expandBy)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();

        List<(int r, int c)> planets = [];
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == '#')
                    planets.Add((r, c));
            }
        }

        List<int> emptyRows = [];
        for (int r = 0; r < grid.Length; r++)
        {
            if (!planets.Any(p => p.r == r))
                emptyRows.Add(r);
        }

        List<int> emptyCols = [];
        for (int c = 0; c < grid[0].Length; c++)
        {
            if (!planets.Any(p => p.c == c))
                emptyCols.Add(c);

        }

        var res = 0L;
        int i = 0;
        foreach (var (r, c) in planets)
        {
            foreach (var (r2, c2) in planets.Skip(i))
            {
                var emptyRowCrossedCnt = emptyRows.Count(emptyRow => Math.Min(r, r2) <= emptyRow && emptyRow <= Math.Max(r, r2));
                var emptyColsCrossedCnt = emptyCols.Count(emptyCol => Math.Min(c, c2) <= emptyCol && emptyCol <= Math.Max(c, c2));
                res += Math.Abs(r2 - r) + Math.Abs(c2 - c) + ((emptyRowCrossedCnt + emptyColsCrossedCnt) * (expandBy - 1));
            }
            i++;
        }

        return res.ToString();
    }
    public string PartOne(string input) => ExpandAndTraverse(input, 2);
    public string PartTwo(string input) => ExpandAndTraverse(input, 100_000);
}
