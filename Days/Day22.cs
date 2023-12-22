namespace AoC2023;

public class Day22 : IDay
{
    public record Point(int X, int Y, int Z);

    public class Block
    {
        public Block(string str)
        {
            var coords = str.Split("~").Select(s => s.Ints().ToArray()).Select(i => new Point(i[0], i[1], i[2]));
            Start = coords.First();
            End = coords.Last();
            ContainedPoints = GetContainedPoints(Start, End);
        }
        public Block(Point start, Point end)
        {
            Start = start;
            End = end;
            ContainedPoints = GetContainedPoints(start, End);
        }

        public Point Start { get; set; }
        public Point End { get; set; }
        public HashSet<Point> ContainedPoints { get; set; } = [];
        private static HashSet<Point> GetContainedPoints(Point startPoint, Point endPoint)
        {
            HashSet<Point> pointsInBlock = [];
            if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
            {
                for (int z = startPoint.Z; z < endPoint.Z + 1; z++)
                    pointsInBlock.Add(new Point(startPoint.X, startPoint.Y, z));
            }
            else if (startPoint.X == endPoint.X && startPoint.Z == endPoint.Z)
            {
                for (int y = startPoint.Y; y < endPoint.Y + 1; y++)
                    pointsInBlock.Add(new Point(startPoint.X, y, startPoint.Z));
            }
            else if (startPoint.Y == endPoint.Y && startPoint.Z == endPoint.Z)
            {
                for (int x = startPoint.X; x < endPoint.X + 1; x++)
                    pointsInBlock.Add(new Point(x, startPoint.Y, startPoint.Z));
            }
            else
                throw new Exception("!");
            return pointsInBlock;
        }
        public void Fall()
        {
            Start = new Point(Start.X, Start.Y, Start.Z - 1);
            End = new Point(End.X, End.Y, End.Z - 1);
            ContainedPoints = GetContainedPoints(Start, End);
        }
        public HashSet<Point> PointsOnFall() => ContainedPoints.Select(p => new Point(p.X, p.Y, p.Z - 1)).ToHashSet();

        public Block Clone() => new(Start, End);
    }

    static long StablizeBlocks(List<Block> blocks, HashSet<Point> allBlockPoints)
    {
        var fallen = new HashSet<Block>();
        while (true)
        {
            bool somethingFell = false;
            foreach (var block in blocks)
            {
                if (block.Start.Z == 1 || block.End.Z == 1)
                    continue;

                foreach (var point in block.ContainedPoints)
                {
                    allBlockPoints.Remove(point);
                }

                bool canFall = block.PointsOnFall().All(np => !allBlockPoints.Contains(np));
                if (canFall)
                {
                    block.Fall();
                    somethingFell = true;
                    fallen.Add(block);
                }
                foreach (var point in block.ContainedPoints)
                {
                    allBlockPoints.Add(point);
                }
            }
            if (!somethingFell)
                return fallen.Count;
        }
    }

    public string PartOne(string input)
    {
        var blocks = input.Lines().Select(l => new Block(l)).ToList();
        var allBlockPoints = blocks.SelectMany(b => b.ContainedPoints).ToHashSet();
        
        _ = StablizeBlocks(blocks, allBlockPoints);

        var ans = 0;
        foreach (var block in blocks)
        {
            var oneRemoved = blocks.Where(bc => bc.Start != block.Start).ToList();
            foreach (var point in block.ContainedPoints)
            {
                allBlockPoints.Remove(point);
            }
            bool somethingFell = false;
            foreach (var insideBlock in oneRemoved)
            {
                if (insideBlock.Start.Z == 1 || insideBlock.End.Z == 1)
                    continue;

                foreach (var point in insideBlock.ContainedPoints)
                {
                    allBlockPoints.Remove(point);
                }

                var canFall = insideBlock.PointsOnFall().All(np => !allBlockPoints.Contains(np));

                foreach (var point in insideBlock.ContainedPoints)
                {
                    allBlockPoints.Add(point);
                }

                if (canFall)
                {
                    somethingFell = true;
                    break;
                }
            }
            if (!somethingFell)
            {
                ans++;
            }
            foreach (var point in block.ContainedPoints)
            {
                allBlockPoints.Add(point);
            }
        }
        return ans.ToString();
    }
    // 480, time taken: 6mins -> Yikes!
    // Now down to 6s, still super slow, use brain (math) for ranges instead of creating every point?

    public string PartTwo(string input)
    {
        var blocks = input.Lines().Select(l => new Block(l)).OrderBy(b => b.Start.Z).ToList();
        var allBlockPoints = blocks.SelectMany(b => b.ContainedPoints).ToHashSet();

        _ = StablizeBlocks(blocks, allBlockPoints);

        var ans = 0L;
        foreach (Block block in blocks)
        {
            var oneRemovedClone = blocks.Where(b => b != block).Select(b => b.Clone()).ToList();
            var oneRemovedBlockPoints = oneRemovedClone.SelectMany(b => b.ContainedPoints).ToHashSet();
            ans += StablizeBlocks(oneRemovedClone, oneRemovedBlockPoints);
        }

        return ans.ToString();
    }
    // 84021
}