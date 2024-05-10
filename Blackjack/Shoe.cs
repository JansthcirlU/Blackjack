namespace Blackjack;

public class Shoe
{
    private List<Card> _cards = [];

    private Shoe() {}
    
    public static Shoe Create(int decks)
    {
        Shoe shoe = new();
        for (int i = 0; i < decks; i++)
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank in Enum.GetValues<Rank>())
                {
                    shoe._cards.Add(new Card(rank, suit, i));
                }
            }
        }
        return shoe;
    }

    public static Shoe CreateFrom(Shoe shoe)
        => new()
        {
            _cards = [.. shoe._cards]
        };
    
    public IEnumerable<(IEnumerable<Card> Drawn, Shoe Remaining)> GetPossibleDrawsAndRemainingShoe(int cards)
    {
        if (cards == 1)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                yield return ([_cards[i]], new() { _cards = [.. _cards[..i], .. _cards[(i + 1)..]] });
            }
        }
        else
        {
            foreach ((IEnumerable<Card> next, Shoe remaining) in GetPossibleDrawsAndRemainingShoe(1))
            {
                foreach ((IEnumerable<Card> subsequent, Shoe final) in remaining.GetPossibleDrawsAndRemainingShoe(cards - 1))
                {
                    yield return (next.Concat(subsequent), final);
                }
            }
        }
    }

    public override string ToString()
        => string.Join(string.Empty, _cards.Select(c => c.ToString()));
}