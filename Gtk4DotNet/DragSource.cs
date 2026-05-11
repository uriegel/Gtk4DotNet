using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class DragSource
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drag_source_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static DragSourceHandle New();

    public static DragSourceHandle SetActions(this DragSourceHandle dragSource, DragAction dragActions)
        => dragSource.SideEffect(n => n._SetActions(dragActions));

    public static DragSourceHandle ConnectPrepare(this DragSourceHandle dragSource, GtkDragSourcePrepare gtkDragSourcePrepare)
    {
        Gtk.SignalConnect(dragSource, "prepare", gtkDragSourcePrepare);
        return dragSource;
    }
        

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drag_source_set_actions", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetActions(this DragSourceHandle dragSource, DragAction dragAction);
}

public delegate nint GtkDragSourcePrepare(nint dragSource, double x, double y, nint userData);


