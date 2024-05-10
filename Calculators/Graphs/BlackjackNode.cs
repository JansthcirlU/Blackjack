using Blackjack.Game;
using Graphs.Base;

namespace Calculators.Graphs;

public class BlackjackNode : IVisitableNode<GameState>
{
    public GameState Value { get; }
    public bool Visited { get; private set; }

    public BlackjackNode(GameState value)
    {
        Value = value;
        Visited = false;
    }

    public void Visit()
        => Visited = true;
}
