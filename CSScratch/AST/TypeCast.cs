namespace CSScratch.AST;

public class TypeCast(Expression expression, TypeRef type) : Expression
{
    public Expression Expression { get; } = expression;
    public TypeRef Type { get; } = type;

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}