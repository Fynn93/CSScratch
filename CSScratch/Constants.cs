#region

using Microsoft.CodeAnalysis.CSharp;

#endregion

namespace CSScratch;

internal static class Constants
{
    public static readonly HashSet<string> DecimalTypes =
    [
        "float",
        "double",
        "Single",
        "Double"
    ];

    public static readonly HashSet<string> IntegerTypes =
    [
        "sbyte",
        "byte",
        "short",
        "ushort",
        "int",
        "uint",
        "long",
        "ulong",
        "SByte",
        "Byte",
        "Int16",
        "Int32",
        "Int64",
        "Int128",
        "UInt16",
        "UInt32",
        "UInt64",
        "UInt128"
    ];

    public static readonly HashSet<string> ReservedIdentifiers =
    [
        "CS"
    ];
}