using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public static class Streamer
{
    public static void Init(int c = 0, nint n = 0) => gst_init(ref c, ref n);
    public static void Deinit() => gst_deinit();

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern void gst_init(ref int argc, ref IntPtr argv);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern void gst_deinit();
}