using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GFileEnumerator
{
    [DllImport(Globals.LibGtk, EntryPoint = "g_file_enumerator_next_file", CallingConvention = CallingConvention.Cdecl)]   
    public extern static IntPtr NextFile(IntPtr enumerator, IntPtr cancellable, IntPtr error);
}