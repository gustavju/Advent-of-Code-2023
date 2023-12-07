using System.Text;
using System.Text.RegularExpressions;

namespace AoC2023;

public static partial class Extensions
{
    public static List<int> AllIndexesOf(this string str, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("the string to find may not be empty", nameof(value));
        }

        List<int> indexes = [];
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
            {
                return indexes;
            }
            indexes.Add(index);
        }
    }

    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
    {
        if (sequence == null)
        {
            yield break;
        }

        List<T> list = sequence.ToList();

        if (list.Count == 0)
        {
            yield return Enumerable.Empty<T>();
        }
        else
        {
            int startingElementIndex = 0;

            foreach (var startingElement in list)
            {
                var index = startingElementIndex;
                var remainingItems = list.Where((e, i) => i != index);

                foreach (var permutationOfRemainder in remainingItems.Permute())
                {
                    yield return permutationOfRemainder.Prepend(startingElement);
                }

                startingElementIndex++;
            }
        }
    }

    // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
    // in lexicographic order (first [0, 1, 2, ..., m-1]).
    private static IEnumerable<int[]> Combinations(int m, int n)
    {
        int[] result = new int[m];
        Stack<int> stack = new(m);
        stack.Push(0);
        while (stack.Count > 0)
        {
            int index = stack.Count - 1;
            int value = stack.Pop();
            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);

                if (index != m)
                {
                    continue;
                }

                yield return (int[])result.Clone();
                break;
            }
        }
    }

    public static IEnumerable<T[]> Combinations<T>(T[] array, int m)
    {
        if (array.Length < m)
        {
            throw new ArgumentException("Array length can't be less than number of selected elements");
        }

        if (m < 1)
        {
            throw new ArgumentException("Number of selected elements can't be less than 1");
        }

        T[] result = new T[m];
        foreach (int[] j in Combinations(m, array.Length))
        {
            for (int i = 0; i < m; i++)
            {
                result[i] = array[j[i]];
            }
            yield return result;
        }
    }
    public static T[,] To2D<T>(this T[][] source)
    {
        try
        {
            int firstDim = source.Length;
            int secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[firstDim, secondDim];
            for (int i = 0; i < firstDim; ++i)
                for (int j = 0; j < secondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }

    public static string ReplaceAt(this string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }
        StringBuilder builder = new(input);
        builder[index] = newChar;
        return builder.ToString();
    }

    public static IEnumerable<string> Lines(this string input, bool keepEmptyLines = false)
    {
        foreach (var line in input.Split(Environment.NewLine).Where(l => keepEmptyLines || !string.IsNullOrEmpty(l.Trim())))
        {
            yield return line;
        }
    }

    public static IEnumerable<string> Words(this string line)
    {
        foreach (var word in line.Split(" "))
        {
            yield return word;
        }
    }

    public static int LowestCommonMultiple(this IEnumerable<int> source)
    {
        int lcm = 1;
        foreach (var x in source)
        {
            lcm *= x;
        }
        return lcm;
    }

    [GeneratedRegex("-?\\d+")]
    private static partial Regex IntRegex();

    public static IEnumerable<int> Ints(this string line) =>
        IntRegex().Matches(line).Select(m => m.Value).Select(int.Parse);

    public static IEnumerable<long> Longs(this string line) =>
        IntRegex().Matches(line).Select(m => m.Value).Select(long.Parse);

    public static int Mod(this int x, int mod) => ((x % mod) + mod) % mod;

    public static int[,] TrimArray(this int[,] originalArray, int rowToRemove, int columnToRemove)
    {
        int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

        for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
        {
            if (i == rowToRemove)
            {
                continue;
            }

            for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
            {
                if (k == columnToRemove)
                {
                    continue;
                }

                result[j, u] = originalArray[i, k];
                u++;
            }
            j++;
        }

        return result;
    }

    public static bool IsInBounds<T>(this T[][] matrix, int r, int c)
        => r < matrix.Length && r >= 0 && c < matrix[0].Length && c >= 0;
}