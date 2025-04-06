namespace CSScratch.AST;

public class ParameterType(string? name, TypeRef type)
    : TypeRef(name != null ? $"{name}: {type.Path}" : type.Path)
{
    public string? Name { get; } = name;
    public TypeRef Type { get; } = type;
}