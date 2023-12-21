namespace AoC2023;

public class Day21 : IDay
{
    enum Direction { East = 0, South = 1, West = 2, North = 3 };
    readonly List<(int r, int c)> directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    public string PartOne(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();

        int startR = 0;
        int startC = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] == 'S')
                {
                    startR = r;
                    startC = c;
                }
            }
        }

        Queue<(int r, int c, int steps)> queue = [];
        queue.Enqueue((startR, startC, 0));
        HashSet<(int r, int c)> seen = [];
        HashSet<(int r, int c)> ans = [];
        while (queue.Count != 0)
        {
            var (r, c, steps) = queue.Dequeue();
            Console.WriteLine((r, c, steps));
            if (!grid.IsInBounds(r, c) || grid[r][c] == '#')
                continue;

            if (seen.Contains((r, c)))
                continue;
            seen.Add((r, c));

            if (steps <= 64 && steps % 2 == 0)
            {
                ans.Add((r, c));
            }

            foreach (var dir in directions)
            {
                var rr = r + dir.r;
                var cc = c + dir.c;
                queue.Enqueue((rr, cc, steps + 1));
            }
        }
        return ans.Count.ToString();
    }

    public string PartTwo(string input)
    {
        // :(
        throw new NotImplementedException();
    }
}
