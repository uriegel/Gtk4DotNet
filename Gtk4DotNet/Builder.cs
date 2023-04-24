using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Builder
{
    public static int AddFromFile(this IntPtr builder, string file) => AddFromFile(builder, file, IntPtr.Zero);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_builder_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_builder_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr FromResource(string path);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetObject(this IntPtr builder, string objectName);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_builder_connect_signals_full", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr ConnectSignals(this IntPtr builder, ConnectDelegate onConnection);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_builder_add_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static int AddFromFile(this IntPtr builder, string file, IntPtr nil);
}

