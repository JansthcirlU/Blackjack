using Blackjack.Scoring;

namespace Blackjack.Players;

public class PlayerHand
{
    private readonly List<Card> _cards = [];

    public Guid Id { get; }
    public decimal Bet { get; }
    public PlayerHandState State { get; }
    public PlayerDecision AllowedDecisions { get; }

    private PlayerHand(decimal bet, IEnumerable<Card> cards, PlayerHandState state, PlayerDecision allowedDecisions)
    {
        Id = Guid.NewGuid();
        Bet = bet;
        _cards.AddRange(cards);
        State = state;
        AllowedDecisions = allowedDecisions;
    }

    public static PlayerHand StartWith(Card first, Card second, decimal bet)
    {
        PlayerHandState state = PlayerHandState.InPlay;
        PlayerDecision allowedDecisions = PlayerDecision.Stand | PlayerDecision.Hit | PlayerDecision.DoubleDown | PlayerDecision.Surrender;
        if (first.IsEquivalent(second)) allowedDecisions |= PlayerDecision.Split;

        PlayerHand hand = new(bet, [first, second], state, allowedDecisions);
        return hand;
    }

    public PlayerHand Stand()
        => new(Bet, [.. _cards], PlayerHandState.Stand, PlayerDecision.None);

    public PlayerHand Hit(Card card)
    {
        List<Card> cardsAfterHit = [.. _cards, card];
        PlayerDecision allowedDecisions = PlayerDecision.Stand | PlayerDecision.Hit | PlayerDecision.DoubleDown | PlayerDecision.Surrender;
        if (Scorer.IsBust(cardsAfterHit)) return new(Bet, cardsAfterHit, PlayerHandState.Bust, PlayerDecision.None);
        if (Scorer.IsBlackjack(cardsAfterHit)) return new(Bet, cardsAfterHit, PlayerHandState.Blackjack, PlayerDecision.None);
        return new(Bet, cardsAfterHit, PlayerHandState.InPlay, allowedDecisions);
    }

    public PlayerHand DoubleDown(Card card)
    {
        decimal betAfterDoubleDown = Bet * 2;
        List<Card> cardsAfterDoubleDown = [.. _cards, card];
        PlayerDecision decisionsAfterDoubleDown = PlayerDecision.None;
        return Scorer.IsBust(cardsAfterDoubleDown)
            ? new(betAfterDoubleDown, cardsAfterDoubleDown, PlayerHandState.Bust, decisionsAfterDoubleDown)
            : new(betAfterDoubleDown, cardsAfterDoubleDown, PlayerHandState.DoubleDown, decisionsAfterDoubleDown);
    }

    public (PlayerHand, PlayerHand) Split(Card first, Card second)
    {
        PlayerHand firstHand = StartWith(_cards[0], first, Bet);
        PlayerHand secondHand = StartWith(_cards[1], second, Bet);
        return (firstHand, secondHand);
    }

    public PlayerHand Surrender()
        => new(Bet, [.. _cards], PlayerHandState.Surrender, PlayerDecision.None);

    public override string ToString()
        => string.Join(string.Empty, _cards.Select(c => c.ToString()));
}
