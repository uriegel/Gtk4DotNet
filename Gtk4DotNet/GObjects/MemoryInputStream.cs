using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MemoryInputStream : InputStream
{
    public MemoryInputStream()
    {
        SetInternalHandle(New());
        CheckDiagnostics();
    }

    public MemoryInputStream(GBytes bytes)
    {
        SetInternalHandle(New(bytes));
        CheckDiagnostics();
    } 

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(GBytes bytes);

    [DllImport(Libs.LibGio, EntryPoint = "g_memory_input_stream_add_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddBytes(MemoryInputStream stream, GBytes bytes);
}

