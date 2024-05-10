namespace Blackjack.Scoring;

public static class Scorer
{
    public static BlackjackScore GetScore(IEnumerable<Card> cards)
    {
        int score = 0;
        int aceCount = 0;
        bool isSoft = false;

        foreach (Card card in cards)
        {
            score += card.Rank switch
            {
                Rank.Ace => 1,
                Rank.Jack => 10,
                Rank.Queen => 10,
                Rank.King => 10,
                _ => (int)card.Rank
            };

            if (card.Rank == Rank.Ace) aceCount++;
        }

        if (score <= 11 && aceCount > 0)
        {
            score += 10;
            isSoft = true;
        }

        return new(score, isSoft);
    }

    public static BlackjackScore GetScore(params Card[] cards)
        => GetScore(cards);

    public static bool IsBlackjack(IEnumerable<Card> cards)
        => cards.ToArray() is [Card first, Card second]
        &&  (
                first.Rank is Rank.Ace && second.Rank is Rank.Ten or Rank.Jack or Rank.Queen or Rank.King
                ||
                second.Rank is Rank.Ace && first.Rank is Rank.Ten or Rank.Jack or Rank.Queen or Rank.King
            );
    
    public static bool IsBust(IEnumerable<Card> cards)
        => GetScore(cards).Total > 21;
}