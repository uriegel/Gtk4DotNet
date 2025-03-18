using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Paned
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static PanedHandle New(Orientation orientation);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static PanedHandle StartChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink)
        => paned.SideEffect(p => p.SetStartChild(widget, resize, shrink));

    public static PanedHandle EndChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink)
        => paned.SideEffect(p => p.SetEndChild(widget, resize, shrink));

    public static THandle GetStartChild<THandle>(this PanedHandle paned)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetStartChild(paned));
        return res;
    }

    public static THandle GetEndChild<THandle>(this PanedHandle paned)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetEndChild(paned));
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_position", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetPosition(this PanedHandle paned, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_start_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetStartChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_end_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetEndChild(this PanedHandle paned, WidgetHandle widget, bool resize, bool shrink);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_get_start_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetStartChild(this PanedHandle paned);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_get_end_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetEndChild(this PanedHandle paned);
}