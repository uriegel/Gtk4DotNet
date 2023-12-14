using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class GestureSingle
{
    public static T Button<T>(this T gestureSingle, MouseButton button)
        where T : GestureSingleHandle
        => gestureSingle.SideEffect(g => g.SetButton(button));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_gesture_single_set_button", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetButton(this GestureSingleHandle gestureSingle, MouseButton button);
}

