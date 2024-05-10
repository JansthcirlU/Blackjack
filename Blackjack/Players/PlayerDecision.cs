namespace Blackjack.Players;

[Flags]
public enum PlayerDecision
{
    None = 0,
    Stand = 1,
    Hit = 2,
    DoubleDown = 4,
    Surrender = 8,
    Split = 16,
}