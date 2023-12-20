namespace AoC2023;

public class Day16 : IDay
{
    enum Direction { East = 0, South = 1, West = 2, North = 3 };
    readonly List<(int r, int c)> directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    (int r, int c, Direction dir) Move(int r, int c, Direction dir) =>
        (r + directions[(int)dir].r, c + directions[(int)dir].c, dir);

    int GetScore(char[][] grid, int startR = 0, int startC = 0, Direction startDir = Direction.East)
    {
        Queue<(int r, int c, Direction dir)> queue = [];
        queue.Enqueue((startR, startC, startDir));

        HashSet<(int r, int c)> seen = [];
        HashSet<(int, int, Direction)> prev = [];

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (!grid.IsInBounds(current.r, current.c) || prev.Contains(current))
                continue;
            
            seen.Add((current.r, current.c));
            prev.Add(current);

            (int r, int c, Direction dir) = current;
            var sqr = grid[r][c];
            // Now check out this badboi!
            if (sqr == '.' ||
                (sqr == '|' && (dir == Direction.North || dir == Direction.South)) ||
                (sqr == '-' && (dir == Direction.East || dir == Direction.West)))
            {
                queue.Enqueue(Move(r, c, dir));
            }
            else if ((sqr == '/' && dir == Direction.East) ||
                     (sqr == '\\' && dir == Direction.West))
            {
                queue.Enqueue(Move(r, c, Direction.North));
            }
            else if ((sqr == '\\' && dir == Direction.East) ||
                     (sqr == '/' && dir == Direction.West))
            {
                queue.Enqueue(Move(r, c, Direction.South));
            }
            else if ((sqr == '\\' && dir == Direction.South) ||
                     (sqr == '/' && dir == Direction.North))
            {
                queue.Enqueue(Move(r, c, Direction.East));
            }
            else if ((sqr == '/' && dir == Direction.South) ||
                     (sqr == '\\' && dir == Direction.North))
            {
                queue.Enqueue(Move(r, c, Direction.West));
            }
            else if (sqr == '|' && (dir == Direction.East || dir == Direction.West))
            {
                queue.Enqueue(Move(r, c, Direction.North));
                queue.Enqueue(Move(r, c, Direction.South));
            }
            else if (sqr == '-' && (dir == Direction.South || dir == Direction.North))
            {
                queue.Enqueue(Move(r, c, Direction.East));
                queue.Enqueue(Move(r, c, Direction.West));
            }
        }
        return seen.Count;
    }

    public string PartOne(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();
        return GetScore(grid).ToString();
    }

    public string PartTwo(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();
        
        var ans = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            ans = Math.Max(ans, GetScore(grid, r, 0, Direction.East));
            ans = Math.Max(ans, GetScore(grid, r, grid[0].Length - 1, Direction.West));
        }
        for (int c = 0; c < grid[0].Length; c++)
        {
            ans = Math.Max(ans, GetScore(grid, 0, c, Direction.South));
            ans = Math.Max(ans, GetScore(grid, grid.Length - 1, c, Direction.North));
        }
        return ans.ToString();
    }
}