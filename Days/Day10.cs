namespace AoC2023;

public class Day10 : IDay
{
    public static List<(int, int)> TileToDirs(char c) =>
   c switch
   {
       '|' => [(-1, 0), (1, 0)],
       '-' => [(0, -1), (0, 1)],
       'L' => [(-1, 0), (0, 1)],
       'J' => [(-1, 0), (0, -1)],
       '7' => [(1, 0), (0, -1)],
       'F' => [(1, 0), (0, 1)],
       '.' => [],
       'S' => [(-1, 0), (1, 0), (0, -1), (0, 1)],
       _ => throw new ArgumentException("Invalid enum value for command", nameof(c)),
   };

    public string PartOne(string input)
    {
        var matrix = input.Lines().Select(l => l.ToCharArray()).ToArray();
        var seen = new Dictionary<(int r, int c), int>();
        
        // This is nasty, fix?
        (int r, int c) start = (26, 31);
        matrix[start.r][start.c] = '|';

        var queue = new Queue<(int r, int c, int steps)>();
        queue.Enqueue((start.r, start.c, 0));

        while (queue.Count != 0)
        {
            var (r, c, steps) = queue.Dequeue();
            if (seen.ContainsKey((r, c)) || matrix[r][c] == '.')
            {
                continue;
            }
            seen.Add((r, c), steps);

            foreach (var connection in GetConnectedTiles((r, c), matrix))
            {
                queue.Enqueue((connection.r, connection.c, steps + 1));
            }
        }
        return seen.Values.Max().ToString();
    }

    private static List<(int r, int c)> GetConnectedTiles((int r, int c) tile, char[][] matrix)
    {
        var conn = new List<(int r, int c)>();
        foreach ((int r, int c) in TileToDirs(matrix[tile.r][tile.c]))
        {
            (int r, int c) next = (tile.r + r, tile.c + c);
            if (matrix.IsInBounds(next.r, next.c))
                conn.Add(next);
        }
        return conn;
    }

    public string PartTwo(string input)
    {
        var matrix = input.Lines().Select(l => l.ToCharArray()).ToArray();
        var seen = new Dictionary<(int r, int c), int>();
        
        // This is nasty, fix?
        (int r, int c) start = (26, 31);
        matrix[start.r][start.c] = '|';

        var queue = new Queue<(int r, int c, int steps)>();
        queue.Enqueue((start.r, start.c, 0));

        while (queue.Count != 0)
        {
            var (r, c, steps) = queue.Dequeue();
            if (seen.ContainsKey((r, c)) || matrix[r][c] == '.')
            {
                continue;
            }
            seen.Add((r, c), steps);

            foreach (var connection in GetConnectedTiles((r, c), matrix))
            {
                queue.Enqueue((connection.r, connection.c, steps + 1));
            }
        }

        /*
            Clean code is for amateurs. 
            Dive into the brilliance of the "if-else soup" – a chaotic masterpiece that keeps everyone guessing. 
            Why settle for readability when you can embrace the thrill of deciphering code mazes? 
            Complexity is an art form, and if-else soup is the avant-garde of programming chaos.
        */
        var count = 0;
        for (var r = 0; r < matrix.Length; r++)
        {
            var crosses = 0;
            char corner = ' ';
            for (var c = 0; c < matrix[r].Length; c++)
            {
                if (seen.ContainsKey((r, c)))
                {
                    var current = matrix[r][c];
                    if (current == '|')
                    {
                        crosses++;
                    }
                    else if (current != '-')
                    {
                        if (corner != ' ')
                        {
                            if ((corner == 'L' && current == '7') || (corner == 'F' && current == 'J'))
                            {
                                crosses++;
                            }
                            corner = ' ';
                        }
                        else
                        {
                            corner = current;
                        }
                    }
                }
                else if (crosses % 2 == 1)
                {
                    count++;
                }
            }
        }
        return count.ToString();
    }
}