using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class InputStream : GObject
{
    public int Read(nint buffer, int count) => Read(this, buffer, count);
    
    public InputStream() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Unref(handle));

    [DllImport(Libs.LibGtk, EntryPoint = "g_input_stream_read", CallingConvention = CallingConvention.Cdecl)]
    extern static int Read(InputStream stream, nint buffer, int count);
}