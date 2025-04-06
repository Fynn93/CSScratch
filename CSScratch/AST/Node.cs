namespace CSScratch.AST;

public abstract class Node
{
    private List<Node>? _descendants;
    public Node? Parent { get; private set; }
    public List<Node> Children { get; } = [];

    public List<Node> Descendants
    {
        get
        {
            if (_descendants != null) return _descendants;
            _descendants = [];
            foreach (var child in Children)
            {
                _descendants.Add(child);
                _descendants.AddRange(child.Descendants);
            }

            return _descendants;
        }
        set => _descendants = value;
    }

    public abstract void Compile(GbWriter writer);

    protected void AddChild(Node child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    protected void AddChildren(IEnumerable<Node> children)
    {
        foreach (var child in children) AddChild(child);
    }
}