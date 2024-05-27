using System.Collections.Immutable;

namespace Blackjack;

public class Shoe : IEquivalent<Shoe>
{
    private ImmutableArray<Card> _cards;

    private Shoe() {}
    
    public static Shoe Create(int decks)
    {
        ImmutableArray<Card>.Builder builder = ImmutableArray.CreateBuilder<Card>(decks * 52);
        for (int i = 0; i < decks; i++)
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank in Enum.GetValues<Rank>())
                {
                    builder.Add(new Card(rank, suit, i));
                }
            }
        }
        Shoe shoe = new()
        {
            _cards = builder.MoveToImmutable()
        };
        return shoe;
    }

    public static Shoe CreateFrom(Shoe shoe)
        => new()
        {
            _cards = [.. shoe._cards]
        };
    
    public IEnumerable<(ImmutableArray<Card> Drawn, Shoe Remaining)> GetPossibleDrawsAndRemainingShoe(int cards)
    {
        if (cards == 1)
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                yield return ([_cards[i]], new() { _cards = [.. _cards[..i], .. _cards[(i + 1)..]] });
            }
        }
        else
        {
            foreach ((ImmutableArray<Card> next, Shoe remaining) in GetPossibleDrawsAndRemainingShoe(1))
            {
                foreach ((ImmutableArray<Card> subsequent, Shoe final) in remaining.GetPossibleDrawsAndRemainingShoe(cards - 1))
                {
                    yield return ([..next, ..subsequent], final);
                }
            }
        }
    }

    public bool IsEquivalentTo(Shoe other)
    {
        if (other is null) return false;
        if (_cards.Length != other._cards.Length) return false;

        for (int i = 0; i < _cards.Length; i++)
        {
            if (!_cards[i].IsEquivalentTo(other._cards[i])) return false;
        }

        return true;
    }

    public override string ToString()
        => string.Join(string.Empty, _cards.Select(c => c.ToString()));
}