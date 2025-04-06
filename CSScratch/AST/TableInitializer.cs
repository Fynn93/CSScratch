namespace CSScratch.AST;

public class TableInitializer(List<Expression>? values = null, List<Expression>? keys = null)
    : Expression
{
    public List<Expression> Values { get; } = values ?? [];
    public List<Expression> Keys { get; } = keys ?? [];

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}