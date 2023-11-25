using System.Runtime.InteropServices;
using GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
struct GList {
    IntPtr data;
    IntPtr next;
    IntPtr prev;

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_free ", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr Free(IntPtr list);
}