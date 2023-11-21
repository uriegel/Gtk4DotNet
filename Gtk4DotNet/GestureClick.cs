using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GestureClick
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GestureClickHandle New();
}

