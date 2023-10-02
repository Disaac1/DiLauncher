using Godot;

public partial class GodotLogger : Node
{

    private static Node _wrapper = GD.Load<GDScript>("res://Launcher/loggerWrapper.gd").New().As<Node>();


    public static void info(string message)
    {
        _wrapper.Call("info", message);
    }

    public static void debug(string message)
    {
        _wrapper.Call("debug", message);
    }

    public static void warn(string message)
    {
        _wrapper.Call("warn", message);
    }

    public static void error(string message)
    {
        _wrapper.Call("error", message);
    }

    public static void fatal(string message)
    {
        _wrapper.Call("fatal", message);
    }
}