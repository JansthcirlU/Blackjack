namespace Blackjack;

public readonly record struct Card(Rank Rank, Suit Suit, int Deck) : IEquivalent<Card>
{
    public bool IsEquivalentTo(Card other)
        => GetRankValue() == other.GetRankValue();

    public int GetRankValue()
        => Rank switch
        {
            Rank.Ace => 1,
            Rank.Ten or Rank.Jack or Rank.Queen or Rank.King => 10,
            Rank r => (int)r,
        };

    public override string ToString()
    {
        string rank = Rank switch
        {
            Rank.Ace => "A",
            Rank.Two => "2",
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "T",
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            _ => throw new InvalidOperationException("Invalid rank")
        };
        string suit = Suit switch
        {
            Suit.Clubs => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            Suit.Spades => "♠",
            _ => throw new InvalidOperationException("Invalid suit")
        };
        return $"{rank}{suit}";
    }
};