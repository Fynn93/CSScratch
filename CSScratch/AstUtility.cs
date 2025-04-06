#region

using System.Reflection;
using System.Text.RegularExpressions;
using CSScratch.AST;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace CSScratch;

public static partial class AstUtility
{
    /// <summary>File path -> dictionary(identifier name, amount of times identifier is used)</summary>
    private static readonly Dictionary<string, Dictionary<string, uint>> IdentifierDeclarations = [];

    public static TableInitializer CreateTypeInfo(Type type)
    {
        List<Expression> keys =
        [
            new Literal("\"Name\""),
            new Literal("\"FullName\""),
            new Literal("\"Namespace\""),
            new Literal("\"AssemblyQualifiedName\""),
            new Literal("\"TypeInitializer\""),
            new Literal("\"ReflectedType\""),
            new Literal("\"IsAbstract\""),
            new Literal("\"IsAnsiClass\""),
            new Literal("\"IsArray\""),
            new Literal("\"IsSealed\""),
            new Literal("\"IsInterface\""),
            new Literal("\"IsGenericTypeParameter\""),
            new Literal("\"IsGenericTypeDefinition\""),
            new Literal("\"IsGenericType\""),
            new Literal("\"IsGenericMethodParameter\""),
            new Literal("\"IsConstructedGenericType\""),
            new Literal("\"IsImport\""),
            new Literal("\"IsClass\""),
            new Literal("\"IsCollectible\""),
            new Literal("\"IsByRef\""),
            new Literal("\"IsByRefLike\""),
            new Literal("\"IsAutoClass\""),
            new Literal("\"IsAutoLayout\""),
            new Literal("\"IsCOMObject\""),
            new Literal("\"IsContextful\""),
            new Literal("\"IsEnum\""),
            new Literal("\"IsExplicitLayout\""),
            new Literal("\"IsPointer\""),
            new Literal("\"IsFunctionPointer\""),
            new Literal("\"IsUnmanagedFunctionPointer\""),
            new Literal("\"IsLayoutSequential\""),
            new Literal("\"IsMarshalByRef\""),
            new Literal("\"IsNested\""),
            new Literal("\"IsNestedAssembly\""),
            new Literal("\"IsNestedFamily\""),
            new Literal("\"IsNestedFamANDAssem\""),
            new Literal("\"IsNestedFamORAssem\""),
            new Literal("\"IsNestedPrivate\""),
            new Literal("\"IsNestedPublic\""),
            new Literal("\"IsNotPublic\""),
            new Literal("\"IsPublic\""),
            new Literal("\"IsSZArray\""),
            new Literal("\"IsSecurityCritical\""),
            new Literal("\"IsSecuritySafeCritical\""),
            new Literal("\"IsSecurityTransparent\""),
            new Literal("\"IsSignatureType\""),
            new Literal("\"IsSpecialName\""),
            new Literal("\"IsTypeDefinition\""),
            new Literal("\"IsUnicodeClass\""),
            new Literal("\"IsValueType\""),
            new Literal("\"IsVariableBoundArray\""),
            new Literal("\"IsVisible\""),
            new Literal("\"UnderlyingSystemType\""),
            new Literal("\"BaseType\""),
            new Literal("\"DeclaringType\""),
            new Literal("\"ContainsGenericParameters\""),
            new Literal("\"GenericTypeArguments\""),
            new Literal("\"GUID\"")
        ];
        List<Expression> values =
        [
            new Literal($"\"{type.Name}\""),
            type.FullName != null ? new Literal($"\"{type.FullName}\"") : Void(),
            type.Namespace != null ? new Literal($"\"{type.Namespace}\"") : Void(),
            type.AssemblyQualifiedName != null ? new Literal($"\"{type.AssemblyQualifiedName}\"") : Void(),
            type.TypeInitializer != null ? CreateConstructorInfo(type.TypeInitializer) : Void(),
            type.ReflectedType != null ? CreateTypeInfo(type.ReflectedType) : Void(),
            new Literal(type.IsAbstract.ToString().ToLower()),
            new Literal(type.IsAnsiClass.ToString().ToLower()),
            new Literal(type.IsArray.ToString().ToLower()),
            new Literal(type.IsSealed.ToString().ToLower()),
            new Literal(type.IsInterface.ToString().ToLower()),
            new Literal(type.IsGenericTypeParameter.ToString().ToLower()),
            new Literal(type.IsGenericTypeDefinition.ToString().ToLower()),
            new Literal(type.IsGenericType.ToString().ToLower()),
            new Literal(type.IsGenericMethodParameter.ToString().ToLower()),
            new Literal(type.IsConstructedGenericType.ToString().ToLower()),
            new Literal(type.IsImport.ToString().ToLower()),
            new Literal(type.IsClass.ToString().ToLower()),
            new Literal(type.IsCollectible.ToString().ToLower()),
            new Literal(type.IsByRef.ToString().ToLower()),
            new Literal(type.IsByRefLike.ToString().ToLower()),
            new Literal(type.IsAutoClass.ToString().ToLower()),
            new Literal(type.IsAutoLayout.ToString().ToLower()),
            new Literal(type.IsCOMObject.ToString().ToLower()),
            new Literal(type.IsContextful.ToString().ToLower()),
            new Literal(type.IsEnum.ToString().ToLower()),
            new Literal(type.IsExplicitLayout.ToString().ToLower()),
            new Literal(type.IsPointer.ToString().ToLower()),
            new Literal(type.IsFunctionPointer.ToString().ToLower()),
            new Literal(type.IsUnmanagedFunctionPointer.ToString().ToLower()),
            new Literal(type.IsLayoutSequential.ToString().ToLower()),
            new Literal(type.IsMarshalByRef.ToString().ToLower()),
            new Literal(type.IsNested.ToString().ToLower()),
            new Literal(type.IsNestedAssembly.ToString().ToLower()),
            new Literal(type.IsNestedFamily.ToString().ToLower()),
            new Literal(type.IsNestedFamANDAssem.ToString().ToLower()),
            new Literal(type.IsNestedFamORAssem.ToString().ToLower()),
            new Literal(type.IsNestedPrivate.ToString().ToLower()),
            new Literal(type.IsNestedPublic.ToString().ToLower()),
            new Literal(type.IsNotPublic.ToString().ToLower()),
            new Literal(type.IsPublic.ToString().ToLower()),
            new Literal(type.IsSZArray.ToString().ToLower()),
            new Literal(type.IsSecurityCritical.ToString().ToLower()),
            new Literal(type.IsSecuritySafeCritical.ToString().ToLower()),
            new Literal(type.IsSecurityTransparent.ToString().ToLower()),
            new Literal(type.IsSignatureType.ToString().ToLower()),
            new Literal(type.IsSpecialName.ToString().ToLower()),
            new Literal(type.IsTypeDefinition.ToString().ToLower()),
            new Literal(type.IsUnicodeClass.ToString().ToLower()),
            new Literal(type.IsValueType.ToString().ToLower()),
            new Literal(type.IsVariableBoundArray.ToString().ToLower()),
            new Literal(type.IsVisible.ToString().ToLower()),
            type != type.UnderlyingSystemType ? CreateTypeInfo(type.UnderlyingSystemType) : Void(),
            type.BaseType != null ? CreateTypeInfo(type.BaseType) : Void(),
            type.DeclaringType != null ? CreateTypeInfo(type.DeclaringType) : Void(),
            new Literal(type.ContainsGenericParameters.ToString().ToLower()),
            new TableInitializer(type.GenericTypeArguments.Select(CreateTypeInfo).OfType<Expression>().ToList()),
            new Literal($"\"{type.GUID}\"")
        ];

        return new TableInitializer(values, keys);
    }

    public static TableInitializer CreateConstructorInfo(ConstructorInfo type)
    {
        List<Expression> keys =
        [
            new Literal("Name")
        ];
        List<Expression> values =
        [
            new Literal(type.Name)
        ];
        return new TableInitializer(values, keys);
    }

    public static ArgumentList CreateArgumentList(List<Expression> arguments)
    {
        return new ArgumentList(arguments.ConvertAll(expression => new Argument(expression)));
    }

    public static QualifiedName QualifiedNameFromMemberAccess(MemberAccess memberAccess)
    {
        var left = memberAccess.Expression is MemberAccess leftMemberAccess
            ? QualifiedNameFromMemberAccess(leftMemberAccess)
            : (Name)memberAccess.Expression;

        return new QualifiedName(left, memberAccess.Name);
    }

    public static Node DiscardVariableIfExpressionStatement(SyntaxNode node, Node value, SyntaxNode? valueParent)
    {
        return valueParent is ExpressionStatementSyntax ? DiscardVariable(node, (Expression)value) : value;
    }

    public static Variable DiscardVariable(SyntaxNode node, Expression value)
    {
        return new Variable(CreateIdentifierName(node, "_"), true, value);
    }

    public static IdentifierName CreateIdentifierName(SyntaxNode node)
    {
        return CreateIdentifierName(node, Utility.GetNamesFromNode(node).First());
    }

    public static IdentifierName CreateIdentifierName(SyntaxNode node, string name, bool bypassReserved = false)
    {
        if (Constants.ReservedIdentifiers.Contains(name) && !bypassReserved)
            Logger.UnsupportedError(node, $"Using '{name}' as an identifier", true, false);

        return new IdentifierName(name);
    }

    public static TypeRef? CreateTypeRef(string? typePath)
    {
        switch (typePath)
        {
            case null:
            case "var":
                return null;
        }

        var mappedType = Utility.GetMappedType(typePath);
        var arrayMatch = MyRegex().Match(mappedType);
        if (mappedType.EndsWith('?'))
            return new OptionalType(CreateTypeRef(mappedType.Replace("?", ""))!);
        return arrayMatch.Success
            ? new ArrayType(CreateTypeRef(arrayMatch.Value.Trim())!)
            : new TypeRef(mappedType, true);
    }

    public static TypeRef? CreateTypeRef(TypeSyntax? type)
    {
        return CreateTypeRef(type?.ToString());
    }

    public static Literal Void()
    {
        return new Literal("void");
    }

    [GeneratedRegex(@"\{[a-zA-Z0-9]+\}")]
    private static partial Regex MyRegex();
}