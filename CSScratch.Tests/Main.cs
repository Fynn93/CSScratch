﻿#region

using CSScratch.Library;
using static CSScratch.Library.Scratch;

#endregion

namespace CSScratch.Tests;

public class Main
{
    /*private static int _turns = 0;
    private static int _range = 0;
    private static int _color = 0;
    [ScratchEvent(ScratchEventType.Initialize)]
    private static void Initialize()
    {
        pen_up();
        erase_all();
        _turns = 0;
        _range = 100;
        _color = 0;
        show(_turns);
        show(_range);
        show(_color);
    }

    [ScratchEvent(ScratchEventType.Tick)]
    private static void Tick()
    {
        for (var i = 0; i < 100; i++)
        {
            for (var i2 = 0; i2 < 100; i2++)
            {
                for (var i3 = 0; i3 < 360; i3++)
                {
                    change_pen_saturation(99);
                    change_pen_hue(_color);
                    pen_down();
                    goto_xy(sin(_turns) * _range, cos(_turns) * _range);
                    goto_xy(0, 0);
                    _turns++;
                }
                _range--;
            }
            _range++;
        }
    }*/
    /*public static void DrawCircle(int x, int y, int radius, int color)
    {
        goto_xy(x, y);
        for (var i = 0; i < 360; i++)
        {
            goto_xy(x + radius * sin(i), y + radius * cos(i));
            set_pen_hue(color);
            pen_down();
        }
        pen_up();
    }

    private static int _x = 0;
    [ScratchEvent(ScratchEventType.Tick)]
    private static void Draw()
    {
        _x++;
        DrawCircle(random(-240, 240), random(-180, 180), random(0, 100), _x);
    }

    [ScratchEvent(ScratchEventType.Initialize)]
    private static void Initialize()
    {
        pen_up();
        erase_all();
        _x = 0;
    }*/

    public static void TestMethod(dynamic name)
    {
        say("Hello ".Concat(name));
    }


    [ScratchEvent(ScratchEventType.Initialize)]
    private static void Initialize()
    {
        Text.Print("Hello World");
    }
}