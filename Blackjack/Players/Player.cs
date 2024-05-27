using System.Collections.Immutable;

namespace Blackjack.Players;

public class Player : IEquivalent<Player>
{
    private readonly ImmutableArray<PlayerHand> _hands;

    public Guid Id { get; }
    public bool CanPlay => _hands.Any(h => h.State == PlayerHandState.InPlay);

    private Player(IEnumerable<PlayerHand> hands)
    {
        Id = Guid.NewGuid();
        _hands = [.. hands];
    }

    public static Player StartWith(Card first, Card second, decimal bet)
    {
        PlayerHand hand = PlayerHand.StartWith(first, second, bet);
        return new([hand]);
    }

    public IEnumerable<(Player player, Shoe remaining)> GetNextPlayerStates(Shoe shoe)
    {
        if (_hands.All(h => h.State != PlayerHandState.InPlay)) yield break;

        for (int i = 0; i < _hands.Length; i++)
        {
            PlayerHand hand = _hands[i];
            if (hand.State != PlayerHandState.InPlay) continue;

            if ((hand.AllowedDecisions & PlayerDecision.Stand) == PlayerDecision.Stand)
            {
                PlayerHand standHand = hand.Stand();
                ImmutableArray<PlayerHand> handsAfterStand = [.. _hands[..i], standHand, .. _hands[(i + 1)..]];
                Player playerAfterStand = new(handsAfterStand);
                Shoe remaining = Shoe.CreateFrom(shoe);
                yield return (playerAfterStand, remaining);
            }

            if ((hand.AllowedDecisions & PlayerDecision.Surrender) == PlayerDecision.Surrender)
            {
                PlayerHand surrenderHand = hand.Surrender();
                ImmutableArray<PlayerHand> handsAfterSurrender = [.. _hands[..i], surrenderHand, .. _hands[(i + 1)..]];
                Player playerAfterSurrender = new(handsAfterSurrender);
                Shoe remaining = Shoe.CreateFrom(shoe);
                yield return (playerAfterSurrender, remaining);
            }

            if ((hand.AllowedDecisions & PlayerDecision.Hit) == PlayerDecision.Hit)
            {
                foreach ((IEnumerable<Card> drawn, Shoe remaining) in shoe.GetPossibleDrawsAndRemainingShoe(1))
                {
                    PlayerHand hitHand = hand.Hit(drawn.First());
                    ImmutableArray<PlayerHand> handsAfterHit = [.. _hands[..i], hitHand, .. _hands[(i + 1)..]];
                    Player playerAfterHit = new(handsAfterHit);
                    yield return (playerAfterHit, remaining);
                }
            }

            if ((hand.AllowedDecisions & PlayerDecision.DoubleDown) == PlayerDecision.DoubleDown)
            {
                foreach ((IEnumerable<Card> drawn, Shoe remaining) in shoe.GetPossibleDrawsAndRemainingShoe(1))
                {
                    PlayerHand doubleDownHand = hand.DoubleDown(drawn.First());
                    ImmutableArray<PlayerHand> handsAfterDoubleDown = [.. _hands[..i], doubleDownHand, .. _hands[(i + 1)..]];
                    Player playerAfterDoubleDown = new(handsAfterDoubleDown);
                    yield return (playerAfterDoubleDown, remaining);
                }
            }

            if ((hand.AllowedDecisions & PlayerDecision.Split) == PlayerDecision.Split)
            {
                foreach ((IEnumerable<Card> drawn, Shoe remaining) in shoe.GetPossibleDrawsAndRemainingShoe(2))
                {
                    (PlayerHand first, PlayerHand second) = hand.Split(drawn.First(), drawn.Last());
                    ImmutableArray<PlayerHand> handsAfterSplit = [.. _hands[..i], first, second, .. _hands[(i + 1)..]];
                    Player playerAfterSplit = new(handsAfterSplit);
                    yield return (playerAfterSplit, remaining);
                }
            }
        }
    }

    public bool IsEquivalentTo(Player other)
    {
        Dictionary<PlayerHand, int> equivalenceCounts = GetEquivalenceCounts(_hands);
        Dictionary<PlayerHand, int> otherEquivalenceCounts = GetEquivalenceCounts(other._hands);
        
        try
        {
            foreach ((PlayerHand key, int value) in equivalenceCounts)
            {
                PlayerHand? otherKey = otherEquivalenceCounts.Keys.SingleOrDefault(k => k.IsEquivalentTo(key));
                if (otherKey is null) return false;
                if (otherEquivalenceCounts[otherKey] != value) return false;
            }
            return true;   
        }
        catch
        {
            return false;
        }
    }

    private static Dictionary<PlayerHand, int> GetEquivalenceCounts(ImmutableArray<PlayerHand> hands)
    {
        Dictionary<PlayerHand, int> equivalentHandsDictionary = [];

        foreach (PlayerHand hand in hands)
        {
            PlayerHand? equivalentHand = equivalentHandsDictionary.Keys.SingleOrDefault(h => h.IsEquivalentTo(hand));
            
            if (equivalentHand is null)
            {
                equivalentHandsDictionary[hand] = 1;
            }
            else
            {
                equivalentHandsDictionary[equivalentHand]++;
            }
        }

        return equivalentHandsDictionary;
    }

    public override string ToString()
        => $"P([{string.Join(", ", _hands.Select(h => h.ToString()))}])";
}