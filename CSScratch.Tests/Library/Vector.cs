using static CSScratch.Library.Scratch;
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace CSScratch.Tests.Library;

public class Vector
{
    public struct Vector2
    {
        public int X;
        public int Y;
    }

    public static Vector2 Create(int x, int y)
    {
        var vector = new Vector2();
        vector.X = x;
        vector.Y = y;
        return vector;
    }

    public static Vector2 Add(Vector2 a, Vector2 b)
    {
        var vector = new Vector2();
        vector.X = a.X + b.X;
        vector.Y = a.Y + b.Y;
        return vector;
    }

    public static Vector2 Subtract(Vector2 a, Vector2 b)
    {
        var vector = new Vector2();
        vector.X = a.X - b.X;
        vector.Y = a.Y - b.Y;
        return vector;
    }

    public static Vector2 Multiply(Vector2 a, int scalar)
    {
        var vector = new Vector2();
        vector.X = a.X * scalar;
        vector.Y = a.Y * scalar;
        return vector;
    }
    public static Vector2 Divide(Vector2 a, int scalar)
    {
        var vector = new Vector2();
        vector.X = a.X / scalar;
        vector.Y = a.Y / scalar;
        return vector;
    }

    public static int Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static int Length(Vector2 a)
    {
        return sqrt(a.X * a.X + a.Y * a.Y);
    }

    public static Vector2 Distance(Vector2 a, Vector2 b)
    {
        var vector = new Vector2();
        vector.X = b.X - a.X;
        vector.Y = b.Y - a.Y;
        return vector;
    }
}