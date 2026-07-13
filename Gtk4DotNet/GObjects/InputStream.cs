using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class InputStream : GObject
{
    public int Read(nint buffer, int count) => Read(this, buffer, count);
    
    public InputStream() : base() {}

    [DllImport(Libs.LibGtk, EntryPoint = "g_input_stream_read", CallingConvention = CallingConvention.Cdecl)]
    extern static int Read(InputStream stream, nint buffer, int count);
}