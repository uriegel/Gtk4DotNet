using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class GestureClick
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GestureClickHandle New();

    public static GestureClickHandle OnPressed(this GestureClickHandle gestureClick, Action<int, double, double> pressed)
    {
        void Pressed(IntPtr _, int pressCount, double x, double y, IntPtr __)  => pressed(pressCount, x, y);
        return gestureClick.SideEffect(g => Gtk.SignalConnect(g, "pressed", Marshal.GetFunctionPointerForDelegate((PressedGestureDelegate)Pressed), IntPtr.Zero, IntPtr.Zero, 0));
    }

}

