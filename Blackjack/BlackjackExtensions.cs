namespace Blackjack;

public static class BlackjackExtensions
{
    public static bool IsEquivalentTo(this IEnumerable<Card> cards, IEnumerable<Card> other)
    {
        if (cards.Count() != other.Count()) return false;

        Dictionary<int, int> thisHandRanks = cards
            .Select(c => c.GetRankValue())
            .GroupBy(rank => rank)
            .ToDictionary(g => g.Key, g => g.Count());
        Dictionary<int, int> otherHandRanks = other
            .Select(c => c.GetRankValue())
            .GroupBy(rank => rank)
            .ToDictionary(g => g.Key, g => g.Count());
        
        return thisHandRanks.Count == otherHandRanks.Count
            && !thisHandRanks.Except(otherHandRanks).Any();
    }
}