using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class ClickGesture : SingleGesture
{
    public static ClickGesture New()
    {
        var click = _New();
        click.CheckDiagnostics();
        return click;
    }

    public void OnPressed(Action<int, double, double> pressed)
        => SignalConnect<PressedGestureDelegate>("pressed", (nint _, int pressCount, double x, double y, nint __)  => pressed(pressCount, x, y));


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ClickGesture _New();
}