using Graphs.Base;

namespace Graphs.Weighted;

public interface IWeightedEdge<TNode, TValue, TWeight> : IEdge<TNode, TValue>
    where TNode : INode<TValue>
{
    TWeight Weight { get; }
}