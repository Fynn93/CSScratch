namespace CSScratch.AST;

public abstract class MappedType(TypeRef keyType, TypeRef valueType)
    : TypeRef($"{{ [{keyType.Path}]: " + valueType.Path + "; }")
{
    public TypeRef KeyType { get; } = keyType;
    public TypeRef ValueType { get; } = valueType;
}