using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class GestureDrag
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_gesture_drag_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GestureDragHandle New();

    public static GestureDragHandle OnDragBegin(this GestureDragHandle gestureDrag, Action<double, double> onDragBegin)
    {
        void dragBegin(IntPtr _, double x, double y, IntPtr __)  => onDragBegin(x, y);
        return gestureDrag.SideEffect(g => Gtk.SignalConnect(g, "drag-begin", Marshal.GetFunctionPointerForDelegate((DragGestureDelegate)dragBegin), IntPtr.Zero, IntPtr.Zero, 0));
    }

    public static GestureDragHandle OnDragUpdate(this GestureDragHandle gestureDrag, Action<double, double> onDragUpdate)
    {
        void dragUpdate(IntPtr _, double x, double y, IntPtr __)  => onDragUpdate(x, y);
        return gestureDrag.SideEffect(g => Gtk.SignalConnect(g, "drag-update", Marshal.GetFunctionPointerForDelegate((DragGestureDelegate)dragUpdate), IntPtr.Zero, IntPtr.Zero, 0));
    }

    public static GestureDragHandle OnDragEnd(this GestureDragHandle gestureDrag, Action<double, double> onDragEnd)
    {
        void dragEnd(IntPtr _, double x, double y, IntPtr __)  => onDragEnd(x, y);
        return gestureDrag.SideEffect(g => Gtk.SignalConnect(g, "drag-end", Marshal.GetFunctionPointerForDelegate((DragGestureDelegate)dragEnd), IntPtr.Zero, IntPtr.Zero, 0));
    }
}

