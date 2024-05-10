using Blackjack.Game;
using Graphs.Base;

namespace Calculators.Graphs;

public record BlackjackEdge(BlackjackNode From, BlackjackNode To) : IEdge<BlackjackNode, GameState>;