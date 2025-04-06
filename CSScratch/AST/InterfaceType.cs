namespace CSScratch.AST;

public sealed class InterfaceType : TypeRef
{
    public InterfaceType(HashSet<FieldType> fields, MappedType? extraMapping = null, bool? isCompact = null) : base("")
    {
        Fields = fields;
        ExtraMapping = extraMapping;
        IsCompact = (Fields.Count == 0 && isCompact == null) || (isCompact ?? false);
        Path = ToString()!;
    }

    public HashSet<FieldType> Fields { get; }
    public MappedType? ExtraMapping { get; }
    public bool IsCompact { get; }
}