using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Overlay
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_overlay_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static OverlayHandle New();

    public static OverlayHandle Child(this OverlayHandle overlay, WidgetHandle child)
        => overlay.SideEffect(n => n.SetChild(child));

    public static OverlayHandle AddOverlay(this OverlayHandle overlay, WidgetHandle child)
        => overlay.SideEffect(n => n._AddOverlay(child));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_overlay_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this OverlayHandle overlay, WidgetHandle child);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_overlay_add_overlay", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddOverlay(this OverlayHandle overlay, WidgetHandle widget);
}

