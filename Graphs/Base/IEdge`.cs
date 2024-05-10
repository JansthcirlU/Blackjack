namespace Graphs.Base;

public interface IEdge<TNode, TValue>
    where TNode : INode<TValue>
{
    TNode From { get; }
    TNode To { get; }
}