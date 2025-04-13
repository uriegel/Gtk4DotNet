using System.Runtime.InteropServices;
using GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
struct GList {
    public nint Data;
    public nint Next;
    public nint Prev;

    [DllImport(Libs.LibGio, EntryPoint = "g_list_free", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Free(nint list);
}