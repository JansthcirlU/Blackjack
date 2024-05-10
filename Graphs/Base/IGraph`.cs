namespace Graphs.Base;

public interface IGraph<TNode, TEdge, TValue>
    where TNode : INode<TValue>
    where TEdge : IEdge<TNode, TValue>
{
    IEnumerable<TNode> Nodes { get; }
    IEnumerable<TEdge> Edges { get; }

    TNode AddNode(TValue value);
}
