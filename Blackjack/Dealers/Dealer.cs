using System.Collections.Immutable;
using Blackjack.Scoring;

namespace Blackjack.Dealers;

public class Dealer : IEquivalent<Dealer>
{
    private readonly ImmutableArray<Card> _hand;
    public bool HitsSoft { get; }
    public DealerState State { get; }
    public DealerDecision AllowedDecisions { get; }
    public bool CanPlay => State == DealerState.InPlay;

    private Dealer(IEnumerable<Card> cards, bool hitsSoft, DealerState state, DealerDecision allowedDecisions)
    {
        _hand = [.. cards];
        HitsSoft = hitsSoft;
        State = state;
        AllowedDecisions = allowedDecisions;
    }

    public static Dealer StartWith(Card first, Card second, bool hitsSoft)
    {
        ImmutableArray<Card> cards = [first, second];
        if (Scorer.IsBlackjack(cards)) return new(cards, hitsSoft, DealerState.Blackjack, DealerDecision.None);

        BlackjackScore score = Scorer.GetScore(cards);
        (DealerState state, DealerDecision allowedDecisions) = GetDealerStateAndDecision(score, hitsSoft);
        return new(cards, hitsSoft, state, allowedDecisions);
    }

    public IEnumerable<(Dealer dealer, Shoe remaining)> GetNextDealerStates(Shoe shoe)
    {
        if (AllowedDecisions == DealerDecision.None) yield break;

        if ((AllowedDecisions & DealerDecision.Hit) == DealerDecision.Hit)
        {
            foreach ((IEnumerable<Card> drawn, Shoe remaining) in shoe.GetPossibleDrawsAndRemainingShoe(1))
            {
                ImmutableArray<Card> handAfterHit = [.. _hand, drawn.First()];
                BlackjackScore score = Scorer.GetScore(handAfterHit);
                (DealerState state, DealerDecision allowedDecisions) = GetDealerStateAndDecision(score, HitsSoft);
                yield return (new(handAfterHit, HitsSoft, state, allowedDecisions), remaining);
            }
        }
    }

    private static (DealerState state, DealerDecision allowedDecisions) GetDealerStateAndDecision(BlackjackScore score, bool hitsSoft)
        => score switch
        {
            { Total: > 21 } => (DealerState.Bust, DealerDecision.None),
            { Total: < 17 } => (DealerState.InPlay, DealerDecision.Hit),
            { IsSoft: true } => hitsSoft ? (DealerState.InPlay, DealerDecision.Hit) : (DealerState.Stand, DealerDecision.None),
            _ => (DealerState.Stand, DealerDecision.None)
        };

    public override string ToString()
        => $"D({string.Join(string.Empty, _hand.Select(c => c.ToString()))})";

    public bool IsEquivalentTo(Dealer other)
    {
        if (other is null) return false;
        if (!_hand.IsEquivalentTo(other._hand)) return false;

        return HitsSoft == other.HitsSoft
            && State == other.State
            && AllowedDecisions == other.AllowedDecisions;
    }
}