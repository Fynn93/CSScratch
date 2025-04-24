#region

using static CSScratch.Library.Scratch;

#endregion

namespace CSScratch.Tests;

///gs list FontData = file ```font.txt```;

public class Text
{
    public static void PrintChar(char character)
    {

    }

    public static void Print(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            PrintChar(text[i]);
        }
    }

    public static void SetFont(string fontName)
    {
        // TODO: Implement
    }
}