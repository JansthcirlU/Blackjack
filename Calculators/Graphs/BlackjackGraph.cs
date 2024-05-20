using System.Numerics;
using Blackjack.Game;
using Graphs.Unweighted;

namespace Calculators.Graphs;

public class BlackjackGraph : IUnweightedGraph<BlackjackNode, BlackjackEdge, GameState>
{
    private readonly LinkedList<BlackjackNode> _nodes = [];
    private readonly LinkedList<BlackjackEdge> _edges = [];

    public IEnumerable<BlackjackNode> Nodes => _nodes.AsEnumerable();
    public IEnumerable<BlackjackEdge> Edges => _edges.AsEnumerable();

    public BlackjackEdge AddEdge(BlackjackNode from, BlackjackNode to)
    {
        BlackjackEdge edge = new(from, to);
        _edges.AddLast(edge);
        return edge;
    }

    public BlackjackNode AddNode(GameState value)
    {
        BlackjackNode node = new(value);
        _nodes.AddLast(node);
        return node;
    }

    public BlackjackNode AddNodeWithCount(GameState value, BigInteger count)
    {
        BlackjackNode node = new(value, count);
        _nodes.AddLast(node);
        return node;
    }

    public IEnumerable<BlackjackNode> GetUnvisitedNodes(int count)
    {
        Queue<BlackjackNode> unvisitedNodes = new();
        int i = 0;
        foreach (BlackjackNode node in _nodes.Where(n => !n.Visited))
        {
            if (count == i++) break;

            unvisitedNodes.Enqueue(node);
        }

        while (unvisitedNodes.Count > 0)
        {
            yield return unvisitedNodes.Dequeue();
        }
    }
}