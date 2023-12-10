using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Popover
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_popover_new", CallingConvention = CallingConvention.Cdecl)]
    public static extern PopoverHandle New();
}