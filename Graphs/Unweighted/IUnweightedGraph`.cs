using Graphs.Base;

namespace Graphs.Unweighted;

public interface IUnweightedGraph<TNode, TEdge, TValue> : IGraph<TNode, TEdge, TValue>
    where TNode : INode<TValue>
    where TEdge : IEdge<TNode, TValue>
{
    TEdge AddEdge(TNode from, TNode to);
}