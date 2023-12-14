using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ScrolledWindow
{
    public static ScrolledWindowHandle New() => New(IntPtr.Zero, IntPtr.Zero);

    public static ScrolledWindowHandle Child(this ScrolledWindowHandle scrolledWindow, WidgetHandle widget)
        => scrolledWindow.SideEffect(s => s.SetChild(widget));
        
    public static ScrolledWindowHandle Policy(this ScrolledWindowHandle scrolledWindow, PolicyType horizontal, PolicyType vertical)
        => scrolledWindow.SideEffect(s => s.SetPolicy(horizontal, vertical));

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_set_min_content_width", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetMinContentWidth(this IntPtr scrolledWindow, int width);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this ScrolledWindowHandle scrolledWindow, WidgetHandle widget);
    
    // [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_get_child", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr GetChild(IntPtr scrolledWindow);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ScrolledWindowHandle New(IntPtr zero, IntPtr zero2);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_set_policy", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPolicy(this ScrolledWindowHandle scrolledWindow, PolicyType horizontal, PolicyType vertical);

    // public static StackHandle AddChild(this StackHandle stack, WidgetHandle widget)
    //     => stack.SideEffect(s => s._AddChild(widget));

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_add_child", CallingConvention = CallingConvention.Cdecl)]
    // extern static void _AddChild(this StackHandle grid, WidgetHandle widget);
}

