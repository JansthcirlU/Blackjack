using Graphs.Base;

namespace Graphs.Weighted;

public interface IWeightedGraph<TNode, TEdge, TValue, TWeight> : IGraph<TNode, TEdge, TValue>
    where TNode : INode<TValue>
    where TEdge : IWeightedEdge<TNode, TValue, TWeight>
{
    TEdge AddEdge(TNode from, TNode to, TWeight weight);
}
