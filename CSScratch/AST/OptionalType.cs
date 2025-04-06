namespace CSScratch.AST;

public class OptionalType(TypeRef nonNullableType) : TypeRef(nonNullableType.Path + "?")
{
    public TypeRef NonNullableType { get; } = nonNullableType;
}