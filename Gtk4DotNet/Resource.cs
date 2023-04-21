using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Resource
{
    /// <summary>
    /// The caller of the function takes ownership of the data, and is responsible for freeing it.
    /// </summary>
    /// <param name="gbytes"></param>
    /// <returns></returns>
    public static IntPtr NewFromData(IntPtr gbytes) => NewFromData(gbytes, IntPtr.Zero);

    [DllImport(Globals.LibGtk, EntryPoint="g_resource_unref", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Unref(IntPtr res);

    [DllImport(Globals.LibGtk, EntryPoint="g_resources_register", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Register(IntPtr res);

    public static bool GetInfo(string path, int none, out long size, out int flags)
        => GetInfo(path, none, out size, out flags, IntPtr.Zero);

    [DllImport(Globals.LibGtk, EntryPoint="g_resources_get_info", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetInfo(string path, int none, out long size, out int flags, IntPtr zero);

    /// <summary>
    /// Opens a new GInputStream from resource. Free the returned object with GObject.Unref()
    /// </summary>
    /// <param name="path"></param>
    /// <param name="none"></param>
    /// <param name="zero"></param>
    /// <returns></returns>
    [DllImport(Globals.LibGtk, EntryPoint="g_resources_open_stream", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr OpenStream(string path, int none, IntPtr zero);

    [DllImport(Globals.LibGtk, EntryPoint="g_resource_new_from_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromData(IntPtr gbytes, IntPtr zero);
}
