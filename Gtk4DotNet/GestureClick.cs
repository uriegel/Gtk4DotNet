using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class GestureClick
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GestureClickHandle New();

    public static GestureClickHandle OnPressed(this GestureClickHandle gestureClick, Action<int, double, double> pressed)
        => gestureClick.SideEffect(g => Gtk.SignalConnect<PressedGestureDelegate>(g, "pressed", 
            (IntPtr _, int pressCount, double x, double y, IntPtr __)  => pressed(pressCount, x, y)));
}

