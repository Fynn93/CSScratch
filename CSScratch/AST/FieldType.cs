namespace CSScratch.AST;

public class FieldType(string name, TypeRef valueType, bool isReadOnly)
    : TypeRef($"{(isReadOnly ? "read " : "")}{name}: {valueType.Path};")
{
    public string Name { get; } = name;
    public TypeRef ValueType { get; } = valueType;
    public bool IsReadOnly { get; } = isReadOnly;
}