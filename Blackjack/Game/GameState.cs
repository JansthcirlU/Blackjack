using Blackjack.Dealers;
using Blackjack.Players;

namespace Blackjack.Game;

public class GameState
{
    public Player Player { get; }
    public Dealer Dealer { get; }
    public Shoe Shoe { get; }

    public GameState(Player player, Dealer dealer, Shoe shoe)
    {
        Player = player;
        Dealer = dealer;
        Shoe = shoe;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public static IEnumerable<GameState> GetInitialStates(BlackjackRules rules)
    {
        Shoe shoe = Shoe.Create(rules.Decks);
        foreach ((IEnumerable<Card> playerCards, Shoe remainingAfterPlayer) in shoe.GetPossibleDrawsAndRemainingShoe(2))
        {
            Player player = Player.StartWith(playerCards.First(), playerCards.Last(), 1.0m);
            foreach ((IEnumerable<Card> dealerCards, Shoe remainingAfterDealer) in remainingAfterPlayer.GetPossibleDrawsAndRemainingShoe(2))
            {
                Dealer dealer = Dealer.StartWith(dealerCards.First(), dealerCards.Last(), rules.DealerHitsSoft);
                yield return new GameState(player, dealer, remainingAfterDealer);
            }
        }
    }

    public bool Equals(GameState? other)
        => other is not null && Id == other.Id;

    public IEnumerable<GameState> GetNextStates()
    {
        if (Player.CanPlay)
        {
            foreach ((Player player, Shoe remaining) in Player.GetNextPlayerStates(Shoe))
            {
                yield return new GameState(player, Dealer, remaining);
            }
        }
        else if (Dealer.CanPlay)
        {
            foreach ((Dealer dealer, Shoe remaining) in Dealer.GetNextDealerStates(Shoe))
            {
                yield return new GameState(Player, dealer, remaining);
            }
        }
        yield break;
    }
}
