namespace AoC2023;

public class Day15 : IDay
{
    struct SequenceStep
    {
        public SequenceStep(string str)
        {
            Label = string.Concat(str.TakeWhile(char.IsLetter));
            OpCode = str[Label.Length];
            Power = OpCode == '=' ? (str[^1] - '0') : -1;
            Hash = TheHASHalgorithm(Label);
        }
        public string Label { get; set; }
        public char OpCode { get; set; }
        public int Power { get; set; }
        public int Hash { get; set; }
    }
    private static int TheHASHalgorithm(string s) => s.Aggregate(0, (sum, val) => (val + sum) * 17 % 256);

    public string PartOne(string input) =>
        input.Split(',').Aggregate(0, (sum, val) => sum + TheHASHalgorithm(val)).ToString();

    public string PartTwo(string input)
    {
        var sequenceSteps = input.Split(',').Select(l => new SequenceStep(l));

        List<List<(string Label, int power)>> boxes = [];
        for (int i = 0; i < 256; i++)
            boxes.Add([]);

        foreach (var seqStep in sequenceSteps)
        {
            int index = boxes[seqStep.Hash].FindIndex(x => x.Label == seqStep.Label);
            if (seqStep.OpCode == '-' && index != -1)
                boxes[seqStep.Hash].RemoveAt(index);
            if (seqStep.OpCode == '=' && index == -1)
                boxes[seqStep.Hash].Add((seqStep.Label, seqStep.Power));
            if (seqStep.OpCode == '=' && index != -1)
                boxes[seqStep.Hash][index] = (seqStep.Label, seqStep.Power);
        }

        return boxes.Select((box, boxIdx) => box.Select((lens, lensIdx) => (1 + boxIdx) * (1 + lensIdx) * lens.power).Sum()).Sum().ToString();
    }
}
