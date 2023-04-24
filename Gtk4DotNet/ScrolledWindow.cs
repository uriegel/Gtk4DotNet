using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class ScrolledWindow
{
    public static IntPtr New() => New(IntPtr.Zero, IntPtr.Zero);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_scrolled_window_set_policy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetPolicy(this IntPtr scrolledWindow, PolicyType horizontal, PolicyType vertical);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_scrolled_window_set_min_content_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetMinContentWidth(this IntPtr scrolledWindow, int width);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_scrolled_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(IntPtr scrolledWindow, IntPtr widget);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_scrolled_window_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetChild(IntPtr scrolledWindow);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_scrolled_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New(IntPtr zero, IntPtr zero2);
}

