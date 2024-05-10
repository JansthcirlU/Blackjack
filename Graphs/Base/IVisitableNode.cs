namespace Graphs.Base;

public interface IVisitableNode<TValue> : INode<TValue>
{
    bool Visited { get; }
    void Visit();
}