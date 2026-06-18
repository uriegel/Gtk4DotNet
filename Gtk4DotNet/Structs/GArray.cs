using System.Runtime.InteropServices;

namespace Gtk4DotNet;

[StructLayout(LayoutKind.Sequential)]
struct GArray
{
    public IntPtr Data;
    public uint Len;
}