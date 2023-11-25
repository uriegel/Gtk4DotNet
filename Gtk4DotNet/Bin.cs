using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Bin
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_bin_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static WidgetHandle GetChild(this BinHandle bin);
}

