namespace CSScratch.AST;

public class AttributeList(List<BaseAttribute> attributes, bool inline) : Statement
{
    public List<BaseAttribute> Attributes { get; } = attributes;
    public bool Inline { get; set; } = inline;

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}