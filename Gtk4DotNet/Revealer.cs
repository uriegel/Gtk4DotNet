using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Revealer
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static RevealerHandle New();
}

