namespace CSScratch.AST;

public class ArrayType(TypeRef elementType) : TypeRef("{ " + elementType.Path + " }")
{
    public TypeRef ElementType { get; } = elementType;
}