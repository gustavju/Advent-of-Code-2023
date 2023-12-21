namespace AoC2023;

public class Day17 : IDay
{
    readonly List<(int r, int c)> directions = [(-1, 0), (0, -1), (1, 0), (0, 1)];
    private static bool IsValidMovePart1(int nextMovesInDirection) => nextMovesInDirection <= 3;
    private static bool IsValidMovePart2(int currentDirection, int currentMovesInDirection, int nextDirection, int nextMovesInDirection) =>
        nextMovesInDirection <= 10 && (nextDirection == currentDirection || currentMovesInDirection >= 4 || currentMovesInDirection == -1);

    private int Move(char[][] grid, bool part2 = false)
    {
        var seen = new Dictionary<(int r, int c, int dir, int movesInDir), int>();
        var queue = new PriorityQueue<(int distance, int r, int c, int dir, int movesInDir), int>();
        queue.Enqueue((0, 0, 0, -1, -1), 0);

        while (queue.Count > 0)
        {
            var (distance, r, c, currentDirection, currentMovesInDirection) = queue.Dequeue();
            if (seen.ContainsKey((r, c, currentDirection, currentMovesInDirection)))
            {
                continue;
            }
            seen[(r, c, currentDirection, currentMovesInDirection)] = distance;
            foreach ((int dr, int dc) ddir in directions)
            {
                var rr = r + ddir.dr;
                var cc = c + ddir.dc;
                var nextDirection = directions.IndexOf(ddir);
                var nextMovesInDirection = nextDirection != currentDirection ? 1 : currentMovesInDirection + 1;
                var isReverse = (nextDirection + 2) % 4 == currentDirection;
                var isValidMove = part2 ? IsValidMovePart2(currentDirection, currentMovesInDirection, nextDirection, nextMovesInDirection) : IsValidMovePart1(nextMovesInDirection);

                if (grid.IsInBounds(rr, cc) && !isReverse && isValidMove)
                {
                    int cost = grid[rr][cc] - '0';
                    queue.Enqueue((distance + cost, rr, cc, nextDirection, nextMovesInDirection), distance + cost);
                }

            }
        }
        return seen.Where(item => item.Key.r == grid.Length - 1 && item.Key.c == grid[0].Length - 1).Min(e => e.Value);
    }
    
    public string PartOne(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();
        return Move(grid).ToString();
    }

    public string PartTwo(string input)
    {
        var grid = input.Lines().Select(l => l.ToCharArray()).ToArray();
        return Move(grid, true).ToString();
    }

}