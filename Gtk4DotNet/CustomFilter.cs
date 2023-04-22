using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class CustomFilter
{
    public static IntPtr New(FilterFunc filter) => New(filter, IntPtr.Zero, IntPtr.Zero);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_custom_filter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New(FilterFunc filter, IntPtr zero, IntPtr zero2);

    public delegate bool FilterFunc(IntPtr item, IntPtr zero);
}
