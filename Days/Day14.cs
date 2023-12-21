namespace AoC2023;

public class Day14 : IDay
{
    public string PartOne(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();
        TiltNorth(grid);
        return GridScore(grid).ToString();
    }

    private static void TiltNorth(char[][] grid)
    {
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] != 'O')
                    continue;

                var currentR = r;
                while (grid.IsInBounds(currentR - 1, c) && grid[currentR - 1][c] == '.')
                    currentR--;

                if (r == currentR)
                    continue;

                grid[currentR][c] = 'O';
                grid[r][c] = '.';
            }
        }
    }

    private static void TiltSouth(char[][] grid)
    {
        for (int r = grid.Length - 1; r >= 0; r--)
        {
            for (int c = grid[0].Length - 1; c >= 0; c--)
            {
                if (grid[r][c] != 'O')
                    continue;

                var currentR = r;
                while (grid.IsInBounds(currentR + 1, c) && grid[currentR + 1][c] == '.')
                    currentR++;

                if (r == currentR)
                    continue;

                grid[currentR][c] = 'O';
                grid[r][c] = '.';
            }
        }
    }

    private static void TiltWest(char[][] grid)
    {
        for (int c = 0; c < grid[0].Length; c++)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                if (grid[r][c] != 'O')
                    continue;

                var currentC = c;
                while (grid.IsInBounds(r, currentC - 1) && grid[r][currentC - 1] == '.')
                    currentC--;

                if (c == currentC)
                    continue;

                grid[r][currentC] = 'O';
                grid[r][c] = '.';
            }
        }
    }

    private static void TiltEast(char[][] grid)
    {
        for (int c = grid[0].Length - 1; c >= 0; c--)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                if (grid[r][c] != 'O')
                    continue;

                var currentC = c;
                while (grid.IsInBounds(r, currentC + 1) && grid[r][currentC + 1] == '.')
                    currentC++;

                if (c == currentC)
                    continue;

                grid[r][currentC] = 'O';
                grid[r][c] = '.';
            }
        }
    }


    private static int GridScore(char[][] grid)
    {
        var score = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] == 'O')
                    score += grid.Length - r;
            }
        }
        return score;
    }
    public string PartTwo(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();

        Dictionary<string, int> cache = [];
        var cycle = 1;

        while (cycle <= 1_000_000_000)
        {
            TiltNorth(grid);
            TiltWest(grid);
            TiltSouth(grid);
            TiltEast(grid);

            var current = String.Join(String.Empty, grid.SelectMany(c => c));

            if (cache.TryGetValue(current, out var cached))
            {
                var remaining = 1_000_000_000 - cycle - 1;
                var loop = cycle - cached;

                var loopRemaining = remaining % loop;
                cycle = 1_000_000_000 - loopRemaining - 1;
            }

            cache[current] = cycle++;
        }
        Console.WriteLine(String.Join(Environment.NewLine, grid.Select(r => String.Join("", r))));
        return GridScore(grid).ToString();
    }
}