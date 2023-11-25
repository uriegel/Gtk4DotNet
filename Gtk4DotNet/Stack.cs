using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Stack
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static StackHandle New();

    public static StackHandle AddChild(this StackHandle stack, WidgetHandle widget)
        => stack.SideEffect(s => s._AddChild(widget));

    public static StackHandle AddTitled(this StackHandle stack, WidgetHandle widget, string name, string title)
        => stack.SideEffect(s => s._AddTitled(widget, name, title));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_get_visible_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static WidgetHandle GetVisibleChild(this StackHandle stack);

    public static THandle? GetVisibleChild<THandle>(this StackHandle stack)
        where THandle : WidgetHandle
        => stack.GetVisibleChild() as THandle;

    public static StackHandle OnVisibleChanged(this StackHandle stack, Action onChanged)
        => stack.SideEffect(s => Gtk.SignalConnect<ThreePointerDelegate>(s, "notify::visible-child", (IntPtr _, IntPtr __, IntPtr ___)  => onChanged()));


    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_add_titled", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddTitled(this StackHandle stack, WidgetHandle widget, string name, string title);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_add_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddChild(this StackHandle grid, WidgetHandle widget);

}

