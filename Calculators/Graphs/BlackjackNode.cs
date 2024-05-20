using System.Numerics;
using Blackjack.Game;
using Graphs.Base;

namespace Calculators.Graphs;

public class BlackjackNode : IVisitableNode<GameState>
{
    public GameState Value { get; }
    public BigInteger Count { get; private set; }
    public bool Visited { get; private set; }

    public BlackjackNode(GameState value)
    {
        Value = value;
        Visited = false;
        Count = 1;
    }
    public BlackjackNode(GameState value, BigInteger count)
    {
        Value = value;
        Visited = false;
        Count = count;
    }

    public void Visit()
        => Visited = true;

    public void IncrementCount()
        => Count++;
}
