using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GList
{
    [DllImport(Globals.LibGtk, EntryPoint = "g_list_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(IntPtr list);

    [DllImport(Globals.LibGtk, EntryPoint = "g_list_length", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetLength(IntPtr list);

    [DllImport(Globals.LibGtk, EntryPoint = "g_list_nth_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetData(IntPtr list, int index);
}

