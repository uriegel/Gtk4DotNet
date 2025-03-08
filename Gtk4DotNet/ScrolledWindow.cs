using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ScrolledWindow
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ScrolledWindowHandle New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static THandle GetChild<THandle>(this ScrolledWindowHandle scrolledWindow)
        where THandle : WidgetHandle, new()
        => new THandle().SideEffect(h => h.SetInternalHandle(scrolledWindow._GetChild()));

    public static ScrolledWindowHandle Child(this ScrolledWindowHandle scrolledWindow, WidgetHandle widget)
        => scrolledWindow.SideEffect(s => s.SetChild(widget));

    public static void RemoveChild(this ScrolledWindowHandle scrolledWindow)
        => scrolledWindow._SetChild(0);

    public static ScrolledWindowHandle Policy(this ScrolledWindowHandle scrolledWindow, PolicyType horizontal, PolicyType vertical)
        => scrolledWindow.SideEffect(s => s.SetPolicy(horizontal, vertical));

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_set_min_content_width", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetMinContentWidth(this IntPtr scrolledWindow, int width);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this ScrolledWindowHandle scrolledWindow, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetChild(this ScrolledWindowHandle scrolledWindow, nint nil);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_set_policy", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPolicy(this ScrolledWindowHandle scrolledWindow, PolicyType horizontal, PolicyType vertical);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_scrolled_window_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint _GetChild(this ScrolledWindowHandle scrolledWindow);
}

