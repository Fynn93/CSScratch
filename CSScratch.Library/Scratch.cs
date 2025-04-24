// ReSharper disable InconsistentNaming
// ReSharper disable ParameterHidesMember

namespace CSScratch.Library;

public abstract class Scratch
{
    #region REPORTERS - Sound

    public static dynamic? volume;

    #endregion
    #region UNARY OPERATORS

    public static dynamic not(dynamic operand)
    {
        return null!;
    }

    public static dynamic length(dynamic string_)
    {
        return null!;
    }

    public static dynamic round(dynamic num)
    {
        return null!;
    }

    public static dynamic abs(dynamic num)
    {
        return null!;
    }

    public static dynamic floor(dynamic num)
    {
        return null!;
    }

    public static dynamic ceil(dynamic num)
    {
        return null!;
    }

    public static dynamic sqrt(dynamic num)
    {
        return null!;
    }

    public static dynamic sin(dynamic num)
    {
        return null!;
    }

    public static dynamic cos(dynamic num)
    {
        return null!;
    }

    public static dynamic tan(dynamic num)
    {
        return null!;
    }

    public static dynamic asin(dynamic num)
    {
        return null!;
    }

    public static dynamic acos(dynamic num)
    {
        return null!;
    }

    public static dynamic atan(dynamic num)
    {
        return null!;
    }

    public static dynamic ln(dynamic num)
    {
        return null!;
    }

    public static dynamic log(dynamic num)
    {
        return null!;
    }

    public static dynamic antiln(dynamic num)
    {
        return null!;
    }

    public static dynamic antilog(dynamic num)
    {
        return null!;
    }

    public static dynamic minus(dynamic num)
    {
        return null!;
    }

    #endregion
    #region BINARY OPERATORS

    public static void Add(dynamic num1, dynamic num2)
    {
    }

    public static void Sub(dynamic operand1, dynamic operand2)
    {
    }

    public static void Mul(dynamic operand1, dynamic operand2)
    {
    }

    public static void Div(dynamic operand1, dynamic operand2)
    {
    }

    public static void Mod(dynamic operand1, dynamic operand2)
    {
    }

    public static void Lt(dynamic operand1, dynamic operand2)
    {
    }

    public static void Gt(dynamic operand1, dynamic operand2)
    {
    }

    public static void Eq(dynamic operand1, dynamic operand2)
    {
    }

    public static void And(dynamic operand1, dynamic operand2)
    {
    }

    public static void Or(dynamic operand1, dynamic operand2)
    {
    }

    public static void Join(dynamic string1, dynamic string2)
    {
    }

    public static void In(dynamic string2, dynamic string1)
    {
    }

    public static void Of(dynamic string_, dynamic letter)
    {
    }

    #endregion
    #region BLOCKS - Motion

    public static void move(dynamic steps)
    {
    }

    public static void turn_left(dynamic degrees)
    {
    }

    public static void turn_right(dynamic degrees)
    {
    }

    public static void goto_random_position()
    {
    }

    public static void goto_mouse_pointer()
    {
    }

    public static void goto_xy(dynamic x, dynamic y)
    {
    }

    public static void glide(dynamic secs, dynamic x, dynamic y)
    {
    }

    public static void glide_to_random_position(dynamic secs)
    {
    }

    public static void glide_to_mouse_pointer()
    {
    }

    public static void glide(dynamic to, dynamic secs)
    {
    }

    public static void point_in_direction(dynamic direction)
    {
    }

    public static void point_towards_mouse_pointer()
    {
    }

    public static void point_towards_random_direction()
    {
    }

    public static void point_towards(dynamic towards)
    {
    }

    public static void change_x(dynamic dx)
    {
    }

    public static void set_x(dynamic x)
    {
    }

    public static void change_y(dynamic dy)
    {
    }

    public static void set_y(dynamic y)
    {
    }

    public static void if_on_edge_bounce()
    {
    }

    public static void set_rotation_style_left_right()
    {
    }

    public static void set_rotation_style_do_not_rotate()
    {
    }

    public static void set_rotation_style_all_around()
    {
    }

    #endregion
    #region BLOCKS - Looks

    public static void say(dynamic message, dynamic secs)
    {
    }

    public static void think(dynamic message, dynamic secs)
    {
    }

    public static void say(dynamic message)
    {
    }

    public static void think(dynamic message)
    {
    }

    public static void switch_costume(dynamic costume)
    {
    }

    public static void next_costume()
    {
    }

    public static void switch_backdrop(dynamic backdrop)
    {
    }

    public static void previous_backdrop(dynamic backdrop)
    {
    }

    public static void random_backdrop()
    {
    }

    public static void next_backdrop()
    {
    }

    public static void set_size(dynamic size)
    {
    }

    public static void change_size(dynamic change)
    {
    }

    public static void show()
    {
    }

    public static void hide()
    {
    }

    public static void goto_front()
    {
    }

    public static void goto_back()
    {
    }

    #endregion
    #region BLOCKS - Sound

    public static void play_sound_until_done(dynamic sound_menu)
    {
    }

    public static void start_sound(dynamic sound)
    {
    }

    public static void stop_all_sounds()
    {
    }

    public static void change_volume(dynamic volume)
    {
    }

    public static void set_volume(dynamic volume)
    {
    }

    public static void clear_sound_effects()
    {
    }

    #endregion
    #region BLOCKS - Event

    public static void broadcast(dynamic message)
    {
    }

    public static void broadcast_and_wait(dynamic message)
    {
    }

    #endregion
    #region BLOCKS - Control

    public static void wait(dynamic duration)
    {
    }

    public static void wait_until(dynamic condition)
    {
    }

    public static void stop_all()
    {
    }

    public static void stop_this_script()
    {
    }

    public static void stop_other_scripts()
    {
    }

    public static void delete_this_clone()
    {
    }

    public static void clone()
    {
    }

    public static void clone(dynamic clone_option)
    {
    }

    #endregion
    #region BLOCKS - Sensing

    public static void ask(dynamic question)
    {
    }

    public static void set_drag_mode_draggable()
    {
    }

    public static void set_drag_mode_not_draggable()
    {
    }

    public static void reset_timer()
    {
    }

    #endregion
    #region BLOCKS - Pen

    public static void erase_all()
    {
    }

    public static void stamp()
    {
    }

    public static void pen_down()
    {
    }

    public static void pen_up()
    {
    }

    public static void set_pen_color(dynamic color)
    {
    }

    public static void change_pen_size(dynamic size)
    {
    }

    public static void set_pen_size(dynamic size)
    {
    }

    public static void set_pen_hue(dynamic value)
    {
    }

    public static void set_pen_saturation(dynamic value)
    {
    }

    public static void set_pen_brightness(dynamic value)
    {
    }

    public static void set_pen_transparency(dynamic value)
    {
    }

    public static void change_pen_hue(dynamic value)
    {
    }

    public static void change_pen_saturation(dynamic value)
    {
    }

    public static void change_pen_brightness(dynamic value)
    {
    }

    public static void change_pen_transparency(dynamic value)
    {
    }

    #endregion
    #region BLOCKS - Variable
    public static void show(dynamic variable)
    {
    }

    public static void hide(dynamic variable)
    {
    }
    #endregion
    #region REPORTERS - Motion

    public static dynamic x_position()
    {
        return null!;
    }

    public static dynamic y_position()
    {
        return null!;
    }

    public static dynamic direction()
    {
        return null!;
    }

    #endregion
    #region REPORTERS - Looks

    public static dynamic size()
    {
        return null!;
    }

    public static dynamic costume_number()
    {
        return null!;
    }

    public static string costume_name()
    {
        return null!;
    }

    public static dynamic backdrop_number()
    {
        return null!;
    }

    public static string backdrop_name()
    {
        return null!;
    }

    #endregion
    #region REPORTERS - Sensing

    public static dynamic distance_to_mouse_pointer()
    {
        return null!;
    }

    public static dynamic distance_to()
    {
        return null!;
    }

    public static dynamic touching_mouse_pointer()
    {
        return null!;
    }

    public static dynamic touching_edge()
    {
        return null!;
    }

    public static dynamic touching()
    {
        return null!;
    }

    public static dynamic key_pressed()
    {
        return null!;
    }

    public static dynamic mouse_down()
    {
        return null!;
    }

    public static dynamic mouse_x()
    {
        return null!;
    }

    public static dynamic mouse_y()
    {
        return null!;
    }

    public static dynamic loudness()
    {
        return null!;
    }

    public static dynamic timer()
    {
        return null!;
    }

    public static dynamic current_year()
    {
        return null!;
    }

    public static dynamic current_month()
    {
        return null!;
    }

    public static dynamic current_date()
    {
        return null!;
    }

    public static string current_day_of_week()
    {
        return null!;
    }

    public static dynamic current_hour()
    {
        return null!;
    }

    public static dynamic current_minute()
    {
        return null!;
    }

    public static dynamic current_second()
    {
        return null!;
    }

    public static dynamic days_since_2000()
    {
        return null!;
    }

    public static string username()
    {
        return null!;
    }

    public static dynamic touching_color()
    {
        return null!;
    }

    public static dynamic color_is_touching_color()
    {
        return null!;
    }

    #endregion
    #region REPORTERS - Operator

    public static dynamic random(dynamic from, dynamic to)
    {
        return 0;
    }

    public static dynamic contains(dynamic string1, dynamic string2)
    {
        return false;
    }

    #endregion
    #region VARIABLES - Lists
    public static readonly List<dynamic> Stack = null!;
    #endregion
}

public enum ScratchEventType
{
    None,
    // standard events
    Message,
    Flag,
    Key,
    Click,
    Backdrop,
    Loudness,
    Timer,
    Clone,
    // non-standard events
    Tick,
    Initialize,
}