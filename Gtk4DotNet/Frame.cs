using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Frame
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_frame_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static FrameHandle New(string? label = null);

    public static FrameHandle Child(this FrameHandle frame, WidgetHandle widget)
        => frame.SideEffect(b => b.SetChild(widget));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_frame_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this FrameHandle frame, WidgetHandle widget);
}