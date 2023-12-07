namespace AoC2023;

public class Day7 : IDay
{
    static char[] cardOrder = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];
    record Hand : IComparable
    {
        public Hand(string[] arr, bool p2 = false)
        {
            OrgCards = arr[0];
            Cards = OrgCards;
            if (p2 && Cards.Contains('J'))
            {
                foreach (var group in Cards.GroupBy(c => c).OrderByDescending(grp => grp.Count()))
                {
                    if (group.Key != 'J')
                        Cards = Cards.Replace('J', group.Key);
                }
            }
            Bet = int.Parse(arr[1]);
            BaseRank = GetBaseRank();
        }
        public string Cards { get; set; }
        public string OrgCards { get; set; }
        public int Bet { get; set; }
        public int BaseRank { get; set; }

        public int CompareTo(object? obj)
        {
            var other = obj as Hand;
            if (BaseRank != other?.BaseRank)
                return BaseRank.CompareTo(other?.BaseRank);

            for (int i = 0; i < Cards.Length; i++)
            {
                var strength = Array.IndexOf(cardOrder, OrgCards.ElementAt(i));
                var otherStrenght = Array.IndexOf(cardOrder, other.OrgCards.ElementAt(i));
                if (strength != otherStrenght)
                    return otherStrenght.CompareTo(strength);
            }
            throw new Exception();
        }

        private int GetBaseRank()
        {
            var grps = Cards.GroupBy(c => c).Select(grp => grp.Count()).OrderDescending().ToArray();

            if (grps[0] == 5) // 5
                return 6;
            if (grps[0] == 4) // 4 of kind
                return 5;
            if (grps[0] == 3 && grps[1] == 2) // house
                return 4;
            if (grps[0] == 3 && grps[1] == 1 && grps[2] == 1) // 3 of kind
                return 3;
            if (grps[0] == 2 && grps[1] == 2) // two pairs
                return 2;
            if (grps[0] == 2) // Pair
                return 1;
            if (grps[0] == 1) // high
                return 0;

            throw new Exception();
        }
    }
    public string PartOne(string input)
    {
        var hands = input.Lines().Select(l => new Hand(l.Split(" "))).Order();
        var sum = 0;
        for (int i = 1; i <= hands.Count(); i++)
        {
            sum += hands.ElementAt(i - 1).Bet * i;
        }
        return sum.ToString();
    }

    public string PartTwo(string input)
    {
        cardOrder = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];
        var hands = input.Lines().Select(l => new Hand(l.Split(" "), true)).Order();
        var sum = 0;
        for (int i = 1; i <= hands.Count(); i++)
        {
            sum += hands.ElementAt(i - 1).Bet * i;
        }
        return sum.ToString();
    }
}
