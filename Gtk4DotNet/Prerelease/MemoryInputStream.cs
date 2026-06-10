using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public static class MemoryInputStream
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MemoryInputStreamHandle New();

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    public extern static MemoryInputStreamHandle New(GBytes bytes);

    [DllImport(Libs.LibGio, EntryPoint = "g_memory_input_stream_add_bytes", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddBytes(this MemoryInputStreamHandle stream, GBytes bytes);
}

public class MemoryInputStreamHandle : InputStream
{
    public MemoryInputStreamHandle() : base() {}
}