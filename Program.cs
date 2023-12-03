using System.Diagnostics;
using AoC2023;

string day = args.Length > 0 ? args[0] : DateTime.Now.ToString("dd");
bool isTest = args.Length > 1 && args[1].StartsWith('t');

string input = File.ReadAllText($"./Inputs/Day{day}{(isTest ? "Test" : "")}.txt");

Type dayClassType = Type.GetType($"AoC2023.Day{day}") ?? throw new FileNotFoundException();

IDay dayClass = Activator.CreateInstance(dayClassType) as IDay ?? throw new TypeLoadException();

RunPart(true);
RunPart(false);

Environment.Exit(-1);

void RunPart(bool partOne)
{
    Stopwatch sw = Stopwatch.StartNew();
    string resultFromPart = partOne ? dayClass.PartOne(input) : dayClass.PartTwo(input);
    sw.Stop();
    Console.WriteLine(@$"--------------------------------
Result {(partOne ? "1" : "2")}: {resultFromPart}, Time taken: {sw.ElapsedMilliseconds}ms
--------------------------------");
}