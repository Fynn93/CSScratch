namespace CSScratch.AST;

public class FunctionType : TypeRef
{
    public FunctionType(List<ParameterType> parameterTypes, TypeRef returnType)
        : base("")
    {
        ParameterTypes = parameterTypes;
        ReturnType = returnType;
        Path = $"({string.Join(", ", parameterTypes.Select(type => type.Path))}) -> {returnType.Path}";
    }

    public List<ParameterType> ParameterTypes { get; }
    public TypeRef ReturnType { get; }
}