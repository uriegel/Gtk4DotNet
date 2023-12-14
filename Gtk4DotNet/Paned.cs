using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Paned
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static PanedHandle New(Orientation orientation);

    public static PanedHandle StartChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink)
        => paned.SideEffect(p => p.SetStartChild(widget, resize, shrink));

    public static PanedHandle EndChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink)
        => paned.SideEffect(p => p.SetEndChild(widget, resize, shrink));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_start_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetStartChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_end_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetEndChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink);
}