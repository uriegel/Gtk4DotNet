using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MemoryInputStream : InputStream
{
    public static MemoryInputStream New()
    {
        var s = _New();
        var stream = new MemoryInputStream();
        stream.SetInternalHandle(s);
        stream.CheckDiagnostics();
        return stream;
    } 
    public static MemoryInputStream New(GBytes bytes)
    {
        var s = _New(bytes);
        var stream = new MemoryInputStream();
        stream.SetInternalHandle(s);
        stream.CheckDiagnostics();
        return stream;
    } 

    public MemoryInputStream() : base() {}

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _New();

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _New(GBytes bytes);

    [DllImport(Libs.LibGio, EntryPoint = "g_memory_input_stream_add_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddBytes(MemoryInputStream stream, GBytes bytes);
}

