using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Native
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_native_get_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static SurfaceHandle GetSurface(NativeHandle native);
}