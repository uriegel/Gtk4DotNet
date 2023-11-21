using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class GestureSingle
{
    public static GestureSingleHandle Button(this GestureSingleHandle gestureSingle, MouseButton button)
        => gestureSingle.SideEffect(g => g.SetButton(button));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_gesture_single_set_button", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetButton(this GestureSingleHandle gestureSingle, MouseButton button);
}

