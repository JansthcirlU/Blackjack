using System.Numerics;
using Blackjack;
using Blackjack.Game;
using Calculators.Graphs;

namespace Calculators;

public class BlackjackGraphCalculator
{
    public static BlackjackGraph GenerateGraph(BlackjackRules rules)
    {
        BlackjackGraph graph = InitialiseGraph(rules);
        while (true)
        {
            bool any = false;
            foreach (BlackjackNode node in graph.GetUnvisitedNodes(100))
            {
                any = true;
                node.Visit();
                foreach (GameState state in node.Value.GetNextStates())
                {
                    BlackjackNode next = graph.AddNode(state);
                    graph.AddEdge(node, next);
                }
            }
            if (!any) break;
        }
        return graph;
    }

    private static BlackjackGraph InitialiseGraph(BlackjackRules rules)
    {
        BlackjackGraph graph = new();
        IEnumerable<GameState> initialStates = GameState.GetInitialStates(rules);
        foreach (GameState state in initialStates)
        {
            BlackjackNode? node = graph.FindEquivalentNode(state);
            
            if (node is null)
            {
                graph.AddNode(state);
            }
            else
            {
                node.IncrementCount();
            }
        }
        return graph;
    }
}