using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class GestureDrag
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_gesture_drag_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GestureDragHandle New();

    public static GestureDragHandle OnDragBegin(this GestureDragHandle gestureDrag, Action<double, double> onDragBegin)
        => gestureDrag.SideEffect(g => Gtk.SignalConnect<DragGestureDelegate>(g, "drag-begin", 
            (IntPtr _, double x, double y, IntPtr __)  => onDragBegin(x, y)));

    public static GestureDragHandle OnDragUpdate(this GestureDragHandle gestureDrag, Action<double, double> onDragUpdate)
        => gestureDrag.SideEffect(g => Gtk.SignalConnect<DragGestureDelegate>(g, "drag-update", 
            (IntPtr _, double x, double y, IntPtr __)  => onDragUpdate(x, y)));

    public static GestureDragHandle OnDragEnd(this GestureDragHandle gestureDrag, Action<double, double> onDragEnd)
        => gestureDrag.SideEffect(g => Gtk.SignalConnect<DragGestureDelegate>(g, "drag-end", 
            (IntPtr _, double x, double y, IntPtr __)  => onDragEnd(x, y)));
}

